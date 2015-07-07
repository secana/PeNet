using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet2
{
    public class IMAGE_IMPORT_DESCRIPTOR
    {
        private byte[] _buff;
        private UInt32 _offset;

        public UInt32 OriginalFirstThunk
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        public UInt32 TimeDateStamp
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value, _offset + 0x4, _buff); }
        }

        public UInt32 ForwarderChain
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x8); }
            set { Utility.SetUInt32(value, _offset + 0x8, _buff); }
        }

        public UInt32 Name
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0xC); }
            set { Utility.SetUInt32(value, _offset + 0xC, _buff); }
        }

        public UInt32 FirstThunk
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x10); }
            set { Utility.SetUInt32(value, _offset + 0x10, _buff); }
        }

        public IMAGE_IMPORT_DESCRIPTOR(byte [] buff, UInt32 offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_IMPORT_DESCRIPTOR\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
