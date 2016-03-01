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

namespace PeNet.Structures
{
    /// <summary>
    ///     Represents the section header for one section.
    /// </summary>
    public class IMAGE_SECTION_HEADER
    {
        private readonly byte[] _buff;
        private readonly uint _offset;

        /// <summary>
        ///     Create a new IMAGE_SECTION_HEADER object.
        /// </summary>
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset to the section header.</param>
        public IMAGE_SECTION_HEADER(byte[] buff, uint offset)
        {
            _offset = offset;
            _buff = buff;
        }

        /// <summary>
        ///     Max. 8 byte long UTF-8 string that names
        ///     the section.
        /// </summary>
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

        /// <summary>
        ///     The raw (file) address of the section.
        /// </summary>
        public uint PhysicalAddress
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x8); }
            set { Utility.SetUInt32(value, _offset + 0x8, _buff); }
        }

        /// <summary>
        ///     Size of the section when loaded into memory. If it's bigger than
        ///     the raw data size, the rest of the section is filled with zeroes.
        /// </summary>
        public uint VirtualSize
        {
            get { return PhysicalAddress; }
            set { PhysicalAddress = value; }
        }

        /// <summary>
        ///     RVA of the section start in memory.
        /// </summary>
        public uint VirtualAddress
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0xC); }
            set { Utility.SetUInt32(value, _offset + 0xC, _buff); }
        }

        /// <summary>
        ///     Size of the section in raw on disk. Must be a multiple of the file alignment
        ///     specified in the optional header. If its less than the virtual size, the rest
        ///     is filled with zeroes.
        /// </summary>
        public uint SizeOfRawData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x10); }
            set { Utility.SetUInt32(value, _offset + 0x10, _buff); }
        }

        /// <summary>
        ///     Raw address of the section in the file.
        /// </summary>
        public uint PointerToRawData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x14); }
            set { Utility.SetUInt32(value, _offset + 0x14, _buff); }
        }

        /// <summary>
        ///     Pointer to the beginning of the relocation. If there are none, the
        ///     value is zero.
        /// </summary>
        public uint PointerToRelocations
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x18); }
            set { Utility.SetUInt32(value, _offset + 0x18, _buff); }
        }

        /// <summary>
        ///     Pointer to the beginning of the line-numbers in the file.
        ///     Zero if there are no line-numbers in the file.
        /// </summary>
        public uint PointerToLinenumbers
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x1C); }
            set { Utility.SetUInt32(value, _offset + 0x1C, _buff); }
        }

        /// <summary>
        ///     The number of relocations for the section. Is zero for executable images.
        /// </summary>
        public ushort NumberOfRelocations
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x20); }
            set { Utility.SetUInt16(value, _offset + 0x20, _buff); }
        }

        /// <summary>
        ///     The number of line-number entries for the section.
        /// </summary>
        public ushort NumberOfLinenumbers
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x22); }
            set { Utility.SetUInt16(value, _offset + 0x22, _buff); }
        }

        /// <summary>
        ///     Section characteristics. Can be resolved with
        /// </summary>
        public uint Characteristics
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x24); }
            set { Utility.SetUInt32(value, _offset + 0x24, _buff); }
        }

        /// <summary>
        ///     Create a string from all object properties.
        /// </summary>
        /// <returns>Section header properties as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_SECTION_HEADER\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}