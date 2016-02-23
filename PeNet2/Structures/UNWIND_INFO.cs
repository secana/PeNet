/***********************************************************************
Copyright 2016 Stefan Hausotte

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*************************************************************************/

using System.Collections.Generic;
using System.Text;

namespace PeNet
{
    public class UNWIND_INFO
    {
        private readonly byte[] _buff;
        private readonly uint _offset;
        private readonly int sizeOfUnwindeCode = 0x4;

        public UNWIND_INFO(byte[] buff, uint offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public byte Version => (byte) (_buff[_offset] & 0x7);

        public byte Flags => (byte) (_buff[_offset] >> 3);

        public byte SizeOfProlog
        {
            get { return _buff[_offset + 0x1]; }
            set { _buff[_offset + 0x1] = value; }
        }

        /// <summary>
        ///     The count of codes is not the count of UNWINDE_CODEs in
        ///     but the number of 2 byte slots. Some UNWINDW_CODEs need more
        ///     than one slot, thus the number of UNWIND_CODEs can be lower than
        ///     the number in CountOfCodes.
        /// </summary>
        public byte CountOfCodes
        {
            get { return _buff[_offset + 0x2]; }
            set { _buff[_offset + 0x2] = value; }
        }

        public byte FrameRegister
        {
            get { return (byte) (_buff[_offset + 0x3] & 0xF); }
        }

        public UNWIND_CODE[] UnwindCode
        {
            get
            {
                var uws = ParseUnwindCodes(_buff, _offset + 0x4);
                return uws;
            }
        }

        public uint ExceptionHandler
        {
            get
            {
                var off = (uint) (_offset + 0x4 + sizeOfUnwindeCode*CountOfCodes);
                return Utility.BytesToUInt32(_buff, off);
            }
            set
            {
                var off = (uint) (_offset + 0x4 + sizeOfUnwindeCode*CountOfCodes);
                Utility.SetUInt32(value, off, _buff);
            }
        }

        public uint FunctionEntry
        {
            get { return ExceptionHandler; }
            set { ExceptionHandler = value; }
        }

        // DONT KNOW HOW BIG
        // TODO: Implement ExceptionData
        public uint[] ExceptionData
        {
            get { return null; }
        }

        private UNWIND_CODE[] ParseUnwindCodes(byte[] buff, uint offset)
        {
            var ucList = new List<UNWIND_CODE>();
            var i = 0;
            uint nodeSize = 0x2;
            var currentUnwindeCode = offset;
            while (i < CountOfCodes)
            {
                int numberOfNodes;
                var uw = new UNWIND_CODE(buff, currentUnwindeCode);
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

        public override string ToString()
        {
            var sb = new StringBuilder("UNWIND_INFO\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
            sb.Append("UnwindCodes\n");
            foreach (var uw in UnwindCode)
            {
                sb.Append(uw);
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}