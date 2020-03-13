using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The ImageImportDescriptors are contained in the Import Directory
    ///     and holds all the information about function and symbol imports.
    /// </summary>
    public class ImageImportDescriptor : AbstractStructure
    {
        /// <summary>
        ///     Create a new ImageImportDescriptor object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the descriptor.</param>
        public ImageImportDescriptor(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Points to the first ImageImportByName struct.
        /// </summary>
        public uint OriginalFirstThunk
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     Time and date stamp.
        /// </summary>
        public uint TimeDateStamp
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Forwarder Chain.
        /// </summary>
        public uint ForwarderChain
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        ///     RVA to the name of the DLL.
        /// </summary>
        public uint Name
        {
            get => PeFile.ReadUInt(Offset + 0xC);
            set => PeFile.WriteUInt(Offset + 0xC, value);
        }

        /// <summary>
        ///     Points to an ImageImportByName struct or
        ///     to the address of the first function.
        /// </summary>
        public uint FirstThunk
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }
    }
}