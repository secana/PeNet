using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The IMAGE_IMPORT_DESCRIPTORs are contained in the Import Directory
    ///     and holds all the information about function and symbol imports.
    /// </summary>
    public class IMAGE_IMPORT_DESCRIPTOR : AbstractStructure
    {
        /// <summary>
        ///     Create a new IMAGE_IMPORT_DESCRIPTOR object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the descriptor.</param>
        public IMAGE_IMPORT_DESCRIPTOR(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Points to the first IMAGE_IMPORT_BY_NAME struct.
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
        ///     Points to an IMAGE_IMPORT_BY_NAME struct or
        ///     to the address of the first function.
        /// </summary>
        public uint FirstThunk
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }
    }
}