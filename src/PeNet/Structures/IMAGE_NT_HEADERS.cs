using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The NT header is the main header for modern Windows applications.
    ///     It contains the file header and the optional header.
    /// </summary>
    public class IMAGE_NT_HEADERS : AbstractStructure
    {
        /// <summary>
        ///     Access to the File header.
        /// </summary>
        public readonly IMAGE_FILE_HEADER FileHeader;

        /// <summary>
        ///     Access to the Optional header.
        /// </summary>
        public readonly IMAGE_OPTIONAL_HEADER OptionalHeader;

        /// <summary>
        ///     Create a new IMAGE_NT_HEADERS object.
        /// </summary>
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset of the NT header.</param>
        public IMAGE_NT_HEADERS(byte[] buff, uint offset)
            : base(buff, offset)
        {
            FileHeader = new IMAGE_FILE_HEADER(buff, offset + 0x4);

            var is32Bit = FileHeader.Machine == (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_I386;

            OptionalHeader = new IMAGE_OPTIONAL_HEADER(buff, offset + 0x18, !is32Bit);
        }

        /// <summary>
        ///     NT header signature.
        /// </summary>
        public uint Signature
        {
            get => Buff.BytesToUInt32(Offset);
            set => Buff.SetUInt32(Offset, value);
        }
    }
}