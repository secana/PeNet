using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The NT header is the main header for modern Windows applications.
    ///     It contains the file header and the optional header.
    /// </summary>
    public class ImageNtHeaders : AbstractStructure
    {
        /// <summary>
        ///     Access to the File header.
        /// </summary>
        public readonly ImageFileHeader FileHeader;

        /// <summary>
        ///     Access to the Optional header.
        /// </summary>
        public readonly ImageOptionalHeader OptionalHeader;

        /// <summary>
        ///     Create a new ImageNtHeaders object.
        /// </summary>
        /// <param name="peFile">A PE file .</param>
        /// <param name="offset">Raw offset of the NT header.</param>
        public ImageNtHeaders(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
            FileHeader = new ImageFileHeader(peFile, offset + 0x4);

            var is32Bit = FileHeader.Machine == MachineType.I386;

            OptionalHeader = new ImageOptionalHeader(peFile, offset + 0x18, !is32Bit);
        }

        /// <summary>
        ///     NT header signature.
        /// </summary>
        public uint Signature
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }
    }
}