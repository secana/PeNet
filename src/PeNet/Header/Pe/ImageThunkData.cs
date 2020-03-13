using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The thunk data is used by for the imports
    ///     in the import section.
    /// </summary>
    public class ImageThunkData : AbstractStructure
    {
        private readonly bool _is64Bit;

        /// <summary>
        ///     Create a new ImageThunkData object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the thunk data.</param>
        /// <param name="is64Bit">Set to true if the PE file is a x64 application.</param>
        public ImageThunkData(IRawFile peFile, uint offset, bool is64Bit)
            : base(peFile, offset)
        {
            _is64Bit = is64Bit;
        }

        /// <summary>
        ///     Points to the address in the IAT or to an
        ///     ImageImportByName struct.
        /// </summary>
        public ulong AddressOfData
        {
            get => _is64Bit ? PeFile.ReadULong(Offset) : PeFile.ReadUInt(Offset);
            set
            {
                if (!_is64Bit)
                    PeFile.WriteUInt(Offset, (uint) value);
                else
                    PeFile.WriteULong(Offset, value);
            }
        }

        /// <summary>
        ///     Same as AddressOfFunction.
        /// </summary>
        public ulong Ordinal
        {
            get => AddressOfData;
            set => AddressOfData = value;
        }

        /// <summary>
        ///     Same as AddressOfFunction.
        /// </summary>
        public ulong ForwarderString
        {
            get => AddressOfData;
            set => AddressOfData = value;
        }

        /// <summary>
        ///     Same as AddressOfFunction.
        /// </summary>
        public ulong Function
        {
            get => AddressOfData;
            set => AddressOfData = value;
        }
    }
}