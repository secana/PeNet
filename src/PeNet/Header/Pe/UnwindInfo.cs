using System.Collections.Generic;
using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The UnwindInfo is used for x64 exception
    ///     handling and to unwind the stack. It is
    ///     pointed to by the RuntimeFunction struct.
    /// </summary>
    public class UnwindInfo : AbstractStructure
    {
        private const int SizeOfUnwindCode = 0x4;

        /// <summary>
        ///     Create a new UnwindInfo object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the UnwindInfo struct.</param>
        public UnwindInfo(IRawFile peFile, uint offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Version
        /// </summary>
        public byte Version => (byte) (PeFile.ReadByte(Offset) >> 5);

        /// <summary>
        ///     Flags
        /// </summary>
        public byte Flags => (byte) (PeFile.ReadByte(Offset) & 0x1F);

        /// <summary>
        ///     Size of prolog.
        /// </summary>
        public byte SizeOfProlog
        {
            get => PeFile.ReadByte(Offset + 0x1);
            set => PeFile.WriteByte(Offset + 0x1, value);
        }

        /// <summary>
        ///     The count of codes is not the count of UNWINDE_CODEs in
        ///     but the number of 2 byte slots. Some UNWINDW_CODEs need more
        ///     than one slot, thus the number of UNWIND_CODEs can be lower than
        ///     the number in CountOfCodes.
        /// </summary>
        public byte CountOfCodes
        {
            get => PeFile.ReadByte(Offset + 0x2);
            set => PeFile.WriteByte(Offset + 0x2, value);
        }

        /// <summary>
        ///     Frame register.
        /// </summary>
        public byte FrameRegister => (byte) (PeFile.ReadByte(Offset + 0x3) >> 4);

        /// <summary>
        ///     Frame offset.
        /// </summary>
        public byte FrameOffset => (byte) (PeFile.ReadByte(Offset + 0x3) & 0xF);

        /// <summary>
        ///     UnwindCode structure.
        /// </summary>
        public UnwindCode[] UnwindCode => ParseUnwindCodes(PeFile, Offset + 0x4);

        /// <summary>
        ///     The exception handler for the function.
        /// </summary>
        public uint ExceptionHandler
        {
            get
            {
                var off = (uint) (Offset + 0x4 + SizeOfUnwindCode*CountOfCodes);
                return PeFile.ReadUInt(off);
            }
            set
            {
                var off = (uint) (Offset + 0x4 + SizeOfUnwindCode*CountOfCodes);
                PeFile.WriteUInt(off, value);
            }
        }

        /// <summary>
        ///     Function entry.
        /// </summary>
        public uint FunctionEntry
        {
            get => ExceptionHandler;
            set => ExceptionHandler = value;
        }

        private UnwindCode[] ParseUnwindCodes(IRawFile peFile, long offset)
        {
            var ucList = new List<UnwindCode>();
            var i = 0;
            const uint nodeSize = 0x2;
            var currentUnwindCode = offset;
            while (i < CountOfCodes)
            {
                int numberOfNodes;
                var uw = new UnwindCode(peFile, currentUnwindCode);
                currentUnwindCode += nodeSize; // CodeOffset and UnwindOp/Opinfo (= 0x2 byte)

                switch (uw.UnwindOp)
                {
                    case UnwindOpType.PushNonvol:
                        break;
                    case UnwindOpType.AllocLarge:
                        currentUnwindCode += (uint) (uw.Opinfo == 0 ? 0x2 : 0x4);
                        break;
                    case UnwindOpType.AllocSmall:
                        break;
                    case UnwindOpType.SetFpreg:
                        break;
                    case UnwindOpType.SaveNonvol:
                        currentUnwindCode += 0x2;
                        break;
                    case UnwindOpType.SaveNonvolFar:
                        currentUnwindCode += 0x4;
                        break;
                    case UnwindOpType.SaveXmm128:
                        currentUnwindCode += 0x2;
                        break;
                    case UnwindOpType.SaveXmm128Far:
                        currentUnwindCode += 0x4;
                        break;
                    case UnwindOpType.PushMachframe:
                        break;
                }

                if ((uw.UnwindOp == UnwindOpType.AllocLarge
                     && uw.Opinfo == 0x0)
                    || (uw.UnwindOp == UnwindOpType.SaveNonvol)
                    || (uw.UnwindOp == UnwindOpType.SaveXmm128))
                {
                    numberOfNodes = 2;
                }
                else if ((uw.UnwindOp == UnwindOpType.AllocLarge
                          && uw.Opinfo == 0x1)
                         || (uw.UnwindOp == UnwindOpType.SaveNonvolFar)
                         || (uw.UnwindOp == UnwindOpType.SaveXmm128Far))
                {
                    numberOfNodes = 3;
                }
                else
                {
                    numberOfNodes = 1;
                }

                i += numberOfNodes;

                ucList.Add(uw);
            }
            return ucList.ToArray();
        }
    }
}