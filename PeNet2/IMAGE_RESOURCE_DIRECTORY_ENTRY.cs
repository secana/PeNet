using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public class IMAGE_RESOURCE_DIRECTORY_ENTRY
    {
        private byte[] _buff;
        private UInt32 _offset;

        public UInt32 Name
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        public UInt32 ID
        {
            get { return Name; }
            set { Name = value; }
        }

        public UInt32 OffsetToData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value, _offset + 0x4, _buff); }
        }

        public IMAGE_RESOURCE_DIRECTORY_ENTRY(byte[] buff, UInt32 offset)
        {
            _offset = offset;
            _buff = buff;
        }
    }
}
