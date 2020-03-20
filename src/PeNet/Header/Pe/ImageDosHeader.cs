using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The ImageDosHeader with which every PE file starts.
    /// </summary>
    public class ImageDosHeader : AbstractStructure
    {
        /// <summary>
        ///     Create a new ImageDosHeader object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset in the PE file to the DOS header.</param>
        public ImageDosHeader(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Magic "MZ" header.
        /// </summary>
        public ushort E_magic
        {
            get => PeFile.ReadUShort(Offset + 0x00);
            set => PeFile.WriteUShort(Offset + 0x00, value);
        }

        /// <summary>
        ///     Bytes on the last page of the file.
        /// </summary>
        public ushort E_cblp
        {
            get => PeFile.ReadUShort(Offset + 0x02);
            set => PeFile.WriteUShort(Offset + 0x02, value);
        }

        /// <summary>
        ///     Pages in the file.
        /// </summary>
        public ushort E_cp
        {
            get => PeFile.ReadUShort(Offset + 0x04);
            set => PeFile.WriteUShort(Offset + 0x04, value);
        }

        /// <summary>
        ///     Relocations.
        /// </summary>
        public ushort E_crlc
        {
            get => PeFile.ReadUShort(Offset + 0x06);
            set => PeFile.WriteUShort(Offset + 0x06, value);
        }

        /// <summary>
        ///     Size of the header in paragraphs.
        /// </summary>
        public ushort E_cparhdr
        {
            get => PeFile.ReadUShort(Offset + 0x08);
            set => PeFile.WriteUShort(Offset + 0x08, value);
        }

        /// <summary>
        ///     Minimum extra paragraphs needed.
        /// </summary>
        public ushort E_minalloc
        {
            get => PeFile.ReadUShort(Offset + 0x0A);
            set => PeFile.WriteUShort(Offset + 0x0A, value);
        }

        /// <summary>
        ///     Maximum extra paragraphs needed.
        /// </summary>
        public ushort E_maxalloc
        {
            get => PeFile.ReadUShort(Offset + 0x0C);
            set => PeFile.WriteUShort(Offset + 0x0C, value);
        }

        /// <summary>
        ///     Initial (relative) SS value.
        /// </summary>
        public ushort E_ss
        {
            get => PeFile.ReadUShort(Offset + 0x0E);
            set => PeFile.WriteUShort(Offset + 0x0E, value);
        }

        /// <summary>
        ///     Initial SP value.
        /// </summary>
        public ushort E_sp
        {
            get => PeFile.ReadUShort(Offset + 0x10);
            set => PeFile.WriteUShort(Offset + 0x10, value);
        }

        /// <summary>
        ///     Checksum
        /// </summary>
        public ushort E_csum
        {
            get => PeFile.ReadUShort(Offset + 0x12);
            set => PeFile.WriteUShort(Offset + 0x12, value);
        }

        /// <summary>
        ///     Initial IP value.
        /// </summary>
        public ushort E_ip
        {
            get => PeFile.ReadUShort(Offset + 0x14);
            set => PeFile.WriteUShort(Offset + 0x14, value);
        }

        /// <summary>
        ///     Initial (relative) CS value.
        /// </summary>
        public ushort E_cs
        {
            get => PeFile.ReadUShort(Offset + 0x16);
            set => PeFile.WriteUShort(Offset + 0x16, value);
        }

        /// <summary>
        ///     Raw address of the relocation table.
        /// </summary>
        public ushort E_lfarlc
        {
            get => PeFile.ReadUShort(Offset + 0x18);
            set => PeFile.WriteUShort(Offset + 0x18, value);
        }

        /// <summary>
        ///     Overlay number.
        /// </summary>
        public ushort E_ovno
        {
            get => PeFile.ReadUShort(Offset + 0x1A);
            set => PeFile.WriteUShort(Offset + 0x1A, value);
        }

        /// <summary>
        ///     Reserved.
        /// </summary>
        public ushort[] E_res // 4 * UInt16
        {
            get
            {
                return new[]
                {
                    PeFile.ReadUShort(Offset + 0x1C),
                    PeFile.ReadUShort(Offset + 0x1E),
                    PeFile.ReadUShort(Offset + 0x20),
                    PeFile.ReadUShort(Offset + 0x22)
                };
            }
            set
            {
                PeFile.WriteUShort(Offset + 0x1C, value[0]);
                PeFile.WriteUShort(Offset + 0x1E, value[1]);
                PeFile.WriteUShort(Offset + 0x20, value[2]);
                PeFile.WriteUShort(Offset + 0x22, value[3]);
            }
        }

        /// <summary>
        ///     OEM identifier.
        /// </summary>
        public ushort E_oemid
        {
            get => PeFile.ReadUShort(Offset + 0x24);
            set => PeFile.WriteUShort(Offset + 0x24, value);
        }

        /// <summary>
        ///     OEM information.
        /// </summary>
        public ushort E_oeminfo
        {
            get => PeFile.ReadUShort(Offset + 0x26);
            set => PeFile.WriteUShort(Offset + 0x26, value);
        }

        /// <summary>
        ///     Reserved.
        /// </summary>
        public ushort[] E_res2 // 10 * UInt16
        {
            get
            {
                return new[]
                {
                    PeFile.ReadUShort(Offset + 0x28),
                    PeFile.ReadUShort(Offset + 0x2A),
                    PeFile.ReadUShort(Offset + 0x2C),
                    PeFile.ReadUShort(Offset + 0x2E),
                    PeFile.ReadUShort(Offset + 0x30),
                    PeFile.ReadUShort(Offset + 0x32),
                    PeFile.ReadUShort(Offset + 0x34),
                    PeFile.ReadUShort(Offset + 0x36),
                    PeFile.ReadUShort(Offset + 0x38),
                    PeFile.ReadUShort(Offset + 0x3A)
                };
            }
            set
            {
                PeFile.WriteUShort(Offset + 0x28, value[0]);
                PeFile.WriteUShort(Offset + 0x2A, value[1]);
                PeFile.WriteUShort(Offset + 0x2C, value[2]);
                PeFile.WriteUShort(Offset + 0x2E, value[3]);
                PeFile.WriteUShort(Offset + 0x30, value[4]);
                PeFile.WriteUShort(Offset + 0x32, value[5]);
                PeFile.WriteUShort(Offset + 0x34, value[6]);
                PeFile.WriteUShort(Offset + 0x36, value[7]);
                PeFile.WriteUShort(Offset + 0x38, value[8]);
                PeFile.WriteUShort(Offset + 0x3A, value[9]);
            }
        }

        /// <summary>
        ///     Raw address of the NT header.
        /// </summary>
        public uint E_lfanew
        {
            get => PeFile.ReadUInt(Offset + 0x3C);
            set => PeFile.WriteUInt(Offset + 0x3C, value);
        }
    }
}