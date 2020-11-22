using PeNet.Header.Pe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeNet
{
    public partial class PeFile
    {

        /// <summary>
        /// Add a new import to the PE file.
        /// If you intend to add multiple imports, use "AddImports" instead.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="function"></param>
        public void AddImport(string module, string function)
        {
            var ai = new AdditionalImport(module, new List<string> { function });
            AddImports(new List<AdditionalImport> { ai });
        }

        /// <summary>
        /// Add imports to the PE file.
        /// </summary>
        /// <param name="additionalImports">List with additional imports.</param>
        public void AddImports(List<AdditionalImport> additionalImports)
        {
            if (ImageNtHeaders is null || ImageSectionHeaders is null || _dataDirectoryParsers is null)
                throw new Exception("NT Headers, Section Headers and Data Directory must not be null.");

            const int sizeOfImpDesc = 0x14;
            var sizeOfThunkData = Is32Bit ? 4 : 8;
            var numAddImpDescs = additionalImports.Count;
            var importRva = ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Import].VirtualAddress;
            var importSize = ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Import].Size;

            int EstimateAdditionalNeededSpace()
                => (int)(additionalImports.Select(ai => ai.Functions).Count() * 64 + importSize);

            var newUnalignedRawSecSize = EstimateAdditionalNeededSpace();

            // First copy the current import descriptor array to the start of the new section to have enough space to
            // add additional import descriptors.
            AddSection(".addImp", (int)(newUnalignedRawSecSize), (ScnCharacteristicsType)0xC0000000);
            var newImpSec = ImageSectionHeaders.First(sh => sh.Name == ".addImp");
            var oldImpDescBytes = RawFile.AsSpan(importRva.RvaToOffset(ImageSectionHeaders), importSize);
            RawFile.WriteBytes(newImpSec.PointerToRawData, oldImpDescBytes);

            // Set the import data directory to the new import section and adjust the size
            ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Import].VirtualAddress = newImpSec.VirtualAddress;
            ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Import].Size = (uint)(importSize + (sizeOfImpDesc * numAddImpDescs));
            var newImportRva = ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Import].VirtualAddress;
            var newImportSize = ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Import].Size;

            var paAdditionalSpace = newImpSec.PointerToRawData + newImportSize;

            // Update import descriptors and imported functions to reflect the new 
            // position in the new section.
            _dataDirectoryParsers.ReparseImportDescriptors(ImageSectionHeaders);
            _dataDirectoryParsers.ReparseImportedFunctions();

            uint AddModName(ref uint offset, string module)
            {
                var tmp = Encoding.ASCII.GetBytes(module);
                var mName = new byte[tmp.Length + 1];
                Array.Copy(tmp, mName, tmp.Length);

                var paName = offset;
                RawFile.WriteBytes(offset, mName);

                offset = (uint)(offset + mName.Length);
                return paName;
            }

            List<uint> AddImpByNames(ref uint offset, List<string> funcs)
            {
                var adrList = new List<uint>();
                foreach (var f in funcs)
                {
                    var ibn = new ImageImportByName(RawFile, offset)
                    {
                        Hint = 0,
                        Name = f
                    };

                    adrList.Add(offset);

                    offset += (uint)ibn.Name.Length + 2;
                }

                // Add zero DWORD to end array
                RawFile.WriteUInt(offset + 1, 0);
                offset += 5;

                return adrList;
            }

            uint AddThunkDatas(ref uint offset, List<uint> adrList)
            {
                var paThunkStart = offset;

                foreach (var adr in adrList)
                {
                    _ = new ImageThunkData(RawFile, offset, Is64Bit)
                    {
                        AddressOfData = adr.OffsetToRva(ImageSectionHeaders!)
                    };

                    offset += (uint)sizeOfThunkData;
                }

                // End array with empty thunk data
                _ = new ImageThunkData(RawFile, offset, Is64Bit)
                {
                    AddressOfData = 0
                };

                offset += (uint)sizeOfThunkData;

                return paThunkStart;
            }

            void AddImportWithNewImpDesc(ref uint tmpOffset, ref long paIdesc, AdditionalImport ai)
            {
                var paName = AddModName(ref tmpOffset, ai.Module);
                var funcAdrs = AddImpByNames(ref tmpOffset, ai.Functions);
                var thunkAdrs = AddThunkDatas(ref tmpOffset, funcAdrs);

                _ = new ImageImportDescriptor(RawFile, paIdesc)
                {
                    Name = paName.OffsetToRva(ImageSectionHeaders),
                    OriginalFirstThunk = 0,
                    FirstThunk = thunkAdrs.OffsetToRva(ImageSectionHeaders),
                    ForwarderChain = 0,
                    TimeDateStamp = 0
                };
                paIdesc += (uint)sizeOfImpDesc;
            }

            var paIdesc = newImportRva.RvaToOffset(ImageSectionHeaders) + ImageImportDescriptors!.Length * sizeOfImpDesc;
            var tmpOffset = paAdditionalSpace;

            // Add new imports
            foreach(var ai in additionalImports)
            {
                AddImportWithNewImpDesc(ref tmpOffset, ref paIdesc, ai);
            }

            // End with zero filled idesc
            _ = new ImageImportDescriptor(RawFile, paIdesc)
            {
                Name = 0,
                OriginalFirstThunk = 0,
                FirstThunk = 0,
                ForwarderChain = 0,
                TimeDateStamp = 0
            };


            // Reparse imports
            _dataDirectoryParsers.ReparseImportDescriptors(ImageSectionHeaders);
            _dataDirectoryParsers.ReparseImportedFunctions();
        }
    }
}
