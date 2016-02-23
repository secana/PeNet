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

using System.Text;

namespace PeNet
{
    public class IMAGE_SECTION_HEADER
    {
        private readonly byte[] _buff;
        private readonly uint _offset;

        public IMAGE_SECTION_HEADER(byte[] buff, uint offset)
        {
            _offset = offset;
            _buff = buff;
        }

        public byte[] Name
        {
            get
            {
                return new[]
                {
                    _buff[_offset + 0],
                    _buff[_offset + 1],
                    _buff[_offset + 2],
                    _buff[_offset + 3],
                    _buff[_offset + 4],
                    _buff[_offset + 5],
                    _buff[_offset + 6],
                    _buff[_offset + 7]
                };
            }

            set
            {
                _buff[_offset + 0] = value[0];
                _buff[_offset + 1] = value[1];
                _buff[_offset + 2] = value[2];
                _buff[_offset + 3] = value[3];
                _buff[_offset + 4] = value[4];
                _buff[_offset + 5] = value[5];
                _buff[_offset + 6] = value[7];
                _buff[_offset + 7] = value[8];
            }
        }

        public uint PhysicalAddress
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x8); }
            set { Utility.SetUInt32(value, _offset + 0x8, _buff); }
        }

        public uint VirtualSize
        {
            get { return PhysicalAddress; }
            set { PhysicalAddress = value; }
        }

        public uint VirtualAddress
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0xC); }
            set { Utility.SetUInt32(value, _offset + 0xC, _buff); }
        }

        public uint SizeOfRawData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x10); }
            set { Utility.SetUInt32(value, _offset + 0x10, _buff); }
        }

        public uint PointerToRawData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x14); }
            set { Utility.SetUInt32(value, _offset + 0x14, _buff); }
        }

        public uint PointerToRelocations
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x18); }
            set { Utility.SetUInt32(value, _offset + 0x18, _buff); }
        }

        public uint PointerToLinenumbers
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x1C); }
            set { Utility.SetUInt32(value, _offset + 0x1C, _buff); }
        }

        private ushort NumberOfRelocations
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x20); }
            set { Utility.SetUInt16(value, _offset + 0x20, _buff); }
        }

        private ushort NumberOfLinenumbers
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x22); }
            set { Utility.SetUInt16(value, _offset + 0x22, _buff); }
        }

        private uint Characteristics
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x24); }
            set { Utility.SetUInt32(value, _offset + 0x24, _buff); }
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_SECTION_HEADER\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}