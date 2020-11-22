using PeNet.Header.Pe;
using System;
using System.Linq;

namespace PeNet
{
    public partial class PeFile
    {
        /// <summary>
        /// Add a new section to the PE file.
        /// </summary>
        /// <param name="name">Name of the section to add. At max. 8 characters.</param>
        /// <param name="unalignedSize">Size in bytes of the new section.</param>
        /// <param name="characteristics">Section characteristics.</param>
        public void AddSection(string name, int unalignedSize, ScnCharacteristicsType characteristics)
        {
            if (ImageNtHeaders is null)
                throw new Exception("IMAGE_NT_HEADERS must not be null.");
            if (ImageDosHeader is null)
                throw new Exception("IMAGE_DOS_HEADER must not be null");

            uint GetNewSizeOfImage()
            {
                var factor = unalignedSize / (double)ImageNtHeaders.OptionalHeader.SectionAlignment;
                var additionalSize = (uint)Math.Ceiling(factor) * ImageNtHeaders!.OptionalHeader.SectionAlignment;
                return ImageNtHeaders.OptionalHeader.SizeOfImage + additionalSize;
            }

            uint GetNewSecHeaderOffset()
            {
                var sizeOfSection = 0x28;
                var x = (uint)ImageNtHeaders!.FileHeader.SizeOfOptionalHeader + 0x18;
                var startOfSectionHeader = ImageDosHeader.E_lfanew + x;
                return (uint)(startOfSectionHeader + (ImageNtHeaders.FileHeader.NumberOfSections * sizeOfSection));
            }

            uint GetNewSecVa()
            {
                var lastSec      = ImageSectionHeaders!.OrderByDescending(sh => sh.VirtualAddress).First();
                var vaLastSecEnd = lastSec.VirtualAddress + lastSec.VirtualSize;
                var factor       = vaLastSecEnd / (double)ImageNtHeaders.OptionalHeader.SectionAlignment;
                return (uint)(Math.Ceiling(factor) * ImageNtHeaders.OptionalHeader.SectionAlignment);
            }

            uint GetNewRawSecSize()
            {
                var factor = unalignedSize / (double)ImageNtHeaders!.OptionalHeader.FileAlignment;
                return (uint) (Math.Ceiling(factor) * ImageNtHeaders!.OptionalHeader.FileAlignment);
            }

            // New raw section size, aligned to FileAlignment
            var newRawSectionSize = GetNewRawSecSize();


            // Append new section to end of file
            RawFile.AppendBytes(new byte[newRawSectionSize]);
            var paNewSec = ImageSectionHeaders!.Last().PointerToRawData + ImageSectionHeaders!.Last().SizeOfRawData;

            // Add new entry in section table
            var newSection = new ImageSectionHeader(RawFile, GetNewSecHeaderOffset(), ImageNtHeaders.OptionalHeader.ImageBase)
            {
                Name                 = name,
                VirtualSize          = (uint)newRawSectionSize,
                VirtualAddress       = GetNewSecVa(),
                SizeOfRawData        = newRawSectionSize,
                PointerToRawData     = paNewSec,
                PointerToRelocations = 0,
                PointerToLinenumbers = 0,
                NumberOfRelocations  = 0,
                NumberOfLinenumbers  = 0,
                Characteristics      = characteristics
            };

            // Increase number of sections
            ImageNtHeaders.FileHeader.NumberOfSections = (ushort)(ImageNtHeaders.FileHeader.NumberOfSections + 1);

            // Adjust image size by image alignment
            ImageNtHeaders.OptionalHeader.SizeOfImage = GetNewSizeOfImage();

            // Reparse section headers
            _nativeStructureParsers.ReparseSectionHeaders();
        }

        /// <summary>
        /// Remove a section from the PE file.
        /// </summary>
        /// <param name="name">Name of the section to remove.</param>
        /// <param name="removeContent">Flag if the content should be removed or only the section header entry.</param>
        public void RemoveSection(string name, bool removeContent = true)
        {
            var sectionToRemove = (ImageSectionHeaders ?? throw new InvalidOperationException("ImageSectionHeaders must not be null.")).First(s => s.Name == name);

            // Remove section from list of sections
            var newSections = ImageSectionHeaders.Where(s => s.Name != name).ToArray();

            // Change number of sections in the file header
            ImageNtHeaders!.FileHeader.NumberOfSections--;

            if (removeContent)
            {
                // Reloc the physical address of all sections
                foreach (var s in newSections)
                {
                    if (s.PointerToRawData > sectionToRemove.PointerToRawData)
                    {
                        s.PointerToRawData -= sectionToRemove.SizeOfRawData;
                    }
                }

                // Remove section content
                RawFile.RemoveRange(sectionToRemove.PointerToRawData, sectionToRemove.SizeOfRawData);
            }

            // Fix virtual size
            for (var i = 1; i < newSections.Length; i++)
            {
                if (newSections[i - 1].VirtualAddress < sectionToRemove.VirtualAddress)
                {
                    newSections[i - 1].VirtualSize = newSections[i].VirtualAddress - newSections[i - 1].VirtualAddress;
                }
            }

            // Replace old section headers with new section headers
            var sectionHeaderOffset = ImageDosHeader!.E_lfanew + ImageNtHeaders!.FileHeader.SizeOfOptionalHeader + 0x18;
            var sizeOfSection = 0x28;
            var newRawSections = new byte[newSections.Length * sizeOfSection];
            for (var i = 0; i < newSections.Length; i++)
            {
                Array.Copy(newSections[i].ToArray(), 0, newRawSections, i * sizeOfSection, sizeOfSection);
            }

            // Null the data directory entry if any available
            var de = ImageNtHeaders
                .OptionalHeader
                .DataDirectory
                .FirstOrDefault(d => d.VirtualAddress == sectionToRemove.VirtualAddress
                    && d.Size == sectionToRemove.VirtualSize);

            if (de != null)
            {
                de.Size = 0;
                de.VirtualAddress = 0;
            }

            // Null the old section headers
            RawFile.WriteBytes(sectionHeaderOffset, new byte[ImageSectionHeaders.Length * sizeOfSection]);

            // Write the new sections headers
            RawFile.WriteBytes(sectionHeaderOffset, newRawSections);

            // Reparse section header
            _nativeStructureParsers.ReparseSectionHeaders();
        }
    }
}
