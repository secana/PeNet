using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet2
{
    public class PeFile
    {
        public class ExportFunction
        {
            public string Name { get; private set; }
            public UInt32 Address { get; private set; }
            public UInt16 Ordinal { get; private set; }

            public ExportFunction(string name, UInt32 address, UInt16 ordinal)
            {
                Name = name;
                Address = address;
                Ordinal = ordinal;
            }
        }

        public IMAGE_DOS_HEADER ImageDosHeader { get; private set; }
        public IMAGE_NT_HEADERS ImageNtHeaders { get; private set; }
        public IMAGE_SECTION_HEADER[] ImageSectionHeaders { get; private set; }
        public IMAGE_EXPORT_DIRECTORY ImageExportDirectory { get; private set; }
        public ExportFunction[] ExportedFunctions { get; private set; }

        public PeFile(string peFile)
        {
            var buff = File.ReadAllBytes(peFile);

            ImageDosHeader = new IMAGE_DOS_HEADER(buff);
            ImageNtHeaders = new IMAGE_NT_HEADERS(buff, ImageDosHeader.e_lfanew);
            ImageSectionHeaders = ParseImageSectionHeaders(buff, ImageNtHeaders.FileHeader.NumberOfSections, ImageDosHeader.e_lfanew + 0xF8);
            
            if (ImageNtHeaders.OptionalHeader.DataDirectory[0].VirtualAddress != 0)
            {
                try
                {
                    ImageExportDirectory = new IMAGE_EXPORT_DIRECTORY(buff, Utility.RVAtoFileMapping(ImageNtHeaders.OptionalHeader.DataDirectory[0].VirtualAddress, ImageSectionHeaders));
                    ExportedFunctions = ParseExportedFunctions(buff, ImageExportDirectory, ImageSectionHeaders);
                }
                catch
                {
                    // No or invalid export directory.
                }
            }
        }

        private ExportFunction[] ParseExportedFunctions(byte[] buff, IMAGE_EXPORT_DIRECTORY ed, IMAGE_SECTION_HEADER[] sh)
        {
            var expFuncs = new ExportFunction[ed.NumberOfNames];
            var funcOffsetPointer = Utility.RVAtoFileMapping(ed.AddressOfFunctions, sh);
            var ordOffset = Utility.RVAtoFileMapping(ed.AddressOfNameOrdinals, sh);
            var nameOffsetPointer = Utility.RVAtoFileMapping(ed.AddressOfNames, sh);

            var funcOffset = Utility.BytesToUInt32(buff, funcOffsetPointer);

            for(UInt32 i = 0; i < expFuncs.Length; i++)
            {
                var namePtr = Utility.BytesToUInt32(buff, nameOffsetPointer + sizeof(UInt32) * i);
                var nameAdr = Utility.RVAtoFileMapping(namePtr, sh);
                var name = Utility.GetName(nameAdr, buff);
                var ordinalIndex = (UInt32)Utility.GetOrdinal(ordOffset + sizeof(UInt16) * i, buff);
                var ordinal = ordinalIndex + ed.Base;
                var address = Utility.BytesToUInt32(buff, funcOffsetPointer + sizeof(UInt32) * ordinalIndex);

                expFuncs[i] = new ExportFunction(name, address, (UInt16) ordinal);
            }

            return expFuncs;
        }

        private IMAGE_SECTION_HEADER[] ParseImageSectionHeaders(byte[] buff, UInt16 numOfSections, UInt32 offset)
        {
            var sh = new IMAGE_SECTION_HEADER[numOfSections];
            UInt32 secSize = 0x28; // Every section header is 40 bytes in size.
            for(UInt32 i = 0; i < numOfSections; i++)
            {
                sh[i] = new IMAGE_SECTION_HEADER(buff, offset + i * secSize);
            }

            return sh;
        }
    }
}
