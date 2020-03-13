using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class ExportedFunctionsParser : SafeParser<ExportFunction[]>
    {
        private readonly ImageExportDirectory? _exportDirectory;
        private readonly ImageSectionHeader[] _sectionHeaders;
        private readonly ImageDataDirectory _exportDataDir;

        internal ExportedFunctionsParser(
            IRawFile peFile,
            ImageExportDirectory? exportDirectory,
            ImageSectionHeader[] sectionHeaders,
            ImageDataDirectory exportDataDir
            )
            : base(peFile, 0)
        {
            _exportDirectory = exportDirectory;
            _sectionHeaders = sectionHeaders;
            _exportDataDir = exportDataDir;
        }

        protected override ExportFunction[]? ParseTarget()
        {
            if (_exportDirectory == null || _exportDirectory.AddressOfFunctions == 0)
                return null;

            var expFuncs = new ExportFunction[_exportDirectory.NumberOfFunctions];

            var funcOffsetPointer = _exportDirectory.AddressOfFunctions.RvaToOffset(_sectionHeaders);
            var ordOffset = _exportDirectory.NumberOfNames == 0 ? 0 : _exportDirectory.AddressOfNameOrdinals.RvaToOffset(_sectionHeaders);
            var nameOffsetPointer = _exportDirectory.NumberOfNames == 0 ? 0 : _exportDirectory.AddressOfNames.RvaToOffset(_sectionHeaders);

            //Get addresses
            for (uint i = 0; i < expFuncs.Length; i++)
            {
                var ordinal = i + _exportDirectory.Base;
                var address = PeFile.ReadUInt(funcOffsetPointer + sizeof(uint)*i);
                expFuncs[i] = new ExportFunction(null, address, (ushort) ordinal);
            }

            //Associate names
            for (uint i = 0; i < _exportDirectory.NumberOfNames; i++)
            {
                var namePtr = PeFile.ReadUInt(nameOffsetPointer + sizeof(uint)*i);
                var nameAdr = namePtr.RvaToOffset(_sectionHeaders);
                var name = PeFile.ReadAsciiString(nameAdr);
                var ordinalIndex = (uint) PeFile.ReadUShort(ordOffset + sizeof(ushort)*i);

                if (IsForwardedExport(expFuncs[ordinalIndex].Address))
                {
                    var forwardNameAdr = expFuncs[ordinalIndex].Address.RvaToOffset(_sectionHeaders);
                    var forwardName = PeFile.ReadAsciiString(forwardNameAdr);

                    expFuncs[ordinalIndex] = new ExportFunction(
                        name,
                        expFuncs[ordinalIndex].Address,
                        expFuncs[ordinalIndex].Ordinal,
                        forwardName);
                }
                else
                {
                    expFuncs[ordinalIndex] = new ExportFunction(
                        name, 
                        expFuncs[ordinalIndex].Address,
                        expFuncs[ordinalIndex].Ordinal);
                }
            }

            return expFuncs;
        }

        // Some exported functions are not directly provided by the DLL but forwarded to
        // another DLL, where the code resides. This functions test, if an export
        // is a forwarded one.
        private bool IsForwardedExport(uint address)
        {
            return _exportDataDir.VirtualAddress <= address 
                && address < _exportDataDir.VirtualAddress + _exportDataDir.Size;
        }
    }
}