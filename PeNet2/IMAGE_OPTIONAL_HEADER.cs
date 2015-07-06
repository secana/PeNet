using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet2
{
    public class IMAGE_OPTIONAL_HEADER
    {
        byte[] _buff;
        UInt32 _offset;

        public UInt16 Magic
        {
            get
            {
                return Utility.BytesToUInt16(_buff, _offset);
            }
            set
            {
                Utility.SetUInt16(value, _offset, _buff);
            }
        }

        public byte MajorLinkerVersion
        {
            get { return _buff[_offset + 0x2]; }
            set { _buff[_offset + 0x2] = value; }
        }

        public byte MinorLinkerVersion
        {
            get { return _buff[_offset + 0x3]; }
            set { _buff[_offset + 03] = value; }
        }

        public UInt32 SizeOfCode
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value, _offset + 0x4, _buff); }
        }

        public UInt32 SizeOfInitializedData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x8); }
            set { Utility.SetUInt32(value, _offset + 0x8, _buff); }
        }

        public UInt32 SizeOfUninitializedData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0xC); }
            set { Utility.SetUInt32(value, _offset + 0xC, _buff); }
        }

        public UInt32 AddressOfEntryPoint
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x10); }
            set { Utility.SetUInt32(value, _offset + 0x10, _buff); }
        }

        public UInt32 BaseOfCode
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x14); }
            set { Utility.SetUInt32(value, _offset + 0x14, _buff); }
        }

        public UInt32 BaseOfData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x18); }
            set { Utility.SetUInt32(value, _offset + 0x18, _buff); }
        }

        public UInt32 ImageBase
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x1C); }
            set { Utility.SetUInt32(value, _offset + 0x1C, _buff); }
        }

        public UInt32 SectionAlignment
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x20); }
            set { Utility.SetUInt32(value, _offset + 0x20, _buff); }
        }

        public UInt32 FileAlignment
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x24); }
            set { Utility.SetUInt32(value, _offset + 0x24, _buff); }
        }

        public UInt16 MajorOperatingSystemVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x28); }
            set { Utility.SetUInt16(value, _offset + 0x28, _buff); }
        }

        public UInt16 MinorOperatingSystemVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x2A); }
            set { Utility.SetUInt16(value, _offset + 0x2A, _buff); }
        }

        public UInt16 MajorImageVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x2C); }
            set { Utility.SetUInt16(value, _offset + 0x2C, _buff); }
        }

        public UInt16 MinorImageVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x2E); }
            set { Utility.SetUInt16(value, _offset + 0x2E, _buff); }
        }

        public UInt16 MajorSubsystemVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x30); }
            set { Utility.SetUInt16(value, _offset + 0x30, _buff); }
        }

        public UInt16 MinorSubsystemVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x32); }
            set { Utility.SetUInt16(value, _offset + 0x32, _buff); }
        }

        public UInt32 Win32VersionValue
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x34); }
            set { Utility.SetUInt32(value, _offset + 0x34, _buff); }
        }

        public UInt32 SizeOfImage
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x38); }
            set { Utility.SetUInt32(value, _offset + 0x38, _buff); }
        }

        public UInt32 SizeOfHeaders
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x3C); }
            set { Utility.SetUInt32(value, _offset + 0x3C, _buff); }
        }

        public UInt32 CheckSum
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x40); }
            set { Utility.SetUInt32(value, _offset + 0x40, _buff); }
        }

        public UInt16 Subsystem
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x44); }
            set { Utility.SetUInt16(value, _offset + 0x44, _buff); }
        }

        public UInt16 DllCharacteristics
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x46); }
            set { Utility.SetUInt16(value, _offset + 0x46, _buff); }
        }
        ///
        public UInt32 SizeOfStackReserve
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x48); }
            set { Utility.SetUInt32(value, _offset + 0x48, _buff); }
        }

        public UInt32 SizeOfStackCommit
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4C); }
            set { Utility.SetUInt32(value, _offset + 0x4C, _buff); }
        }

        public UInt32 SizeOfHeapReserve
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x50); }
            set { Utility.SetUInt32(value, _offset + 0x50, _buff); }
        }

        public UInt32 SizeOfHeapCommit
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x54); }
            set { Utility.SetUInt32(value, _offset + 0x54, _buff); }
        }

        public UInt32 LoaderFlags
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x58); }
            set { Utility.SetUInt32(value, _offset + 0x58, _buff); }
        }

        public UInt32 NumberOfRvaAndSizes
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x5C); }
            set { Utility.SetUInt32(value, _offset + 0x5C, _buff); }
        }

        public readonly IMAGE_DATA_DIRECTORY[] DataDirectory;

        public IMAGE_OPTIONAL_HEADER(byte[] buff, UInt32 offset)
        {
            _buff = buff;
            _offset = offset;
            DataDirectory = new IMAGE_DATA_DIRECTORY[16];

            for(UInt32 i = 0; i < 16; i++)
            {
                DataDirectory[i] = new IMAGE_DATA_DIRECTORY(buff, offset + 0x60 + i * 0x8);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_OPTIONAL_HEADER\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-15}:\t{1,10:X}\n"));
            foreach(var dd in DataDirectory)
                sb.Append(dd.ToString());
            return sb.ToString();
        }
    }
}
