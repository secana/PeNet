using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    /// Bound import descriptor.
    /// </summary>
    public class ImageBoundImportDescriptor : AbstractStructure
    {
        /// <summary>
        /// Create new bound import descriptor structure.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of bound import descriptor in the PE file.</param>
        public ImageBoundImportDescriptor(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// Time date stamp.
        /// </summary>
        public uint TimeDateStamp
        {
            get => PeFile.ReadUInt(Offset + 0);
            set => PeFile.WriteUInt(Offset + 0, value);
        }

        /// <summary>
        /// Offset module name.
        /// </summary>
        public ushort OffsetModuleName
        {
            get => PeFile.ReadUShort(Offset + 4);
            set => PeFile.WriteUShort(Offset + 2, value);
        }

        /// <summary>
        /// Number of module forwarder references.
        /// </summary>
        public ushort NumberOfModuleForwarderRefs
        {
            get => PeFile.ReadUShort(Offset + 6);
            set => PeFile.WriteUShort(Offset + 4, value);
        }
    }
}