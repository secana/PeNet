using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet2
{
    public class IMAGE_THUNK_DATA
    {
        private byte[] _buff;
        private UInt32 _offset;
        private bool _is64Bit;

        public UInt64 AddressOfData
        {
            get 
            { 
                return _is64Bit ? Utility.BytesToUInt64(_buff, _offset) : Utility.BytesToUInt32(_buff, _offset); 
            }
            set 
            {
                if (!_is64Bit)
                    Utility.SetUInt32((UInt32)value, _offset, _buff);
                else
                    Utility.SetUInt64(value, _offset, _buff);
            }
        }

        public UInt64 Ordinal
        {
            get { return AddressOfData; }
            set { AddressOfData = value; }
        }

        public UInt64 ForwarderString
        {
            get { return AddressOfData; }
            set { AddressOfData = value; }
        }

        public UInt64 Function
        {
            get { return AddressOfData; }
            set { AddressOfData = value; }
        }

        public IMAGE_THUNK_DATA(byte[] buff, UInt32 offset, bool is64Bit)
        {
            _buff = buff;
            _offset = offset;
            _is64Bit = is64Bit;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_THUNK_DATA\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-15}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
