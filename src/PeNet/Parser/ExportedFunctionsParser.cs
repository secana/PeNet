using PeNet.Structures;
using PeNet.Utilities;

namespace PeNet.Parser
{
    internal class ExportedFunctionsParser : SafeParser<ExportFunction[]>
    {
        private readonly IMAGE_EXPORT_DIRECTORY _exportDirectory;
        private readonly IMAGE_SECTION_HEADER[] _sectionHeaders;
        private readonly IMAGE_DATA_DIRECTORY _exportDataDir;

        internal ExportedFunctionsParser(
            byte[] buff,
            IMAGE_EXPORT_DIRECTORY exportDirectory,
            IMAGE_SECTION_HEADER[] sectionHeaders,
            IMAGE_DATA_DIRECTORY exportDataDir
            )
            : base(buff, 0)
        {
            _exportDirectory = exportDirectory;
            _sectionHeaders = sectionHeaders;
            _exportDataDir = exportDataDir;
        }

        protected override ExportFunction[] ParseTarget()
        {
            if (_exportDirectory == null || _exportDirectory.AddressOfFunctions == 0)
                return null;

            var expFuncs = new ExportFunction[_exportDirectory.NumberOfFunctions];

            var funcOffsetPointer = _exportDirectory.AddressOfFunctions.RVAtoFileMapping(_sectionHeaders);
            var ordOffset = _exportDirectory.AddressOfNameOrdinals.RVAtoFileMapping(_sectionHeaders);
            var nameOffsetPointer = _exportDirectory.AddressOfNames.RVAtoFileMapping(_sectionHeaders);

            //Get addresses
            for (uint i = 0; i < expFuncs.Length; i++)
            {
                var ordinal = i + _exportDirectory.Base;
                var address = _buff.BytesToUInt32(funcOffsetPointer + sizeof(uint)*i);
                expFuncs[i] = new ExportFunction(null, address, (ushort) ordinal);
            }

            //Associate names
            for (uint i = 0; i < _exportDirectory.NumberOfNames; i++)
            {
                var namePtr = _buff.BytesToUInt32(nameOffsetPointer + sizeof(uint)*i);
                var nameAdr = namePtr.RVAtoFileMapping(_sectionHeaders);
                var name = _buff.GetCString(nameAdr);
                var ordinalIndex = (uint) _buff.GetOrdinal(ordOffset + sizeof(ushort)*i);

                if (IsForwardedExport(expFuncs[ordinalIndex].Address))
                {
                    var forwardNameAdr = expFuncs[ordinalIndex].Address.RVAtoFileMapping(_sectionHeaders);
                    var forwardName = _buff.GetCString(forwardNameAdr);

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