using PeNet.FileParser;

namespace PeNet.Header.Pe
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
        public UnwindOpType UnwindOp 
            => (UnwindOpType) (PeFile.ReadByte(Offset + 0x1) >> 4);

        /// <summary>
        ///     Operation information.
        /// </summary>
        public byte Opinfo 
            => (byte) (PeFile.ReadByte(Offset + 0x1) & 0xF);

        /// <summary>
        ///     Frame offset.
        /// </summary>
        public ushort FrameOffset
        {
            get => PeFile.ReadUShort(Offset + 0x2);
            set => PeFile.WriteUShort(Offset + 0x2, value);
        }
    }

    /// <summary>
    ///     UnwindOp Codes for the unwind information
    ///     used to walk the stack in x64 applications.
    /// </summary>
    public enum UnwindOpType : byte
    {
        /// <summary>
        ///     Push a non volatile integer.
        /// </summary>
        PushNonvol = 0,

        /// <summary>
        ///     Allocate large size on stack.
        /// </summary>
        AllocLarge = 1,

        /// <summary>
        ///     Allocate small size on stack.
        /// </summary>
        AllocSmall = 2,

        /// <summary>
        ///     Establish frame pointer register.
        /// </summary>
        SetFpreg = 3,

        /// <summary>
        ///     Save non volatile register to stack by a MOV.
        /// </summary>
        SaveNonvol = 4,

        /// <summary>
        ///     Save non volatile register to stack with
        ///     a long offset by a MOV.
        /// </summary>
        SaveNonvolFar = 5,

        /// <summary>
        ///     Save a XMM (128 bit) register to the stack.
        /// </summary>
        SaveXmm128 = 8,

        /// <summary>
        ///     Save a XMM (128 bit) register to the stack
        ///     with a long offset.
        /// </summary>
        SaveXmm128Far = 9,

        /// <summary>
        ///     Push a machine frame, which is used to record the effect
        ///     of a hardware interrupt.
        /// </summary>
        PushMachframe = 10
    }
}