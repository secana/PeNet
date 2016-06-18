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
            get { return Buff.BytesToUInt16(Offset + 0x00); }
            set { value.SetUInt16(Offset + 0x00, Buff); }
        }

        /// <summary>
        ///     Bytes on the last page of the file.
        /// </summary>
        public ushort e_cblp
        {
            get { return Buff.BytesToUInt16(Offset + 0x02); }
            set { value.SetUInt16(Offset + 0x02, Buff); }
        }

        /// <summary>
        ///     Pages in the file.
        /// </summary>
        public ushort e_cp
        {
            get { return Buff.BytesToUInt16(Offset + 0x04); }
            set { value.SetUInt16(Offset + 0x04, Buff); }
        }

        /// <summary>
        ///     Relocations.
        /// </summary>
        public ushort e_crlc
        {
            get { return Buff.BytesToUInt16(Offset + 0x06); }
            set { value.SetUInt16(Offset + 0x06, Buff); }
        }

        /// <summary>
        ///     Size of the header in paragraphs.
        /// </summary>
        public ushort e_cparhdr
        {
            get { return Buff.BytesToUInt16(Offset + 0x08); }
            set { value.SetUInt16(Offset + 0x08, Buff); }
        }

        /// <summary>
        ///     Minimum extra paragraphs needed.
        /// </summary>
        public ushort e_minalloc
        {
            get { return Buff.BytesToUInt16(Offset + 0x0A); }
            set { value.SetUInt16(Offset + 0x0A, Buff); }
        }

        /// <summary>
        ///     Maximum extra paragraphs needed.
        /// </summary>
        public ushort e_maxalloc
        {
            get { return Buff.BytesToUInt16(Offset + 0x0C); }
            set { value.SetUInt16(Offset + 0x0C, Buff); }
        }

        /// <summary>
        ///     Initial (relative) SS value.
        /// </summary>
        public ushort e_ss
        {
            get { return Buff.BytesToUInt16(Offset + 0x0E); }
            set { value.SetUInt16(Offset + 0x0E, Buff); }
        }

        /// <summary>
        ///     Initial SP value.
        /// </summary>
        public ushort e_sp
        {
            get { return Buff.BytesToUInt16(Offset + 0x10); }
            set { value.SetUInt16(Offset + 0x10, Buff); }
        }

        /// <summary>
        ///     Checksum
        /// </summary>
        public ushort e_csum
        {
            get { return Buff.BytesToUInt16(Offset + 0x12); }
            set { value.SetUInt16(Offset + 0x12, Buff); }
        }

        /// <summary>
        ///     Initial IP value.
        /// </summary>
        public ushort e_ip
        {
            get { return Buff.BytesToUInt16(Offset + 0x14); }
            set { value.SetUInt16(Offset + 0x14, Buff); }
        }

        /// <summary>
        ///     Initial (relative) CS value.
        /// </summary>
        public ushort e_cs
        {
            get { return Buff.BytesToUInt16(Offset + 0x16); }
            set { value.SetUInt16(Offset + 0x16, Buff); }
        }

        /// <summary>
        ///     Raw address of the relocation table.
        /// </summary>
        public ushort e_lfarlc
        {
            get { return Buff.BytesToUInt16(Offset + 0x18); }
            set { value.SetUInt16(Offset + 0x18, Buff); }
        }

        /// <summary>
        ///     Overlay number.
        /// </summary>
        public ushort e_ovno
        {
            get { return Buff.BytesToUInt16(Offset + 0x1A); }
            set { value.SetUInt16(Offset + 0x1A, Buff); }
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
                    Buff.BytesToUInt16(Offset + 0x1C),
                    Buff.BytesToUInt16(Offset + 0x1E),
                    Buff.BytesToUInt16(Offset + 0x20),
                    Buff.BytesToUInt16(Offset + 0x22)
                };
            }
            set
            {
                value[0].SetUInt16(Offset + 0x1C, Buff);
                value[1].SetUInt16(Offset + 0x1E, Buff);
                value[2].SetUInt16(Offset + 0x20, Buff);
                value[3].SetUInt16(Offset + 0x22, Buff);
            }
        }

        /// <summary>
        ///     OEM identifier.
        /// </summary>
        public ushort e_oemid
        {
            get { return Buff.BytesToUInt16(Offset + 0x24); }
            set { value.SetUInt16(Offset + 0x24, Buff); }
        }

        /// <summary>
        ///     OEM information.
        /// </summary>
        public ushort e_oeminfo
        {
            get { return Buff.BytesToUInt16(Offset + 0x26); }
            set { value.SetUInt16(Offset + 0x26, Buff); }
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
                    Buff.BytesToUInt16(Offset + 0x28),
                    Buff.BytesToUInt16(Offset + 0x2A),
                    Buff.BytesToUInt16(Offset + 0x2C),
                    Buff.BytesToUInt16(Offset + 0x2E),
                    Buff.BytesToUInt16(Offset + 0x30),
                    Buff.BytesToUInt16(Offset + 0x32),
                    Buff.BytesToUInt16(Offset + 0x34),
                    Buff.BytesToUInt16(Offset + 0x36),
                    Buff.BytesToUInt16(Offset + 0x38),
                    Buff.BytesToUInt16(Offset + 0x3A)
                };
            }
            set
            {
                value[0].SetUInt16(Offset + 0x28, Buff);
                value[1].SetUInt16(Offset + 0x2A, Buff);
                value[2].SetUInt16(Offset + 0x2C, Buff);
                value[3].SetUInt16(Offset + 0x2E, Buff);
                value[4].SetUInt16(Offset + 0x30, Buff);
                value[5].SetUInt16(Offset + 0x32, Buff);
                value[6].SetUInt16(Offset + 0x34, Buff);
                value[7].SetUInt16(Offset + 0x36, Buff);
                value[8].SetUInt16(Offset + 0x38, Buff);
                value[9].SetUInt16(Offset + 0x3A, Buff);
            }
        }

        /// <summary>
        ///     Raw address of the NT header.
        /// </summary>
        public uint e_lfanew
        {
            get { return Buff.BytesToUInt32(Offset + 0x3C); }
            set { value.SetUInt32(Offset + 0x3C, Buff); }
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