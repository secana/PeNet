using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The thunk data is used by for the imports
    ///     in the import section.
    /// </summary>
    public class IMAGE_THUNK_DATA : AbstractStructure
    {
        private readonly bool _is64Bit;

        /// <summary>
        ///     Create a new IMAGE_THUNK_DATA object.
        /// </summary>
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset of the thunk data.</param>
        /// <param name="is64Bit">Set to true if the PE file is a x64 application.</param>
        public IMAGE_THUNK_DATA(byte[] buff, uint offset, bool is64Bit)
            : base(buff, offset)
        {
            _is64Bit = is64Bit;
        }

        /// <summary>
        ///     Points to the address in the IAT or to an
        ///     IMAGE_IMPORT_BY_NAME struct.
        /// </summary>
        public ulong AddressOfData
        {
            get => _is64Bit ? PeFile.BytesToUInt64(Offset) : PeFile.ReadUInt(Offset);
            set
            {
                if (!_is64Bit)
                    PeFile.WriteUInt(Offset, (uint) value);
                else
                    PeFile.SetUInt64(Offset, value);
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