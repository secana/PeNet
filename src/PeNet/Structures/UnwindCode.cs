using PeNet.FileParser;

namespace PeNet.Structures
{
    /// <summary>
    ///     The UnwindCode is a struct in
    ///     the UnwindInfo used to describe
    ///     exception handling in x64 applications
    ///     and to walk the stack.
    /// </summary>
    public class UnwindCode : AbstractStructure
    {
        /// <summary>
        ///     Create a new UnwindInfo object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the UnwindInfo.</param>
        public UnwindCode(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Code offset.
        /// </summary>
        public byte CodeOffset
        {
            get => PeFile.ReadByte(Offset);
            set => PeFile.WriteByte(Offset, value);
        }

        /// <summary>
        ///     Unwind operation.
        /// </summary>
        public byte UnwindOp => (byte) (PeFile.ReadByte(Offset + 0x1) >> 4);

        /// <summary>
        ///     Operation information.
        /// </summary>
        public byte Opinfo => (byte) (PeFile.ReadByte(Offset + 0x1) & 0xF);

        /// <summary>
        ///     Frame offset.
        /// </summary>
        public ushort FrameOffset
        {
            get => PeFile.ReadUShort(Offset + 0x2);
            set => PeFile.WriteUShort(Offset + 0x2, value);
        }
    }
}