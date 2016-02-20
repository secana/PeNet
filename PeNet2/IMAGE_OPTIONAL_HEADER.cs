/***********************************************************************
Copyright 2016 Stefan Hausotte

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*************************************************************************/

using System;
using System.Text;

namespace PeNet
{
    public class IMAGE_OPTIONAL_HEADER
    {
        byte[] _buff;
        UInt32 _offset;
        bool _is64Bit;

        public UInt16 Magic
        {
            get { return Utility.BytesToUInt16(_buff, _offset); }
            set { Utility.SetUInt16(value, _offset, _buff); }
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
            get 
            {
                return _is64Bit ? 0 : Utility.BytesToUInt32(_buff, _offset + 0x18); 
            }
            set 
            {
                if (!_is64Bit)
                    Utility.SetUInt32(value, _offset + 0x18, _buff);
                else
                    throw new Exception("IMAGE_OPTIONAL_HEADER->BaseOfCode does not exist in 64 bit applications.");
            }
        }

        public UInt64 ImageBase
        {
            get 
            { 
                return _is64Bit ? Utility.BytesToUInt64(_buff, _offset + 0x18) : Utility.BytesToUInt32(_buff, _offset + 0x1C); 
            }
            set 
            {
                if (!_is64Bit)
                    Utility.SetUInt32((UInt32)value, _offset + 0x1C, _buff);
                else
                    Utility.SetUInt64(value, _offset + 0x18, _buff);
            }
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

        public UInt64 SizeOfStackReserve
        {
            get 
            {
                return _is64Bit ? Utility.BytesToUInt64(_buff, _offset + 0x48) : Utility.BytesToUInt32(_buff, _offset + 0x48);
            }
            set 
            {
                if (!_is64Bit)
                    Utility.SetUInt32((UInt32)value, _offset + 0x48, _buff);
                else
                    Utility.SetUInt64(value, _offset + 0x48, _buff);
            }
        }

        public UInt64 SizeOfStackCommit
        {
            get 
            { 
                return _is64Bit ? Utility.BytesToUInt64(_buff, _offset + 0x50) : Utility.BytesToUInt32(_buff, _offset + 0x4C); 
            }
            set 
            {
                if (!_is64Bit)
                    Utility.SetUInt32((UInt32)value, _offset + 0x4C, _buff);
                else
                    Utility.SetUInt64(value, _offset + 0x50, _buff);
            }
        }

        public UInt64 SizeOfHeapReserve
        {
            get 
            { 
                return _is64Bit ? Utility.BytesToUInt64(_buff, _offset + 0x58) : Utility.BytesToUInt32(_buff, _offset + 0x50); 
            }
            set 
            {
                if (!_is64Bit)
                    Utility.SetUInt32((UInt32)value, _offset + 0x50, _buff);
                else
                    Utility.SetUInt64(value, _offset + 0x58, _buff);
            }
        }

        public UInt64 SizeOfHeapCommit
        {
            get 
            { 
                return _is64Bit ? Utility.BytesToUInt64(_buff, _offset + 0x60) : Utility.BytesToUInt32(_buff, _offset + 0x54); 
            }
            set 
            {
                if (!_is64Bit)
                    Utility.SetUInt32((UInt32)value, _offset + 0x54, _buff);
                else
                    Utility.SetUInt64(value, _offset + 0x60, _buff);
            }
        }

        public UInt32 LoaderFlags
        {
            get 
            { 
                return _is64Bit ? Utility.BytesToUInt32(_buff, _offset + 0x68) : Utility.BytesToUInt32(_buff, _offset + 0x58); 
            }
            set 
            {
                if (!_is64Bit)
                    Utility.SetUInt32(value, _offset + 0x58, _buff);
                else
                    Utility.SetUInt32(value, _offset + 0x68, _buff);
            }
        }

        public UInt32 NumberOfRvaAndSizes
        {
            get 
            { 
                return _is64Bit ? Utility.BytesToUInt32(_buff, _offset + 0x6C) : Utility.BytesToUInt32(_buff, _offset + 0x5C); 
            }
            set 
            {
                if (!_is64Bit)
                    Utility.SetUInt32(value, _offset + 0x5C, _buff);
                else
                    Utility.SetUInt32(value, _offset + 0x6C, _buff);
            }
        }

        public readonly IMAGE_DATA_DIRECTORY[] DataDirectory;

        public IMAGE_OPTIONAL_HEADER(byte[] buff, UInt32 offset, bool is64Bit)
        {
            _buff = buff;
            _offset = offset;
            _is64Bit = is64Bit;

            DataDirectory = new IMAGE_DATA_DIRECTORY[16];

            for(UInt32 i = 0; i < 16; i++)
            {
                if(!_is64Bit)
                    DataDirectory[i] = new IMAGE_DATA_DIRECTORY(buff, offset + 0x60 + i * 0x8);
                else
                    DataDirectory[i] = new IMAGE_DATA_DIRECTORY(buff, offset + 0x70 + i * 0x8);
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
