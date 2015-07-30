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
            get { return Name & 0xFFFF; }
            set { Name = value & 0xFFFF; }
        }

        public UInt32 OffsetToData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value, _offset + 0x4, _buff); }
        }

        public UInt32 OffsetToDirectory
        {
            get { return OffsetToData & 0x7FFFFFFF; }
        }

        /// <summary>
        /// True if the entry data is a directory
        /// </summary>
        public bool DataIsDirectory
        {
            get
            {
                if ((OffsetToData & 0x80000000) == 0x80000000)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// True if the entry is a resource with a name.
        /// </summary>
        public bool IsNamedEntry
        {
            get
            {
                if ((Name & 0x80000000) == 0x80000000)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// True if the entry is a resource with an ID instead of a name.
        /// </summary>
        public bool IsIdEnitry
        {
            get { return !IsNamedEntry; }
        }

        public IMAGE_RESOURCE_DIRECTORY_ENTRY(byte[] buff, UInt32 offset)
        {
            _offset = offset;
            _buff = buff;
        }
    }
}
