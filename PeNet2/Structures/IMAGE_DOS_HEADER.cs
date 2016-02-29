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
    ///     The IMAGE_DOS_HEADER with which every PE file starts.
    /// </summary>
    public class IMAGE_DOS_HEADER
    {
        private readonly byte[] _buff;

        /// <summary>
        ///     Create a new IMAGE_DOS_HEADER object.
        /// </summary>
        /// <param name="buff">Byte buffer containing a PE file.</param>
        public IMAGE_DOS_HEADER(byte[] buff)
        {
            _buff = buff;
        }

        /// <summary>
        ///     Magic "MZ" header.
        /// </summary>
        public ushort e_magic
        {
            get { return Utility.BytesToUInt16(_buff, 0x00); }
            set { Utility.SetUInt16(value, 0x00, _buff); }
        }

        /// <summary>
        ///     Bytes on the last page of the file.
        /// </summary>
        public ushort e_cblp
        {
            get { return Utility.BytesToUInt16(_buff, 0x02); }
            set { Utility.SetUInt16(value, 0x02, _buff); }
        }

        /// <summary>
        ///     Pages in the file.
        /// </summary>
        public ushort e_cp
        {
            get { return Utility.BytesToUInt16(_buff, 0x04); }
            set { Utility.SetUInt16(value, 0x04, _buff); }
        }

        /// <summary>
        ///     Relocations.
        /// </summary>
        public ushort e_crlc
        {
            get { return Utility.BytesToUInt16(_buff, 0x06); }
            set { Utility.SetUInt16(value, 0x06, _buff); }
        }

        /// <summary>
        ///     Size of the header in paragraphs.
        /// </summary>
        public ushort e_cparhdr
        {
            get { return Utility.BytesToUInt16(_buff, 0x08); }
            set { Utility.SetUInt16(value, 0x08, _buff); }
        }

        /// <summary>
        ///     Minimum extra paragraphs needed.
        /// </summary>
        public ushort e_minalloc
        {
            get { return Utility.BytesToUInt16(_buff, 0x0A); }
            set { Utility.SetUInt16(value, 0x0A, _buff); }
        }

        /// <summary>
        ///     Maximum extra paragraphs needed.
        /// </summary>
        public ushort e_maxalloc
        {
            get { return Utility.BytesToUInt16(_buff, 0x0C); }
            set { Utility.SetUInt16(value, 0x0C, _buff); }
        }

        /// <summary>
        ///     Initial (relative) SS value.
        /// </summary>
        public ushort e_ss
        {
            get { return Utility.BytesToUInt16(_buff, 0x0E); }
            set { Utility.SetUInt16(value, 0x0E, _buff); }
        }

        /// <summary>
        ///     Initial SP value.
        /// </summary>
        public ushort e_sp
        {
            get { return Utility.BytesToUInt16(_buff, 0x10); }
            set { Utility.SetUInt16(value, 0x10, _buff); }
        }

        /// <summary>
        ///     Checksum
        /// </summary>
        public ushort e_csum
        {
            get { return Utility.BytesToUInt16(_buff, 0x12); }
            set { Utility.SetUInt16(value, 0x12, _buff); }
        }

        /// <summary>
        ///     Initial IP value.
        /// </summary>
        public ushort e_ip
        {
            get { return Utility.BytesToUInt16(_buff, 0x14); }
            set { Utility.SetUInt16(value, 0x14, _buff); }
        }

        /// <summary>
        ///     Initial (relative) CS value.
        /// </summary>
        public ushort e_cs
        {
            get { return Utility.BytesToUInt16(_buff, 0x16); }
            set { Utility.SetUInt16(value, 0x16, _buff); }
        }

        /// <summary>
        ///     Raw address of the relocation table.
        /// </summary>
        public ushort e_lfarlc
        {
            get { return Utility.BytesToUInt16(_buff, 0x18); }
            set { Utility.SetUInt16(value, 0x18, _buff); }
        }

        /// <summary>
        ///     Overlay number.
        /// </summary>
        public ushort e_ovno
        {
            get { return Utility.BytesToUInt16(_buff, 0x1A); }
            set { Utility.SetUInt16(value, 0x1A, _buff); }
        }

        /// <summary>
        ///     Reserved.
        /// </summary>
        public ushort[] e_res // 4 * UInt16
        {
            get
            {
                return new[]
                {
                    Utility.BytesToUInt16(_buff, 0x1C),
                    Utility.BytesToUInt16(_buff, 0x1E),
                    Utility.BytesToUInt16(_buff, 0x20),
                    Utility.BytesToUInt16(_buff, 0x22)
                };
            }
            set
            {
                Utility.SetUInt16(value[0], 0x1C, _buff);
                Utility.SetUInt16(value[1], 0x1E, _buff);
                Utility.SetUInt16(value[2], 0x20, _buff);
                Utility.SetUInt16(value[3], 0x22, _buff);
            }
        }

        /// <summary>
        ///     OEM identifier.
        /// </summary>
        public ushort e_oemid
        {
            get { return Utility.BytesToUInt16(_buff, 0x24); }
            set { Utility.SetUInt16(value, 0x24, _buff); }
        }

        /// <summary>
        ///     OEM information.
        /// </summary>
        public ushort e_oeminfo
        {
            get { return Utility.BytesToUInt16(_buff, 0x26); }
            set { Utility.SetUInt16(value, 0x26, _buff); }
        }

        /// <summary>
        ///     Reserved.
        /// </summary>
        public ushort[] e_res2 // 10 * UInt16
        {
            get
            {
                return new[]
                {
                    Utility.BytesToUInt16(_buff, 0x28),
                    Utility.BytesToUInt16(_buff, 0x2A),
                    Utility.BytesToUInt16(_buff, 0x2C),
                    Utility.BytesToUInt16(_buff, 0x2E),
                    Utility.BytesToUInt16(_buff, 0x30),
                    Utility.BytesToUInt16(_buff, 0x32),
                    Utility.BytesToUInt16(_buff, 0x34),
                    Utility.BytesToUInt16(_buff, 0x36),
                    Utility.BytesToUInt16(_buff, 0x38),
                    Utility.BytesToUInt16(_buff, 0x3A)
                };
            }
            set
            {
                Utility.SetUInt16(value[0], 0x28, _buff);
                Utility.SetUInt16(value[1], 0x2A, _buff);
                Utility.SetUInt16(value[2], 0x2C, _buff);
                Utility.SetUInt16(value[3], 0x2E, _buff);
                Utility.SetUInt16(value[4], 0x30, _buff);
                Utility.SetUInt16(value[5], 0x32, _buff);
                Utility.SetUInt16(value[6], 0x34, _buff);
                Utility.SetUInt16(value[7], 0x36, _buff);
                Utility.SetUInt16(value[8], 0x38, _buff);
                Utility.SetUInt16(value[9], 0x3A, _buff);
            }
        }

        /// <summary>
        ///     Raw address of the NT header.
        /// </summary>
        public uint e_lfanew
        {
            get { return Utility.BytesToUInt32(_buff, 0x3C); }
            set { Utility.SetUInt32(value, 0x3C, _buff); }
        }

        /// <summary>
        ///     Creates a string representation of all properties.
        /// </summary>
        /// <returns>The header properties as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_DOS_HEADER\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}