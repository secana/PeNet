using System.Collections.Generic;
using PeNet.FileParser;

namespace PeNet.Structures
{
    /// <summary>
    ///     The UNWIND_INFO is used for x64 exception
    ///     handling and to unwind the stack. It is
    ///     pointed to by the RUNTIME_FUNCTION struct.
    /// </summary>
    public class UNWIND_INFO : AbstractStructure
    {
        private readonly int sizeOfUnwindeCode = 0x4;

        /// <summary>
        ///     Create a new UNWIND_INFO object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the UNWIND_INFO struct.</param>
        public UNWIND_INFO(IRawFile peFile, uint offset)
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
        public UNWIND_CODE[] UnwindCode => ParseUnwindCodes(PeFile, Offset + 0x4);

        /// <summary>
        ///     The exception handler for the function.
        /// </summary>
        public uint ExceptionHandler
        {
            get
            {
                var off = (uint) (Offset + 0x4 + sizeOfUnwindeCode*CountOfCodes);
                return PeFile.ReadUInt(off);
            }
            set
            {
                var off = (uint) (Offset + 0x4 + sizeOfUnwindeCode*CountOfCodes);
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

        private UNWIND_CODE[] ParseUnwindCodes(IRawFile peFile, long offset)
        {
            var ucList = new List<UNWIND_CODE>();
            var i = 0;
            uint nodeSize = 0x2;
            var currentUnwindeCode = offset;
            while (i < CountOfCodes)
            {
                int numberOfNodes;
                var uw = new UNWIND_CODE(peFile, currentUnwindeCode);
                currentUnwindeCode += nodeSize; // CodeOffset and UnwindOp/Opinfo (= 0x2 byte)

                switch (uw.UnwindOp)
                {
                    case (byte) Constants.UnwindOpCodes.UWOP_PUSH_NONVOL:
                        break;
                    case (byte) Constants.UnwindOpCodes.UWOP_ALLOC_LARGE:
                        currentUnwindeCode += (uint) (uw.Opinfo == 0 ? 0x2 : 0x4);
                        break;
                    case (byte) Constants.UnwindOpCodes.UWOP_ALLOC_SMALL:
                        break;
                    case (byte) Constants.UnwindOpCodes.UWOP_SET_FPREG:
                        break;
                    case (byte) Constants.UnwindOpCodes.UWOP_SAVE_NONVOL:
                        currentUnwindeCode += 0x2;
                        break;
                    case (byte) Constants.UnwindOpCodes.UWOP_SAVE_NONVOL_FAR:
                        currentUnwindeCode += 0x4;
                        break;
                    case (byte) Constants.UnwindOpCodes.UWOP_SAVE_XMM128:
                        currentUnwindeCode += 0x2;
                        break;
                    case (byte) Constants.UnwindOpCodes.UWOP_SAVE_XMM128_FAR:
                        currentUnwindeCode += 0x4;
                        break;
                    case (byte) Constants.UnwindOpCodes.UWOP_PUSH_MACHFRAME:
                        break;
                }

                if ((uw.UnwindOp == (byte) Constants.UnwindOpCodes.UWOP_ALLOC_LARGE
                     && uw.Opinfo == 0x0)
                    || (uw.UnwindOp == (byte) Constants.UnwindOpCodes.UWOP_SAVE_NONVOL)
                    || (uw.UnwindOp == (byte) Constants.UnwindOpCodes.UWOP_SAVE_XMM128))
                {
                    numberOfNodes = 2;
                }
                else if ((uw.UnwindOp == (byte) Constants.UnwindOpCodes.UWOP_ALLOC_LARGE
                          && uw.Opinfo == 0x1)
                         || (uw.UnwindOp == (byte) Constants.UnwindOpCodes.UWOP_SAVE_NONVOL_FAR)
                         || (uw.UnwindOp == (byte) Constants.UnwindOpCodes.UWOP_SAVE_XMM128_FAR))
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