using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Bound import descriptor.
    /// </summary>
    public class IMAGE_BOUND_IMPORT_DESCRIPTOR : AbstractStructure
    {
        /// <summary>
        /// Create new bound import descriptor structure.
        /// </summary>
        /// <param name="buff">PE file as byte buffer.</param>
        /// <param name="offset">Offset of bound import descriptor in the buffer.</param>
        public IMAGE_BOUND_IMPORT_DESCRIPTOR(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        /// <summary>
        /// Time date stamp.
        /// </summary>
        public uint TimeDateStamp
        {
            get { return Buff.BytesToUInt32(Offset + 0); }
            set { Buff.SetUInt32(Offset + 0, value); }
        }

        /// <summary>
        /// Offset module name.
        /// </summary>
        public ushort OffsetModuleName
        {
            get { return Buff.BytesToUInt16(Offset + 4); }
            set { Buff.SetUInt16(Offset + 2, value); }
        }

        /// <summary>
        /// Number of moduke forwarder references.
        /// </summary>
        public ushort NumberOfModuleForwarderRefs
        {
            get { return Buff.BytesToUInt16(Offset + 6); }
            set { Buff.SetUInt16(Offset + 4, value); }
        }
    }
}