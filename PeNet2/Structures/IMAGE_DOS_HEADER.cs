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
    public class IMAGE_DOS_HEADER : AbstractStructure
    {
        /// <summary>
        ///     Create a new IMAGE_DOS_HEADER object.
        /// </summary>
        /// <param name="buff">Byte buffer containing a PE file.</param>
        /// <param name="offset">Offset in the buffer to the DOS header.</param>
        public IMAGE_DOS_HEADER(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        /// <summary>
        ///     Magic "MZ" header.
        /// </summary>
        public ushort e_magic
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x00); }
            set { Utility.SetUInt16(value, Offset + 0x00, Buff); }
        }

        /// <summary>
        ///     Bytes on the last page of the file.
        /// </summary>
        public ushort e_cblp
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x02); }
            set { Utility.SetUInt16(value, Offset + 0x02, Buff); }
        }

        /// <summary>
        ///     Pages in the file.
        /// </summary>
        public ushort e_cp
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x04); }
            set { Utility.SetUInt16(value, Offset + 0x04, Buff); }
        }

        /// <summary>
        ///     Relocations.
        /// </summary>
        public ushort e_crlc
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x06); }
            set { Utility.SetUInt16(value, Offset + 0x06, Buff); }
        }

        /// <summary>
        ///     Size of the header in paragraphs.
        /// </summary>
        public ushort e_cparhdr
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x08); }
            set { Utility.SetUInt16(value, Offset + 0x08, Buff); }
        }

        /// <summary>
        ///     Minimum extra paragraphs needed.
        /// </summary>
        public ushort e_minalloc
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x0A); }
            set { Utility.SetUInt16(value, Offset + 0x0A, Buff); }
        }

        /// <summary>
        ///     Maximum extra paragraphs needed.
        /// </summary>
        public ushort e_maxalloc
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x0C); }
            set { Utility.SetUInt16(value, Offset + 0x0C, Buff); }
        }

        /// <summary>
        ///     Initial (relative) SS value.
        /// </summary>
        public ushort e_ss
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x0E); }
            set { Utility.SetUInt16(value, Offset + 0x0E, Buff); }
        }

        /// <summary>
        ///     Initial SP value.
        /// </summary>
        public ushort e_sp
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x10); }
            set { Utility.SetUInt16(value, Offset + 0x10, Buff); }
        }

        /// <summary>
        ///     Checksum
        /// </summary>
        public ushort e_csum
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x12); }
            set { Utility.SetUInt16(value, Offset + 0x12, Buff); }
        }

        /// <summary>
        ///     Initial IP value.
        /// </summary>
        public ushort e_ip
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x14); }
            set { Utility.SetUInt16(value, Offset + 0x14, Buff); }
        }

        /// <summary>
        ///     Initial (relative) CS value.
        /// </summary>
        public ushort e_cs
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x16); }
            set { Utility.SetUInt16(value, Offset + 0x16, Buff); }
        }

        /// <summary>
        ///     Raw address of the relocation table.
        /// </summary>
        public ushort e_lfarlc
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x18); }
            set { Utility.SetUInt16(value, Offset + 0x18, Buff); }
        }

        /// <summary>
        ///     Overlay number.
        /// </summary>
        public ushort e_ovno
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x1A); }
            set { Utility.SetUInt16(value, Offset + 0x1A, Buff); }
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
                    Utility.BytesToUInt16(Buff, Offset + 0x1C),
                    Utility.BytesToUInt16(Buff, Offset + 0x1E),
                    Utility.BytesToUInt16(Buff, Offset + 0x20),
                    Utility.BytesToUInt16(Buff, Offset + 0x22)
                };
            }
            set
            {
                Utility.SetUInt16(value[0], Offset + 0x1C, Buff);
                Utility.SetUInt16(value[1], Offset + 0x1E, Buff);
                Utility.SetUInt16(value[2], Offset + 0x20, Buff);
                Utility.SetUInt16(value[3], Offset + 0x22, Buff);
            }
        }

        /// <summary>
        ///     OEM identifier.
        /// </summary>
        public ushort e_oemid
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x24); }
            set { Utility.SetUInt16(value, Offset + 0x24, Buff); }
        }

        /// <summary>
        ///     OEM information.
        /// </summary>
        public ushort e_oeminfo
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x26); }
            set { Utility.SetUInt16(value, Offset + 0x26, Buff); }
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
                    Utility.BytesToUInt16(Buff, Offset + 0x28),
                    Utility.BytesToUInt16(Buff, Offset + 0x2A),
                    Utility.BytesToUInt16(Buff, Offset + 0x2C),
                    Utility.BytesToUInt16(Buff, Offset + 0x2E),
                    Utility.BytesToUInt16(Buff, Offset + 0x30),
                    Utility.BytesToUInt16(Buff, Offset + 0x32),
                    Utility.BytesToUInt16(Buff, Offset + 0x34),
                    Utility.BytesToUInt16(Buff, Offset + 0x36),
                    Utility.BytesToUInt16(Buff, Offset + 0x38),
                    Utility.BytesToUInt16(Buff, Offset + 0x3A)
                };
            }
            set
            {
                Utility.SetUInt16(value[0], Offset + 0x28, Buff);
                Utility.SetUInt16(value[1], Offset + 0x2A, Buff);
                Utility.SetUInt16(value[2], Offset + 0x2C, Buff);
                Utility.SetUInt16(value[3], Offset + 0x2E, Buff);
                Utility.SetUInt16(value[4], Offset + 0x30, Buff);
                Utility.SetUInt16(value[5], Offset + 0x32, Buff);
                Utility.SetUInt16(value[6], Offset + 0x34, Buff);
                Utility.SetUInt16(value[7], Offset + 0x36, Buff);
                Utility.SetUInt16(value[8], Offset + 0x38, Buff);
                Utility.SetUInt16(value[9], Offset + 0x3A, Buff);
            }
        }

        /// <summary>
        ///     Raw address of the NT header.
        /// </summary>
        public uint e_lfanew
        {
            get { return Utility.BytesToUInt32(Buff, Offset + 0x3C); }
            set { Utility.SetUInt32(value, Offset + 0x3C, Buff); }
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