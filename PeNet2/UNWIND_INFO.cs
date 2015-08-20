using System;
using System.Collections.Generic;
using System.Text;

namespace PeNet
{
    public class UNWIND_INFO
    {
        byte[] _buff;
        UInt32 _offset;
        int sizeOfUnwindeCode = 0x4;

        public byte Version
        {
            get { return (byte) (_buff[_offset] & 0x7); }
        }

        public byte Flags
        {
            get { return (byte) (_buff[_offset] >> 3); }
        }

        public byte SizeOfProlog
        {
            get { return _buff[_offset + 0x1]; }
            set { _buff[_offset + 0x1] = value; }
        }

        /// <summary>
        /// The count of codes is not the count of UNWINDE_CODEs in 
        /// but the number of 2 byte slots. Some UNWINDW_CODEs need more
        /// than one slot, thus the number of UNWIND_CODEs can be lower than
        /// the number in CountOfCodes.
        /// </summary>
        public byte CountOfCodes
        {
            get { return _buff[_offset + 0x2]; }
            set { _buff[_offset + 0x2] = value; }
        }

        public byte FrameRegister
        {
            get { return (byte)(_buff[_offset + 0x3] & 0xF); }
        }

        public UNWIND_CODE[] UnwindCode
        {
            get
            {
                var uws = ParseUnwindCodes(_buff, _offset + 0x4);
                return uws;
            }
        }

        public UInt32 ExceptionHandler
        {
            get
            {
                UInt32 off = (UInt32) (_offset + 0x4 + sizeOfUnwindeCode * CountOfCodes);
                return Utility.BytesToUInt32(_buff, off);
            }
            set
            {
                UInt32 off = (UInt32)(_offset + 0x4 + sizeOfUnwindeCode * CountOfCodes);
                Utility.SetUInt32(value, off, _buff);
            }
        }

        public UInt32 FunctionEntry
        {
            get { return ExceptionHandler; }
            set { ExceptionHandler = value; }
        }

        // DONT KNOW HOW BIG
        public UInt32[] ExceptionData
        {
            get { return null; }
        }

        public UNWIND_INFO(byte[] buff, UInt32 offset)
        {
            _buff = buff;
            _offset = offset;
        }

        UNWIND_CODE[] ParseUnwindCodes(byte[] buff, UInt32 offset)
        {
            var ucList = new List<UNWIND_CODE>();
            var i = 0;
            UInt32 nodeSize = 0x2;
            var currentUnwindeCode = (UInt32)(offset);
            while (i < CountOfCodes)
            {
                var numberOfNodes = 0;
                var uw = new UNWIND_CODE(buff, currentUnwindeCode);
                currentUnwindeCode += nodeSize; // CodeOffset and UnwindOp/Opinfo (= 0x2 byte)

                switch (uw.UnwindOp)
                {
                    case (byte)Constants.UnwindOpCodes.UWOP_PUSH_NONVOL:
                        break;                        
                    case (byte)Constants.UnwindOpCodes.UWOP_ALLOC_LARGE:
                        currentUnwindeCode += (UInt32) (uw.Opinfo == 0 ? 0x2 : 0x4);
                        break;
                    case (byte)Constants.UnwindOpCodes.UWOP_ALLOC_SMALL:
                        break;
                    case (byte)Constants.UnwindOpCodes.UWOP_SET_FPREG:
                        break;
                    case (byte)Constants.UnwindOpCodes.UWOP_SAVE_NONVOL:
                        currentUnwindeCode += 0x2;
                        break;
                    case (byte)Constants.UnwindOpCodes.UWOP_SAVE_NONVOL_FAR:
                        currentUnwindeCode += 0x4;
                        break;
                    case (byte)Constants.UnwindOpCodes.UWOP_SAVE_XMM128:
                        currentUnwindeCode += 0x2;
                        break;
                    case (byte)Constants.UnwindOpCodes.UWOP_SAVE_XMM128_FAR:
                        currentUnwindeCode += 0x4;
                        break;
                    case (byte)Constants.UnwindOpCodes.UWOP_PUSH_MACHFRAME:
                        break;
                }

                if ((uw.UnwindOp == (byte)Constants.UnwindOpCodes.UWOP_ALLOC_LARGE
                    && uw.Opinfo == 0x0)
                    || (uw.UnwindOp == (byte)Constants.UnwindOpCodes.UWOP_SAVE_NONVOL)
                    || (uw.UnwindOp == (byte)Constants.UnwindOpCodes.UWOP_SAVE_XMM128))
                {
                    numberOfNodes = 2;
                }
                else if ((uw.UnwindOp == (byte)Constants.UnwindOpCodes.UWOP_ALLOC_LARGE
                    && uw.Opinfo == 0x1)
                    || (uw.UnwindOp == (byte)Constants.UnwindOpCodes.UWOP_SAVE_NONVOL_FAR)
                    || (uw.UnwindOp == (byte)Constants.UnwindOpCodes.UWOP_SAVE_XMM128_FAR))
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

        public override string ToString()
        {
            var sb = new StringBuilder("UNWIND_INFO\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
            sb.Append("UnwindCodes\n");
            foreach(var uw in UnwindCode)
            {
                sb.Append(uw.ToString());
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
