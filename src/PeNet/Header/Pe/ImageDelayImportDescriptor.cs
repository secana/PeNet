using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    /// The ImageDelayImportDirectory describes delayed imports.
    /// </summary>
    public class ImageDelayImportDescriptor : AbstractStructure
    {
        /// <summary>
        /// Create a new ImageDelayImportDirectory object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset to the delay import descriptor.</param>
        public ImageDelayImportDescriptor(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public uint GrAttrs
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint SzName
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint Phmod
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint PIat
        {
            get => PeFile.ReadUInt(Offset + 0xc);
            set => PeFile.WriteUInt(Offset + 0xc, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint PInt
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint PBoundIAT
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint PUnloadIAT
        {
            get => PeFile.ReadUInt(Offset + 0x18);
            set => PeFile.WriteUInt(Offset + 0x16, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public uint DwTimeStamp
        {
            get => PeFile.ReadUInt(Offset + 0x1c);
            set => PeFile.WriteUInt(Offset + 0x1c, value);
        }
    }
}