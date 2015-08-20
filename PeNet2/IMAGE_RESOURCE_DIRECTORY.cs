using System;

namespace PeNet
{
    public class IMAGE_RESOURCE_DIRECTORY
    {
        UInt32 _offset;
        byte[] _buff;

        public UInt32 Characteristics
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        public UInt32 TimeDateStamp
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value, _offset + 0x4, _buff); }
        }

        public UInt16 MajorVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x8); }
            set { Utility.SetUInt16(value, _offset + 0x8, _buff); }
        }

        public UInt16 MinorVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0xa); }
            set { Utility.SetUInt16(value, _offset + 0xa, _buff); }
        }

        public UInt16 NumberOfNameEntries
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0xc); }
            set { Utility.SetUInt16(value, _offset + 0xc, _buff); }
        }

        public UInt16 NumberOfIdEntries
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0xe); }
            set { Utility.SetUInt16(value, _offset + 0xe, _buff); }
        }

        public IMAGE_RESOURCE_DIRECTORY_ENTRY[] DirectoryEntries;

        public IMAGE_RESOURCE_DIRECTORY(byte[] buff, UInt32 offset)
        {
            _offset = offset;
            _buff = buff;
            DirectoryEntries = new IMAGE_RESOURCE_DIRECTORY_ENTRY[NumberOfIdEntries + NumberOfNameEntries];
        }
    }
}
