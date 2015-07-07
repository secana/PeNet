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

        public UInt32 AddressOfData
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        public UInt32 Ordinal
        {
            get { return AddressOfData; }
            set { AddressOfData = value; }
        }

        public UInt32 ForwarderString
        {
            get { return AddressOfData; }
            set { AddressOfData = value; }
        }

        public UInt32 Function
        {
            get { return AddressOfData; }
            set { AddressOfData = value; }
        }

        public IMAGE_THUNK_DATA(byte[] buff, UInt32 offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_THUNK_DATA\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-15}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
