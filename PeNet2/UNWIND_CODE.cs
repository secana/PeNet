using System;
using System.Text;

namespace PeNet
{
    public class UNWIND_CODE
    {
        byte[] _buff;
        UInt32 _offset;

        public byte CodeOffset
        {
            get { return _buff[_offset]; }
            set { _buff[_offset] = value; }
        }

        public byte UnwindOp
        {
            get { return (byte)(_buff[_offset + 0x1] & 0xF); }
        }

        public byte Opinfo
        {
            get { return (byte)(_buff[_offset + 0x1] >> 0x4); }
        }

        public UInt16 FrameOffset
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x2); }
            set { Utility.SetUInt16(value, _offset + 0x2, _buff); }
        }

        public UNWIND_CODE(byte[] buff, UInt32 offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("UNWIND_CODE\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
