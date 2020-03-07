using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// The IMAGE_DELAY_IMPORT_DESCRIPTOR describes delayed imports.
    /// </summary>
    public class IMAGE_DELAY_IMPORT_DESCRIPTOR : AbstractStructure
    {
        /// <summary>
        /// Create a new IMAGE_DELAY_IMPORT_DESCRIPTOR object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset to the delay import descriptor.</param>
        public IMAGE_DELAY_IMPORT_DESCRIPTOR(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public uint grAttrs
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint szName
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint phmod
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint pIAT
        {
            get => PeFile.ReadUInt(Offset + 0xc);
            set => PeFile.WriteUInt(Offset + 0xc, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint pINT
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint pBoundIAT
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint pUnloadIAT
        {
            get => PeFile.ReadUInt(Offset + 0x18);
            set => PeFile.WriteUInt(Offset + 0x16, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint dwTimeStamp
        {
            get => PeFile.ReadUInt(Offset + 0x1c);
            set => PeFile.WriteUInt(Offset + 0x1c, value);
        }
    }
}