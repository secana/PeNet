using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public class UNWIND_INFO
    {
        private byte[] _buff;
        private UInt32 _offset;
        private int sizeOfUnwindeCode = 0x4;

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

        public byte CountOfCodes
        {
            get { return _buff[_offset + 0x2]; }
            set { _buff[_offset + 0x2] = value; }
        }

        public byte FrameRegister
        {
            get { return (byte)(_buff[_offset + 0x3] & 0xF); }
        }

        public byte Opinfo
        {
            get { return (byte) (_buff[_offset + 0x3] >> 4);}
        }

        public UNWIND_CODE[] UnwindCode
        {
            get
            {
                var uw = new UNWIND_CODE[CountOfCodes];
                for(int i = 0; i < CountOfCodes; i++)
                {
                    uw[i] = new UNWIND_CODE(_buff, (UInt32) (_offset + 0x4 + i * sizeOfUnwindeCode));
                }
                return uw;
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

        public override string ToString()
        {
            var sb = new StringBuilder("UNWIND_INFO\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));
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
