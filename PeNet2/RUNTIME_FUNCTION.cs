using System;
using System.Text;

namespace PeNet
{
    public class RUNTIME_FUNCTION
    {
        byte[] _buff;
        UInt32 _offset;

        public UInt32 FunctionStart
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        public UInt32 FunctionEnd
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value,_offset + 0x4, _buff); }
        }

        public UInt32 UnwindInfo
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x8); }
            set { Utility.SetUInt32(value, _offset + 0x8, _buff); }
        }

        public RUNTIME_FUNCTION(byte[] buff, UInt32 offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("RUNTIME_FUNCTION\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
