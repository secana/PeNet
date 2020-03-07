using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     Represents the section header for one section.
    /// </summary>
    public class IMAGE_SECTION_HEADER : AbstractStructure
    {
        /// <summary>
        ///     Create a new IMAGE_SECTION_HEADER object.
        /// </summary>
        /// <param name="imageBaseAddress">Base address of the image from the Optional header.</param>
        /// <param name="buff">A PE file.</param>
        /// <param name="offset">Raw offset to the section header.</param>
        public IMAGE_SECTION_HEADER(IRawFile peFile, long offset, ulong imageBaseAddress)
            : base(peFile, offset)
        {
            ImageBaseAddress = imageBaseAddress;
        }

        /// <summary>
        /// Base address of the image from the Optional header.
        /// </summary>
        public ulong ImageBaseAddress { get; }

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
                    PeFile.ReadByte(Offset + 0),
                    PeFile.ReadByte(Offset + 1),
                    PeFile.ReadByte(Offset + 2),
                    PeFile.ReadByte(Offset + 3),
                    PeFile.ReadByte(Offset + 4),
                    PeFile.ReadByte(Offset + 5),
                    PeFile.ReadByte(Offset + 6),
                    PeFile.ReadByte(Offset + 7)
                };
            }

            set
            {
                PeFile.WriteByte(Offset, value[0]);
                PeFile.WriteByte(Offset, value[1]);
                PeFile.WriteByte(Offset, value[2]);
                PeFile.WriteByte(Offset, value[3]);
                PeFile.WriteByte(Offset, value[4]);
                PeFile.WriteByte(Offset, value[5]);
                PeFile.WriteByte(Offset, value[6]);
                PeFile.WriteByte(Offset, value[7]);
            }
        }

        /// <summary>
        /// The section name byte array resolved to
        /// a string.
        /// </summary>
        public string NameResolved => FlagResolver.ResolveSectionName(Name);

        /// <summary>
        ///     Size of the section when loaded into memory. If it's bigger than
        ///     the raw data size, the rest of the section is filled with zeros.
        /// </summary>
        public uint VirtualSize
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }


        /// <summary>
        ///     RVA of the section start in memory.
        /// </summary>
        public uint VirtualAddress
        {
            get => PeFile.ReadUInt(Offset + 0xC);
            set => PeFile.WriteUInt(Offset + 0xC, value);
        }

        /// <summary>
        ///     Size of the section in raw on disk. Must be a multiple of the file alignment
        ///     specified in the optional header. If its less than the virtual size, the rest
        ///     is filled with zeros.
        /// </summary>
        public uint SizeOfRawData
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        ///     Raw address of the section in the file.
        /// </summary>
        public uint PointerToRawData
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        ///     Pointer to the beginning of the relocation. If there are none, the
        ///     value is zero.
        /// </summary>
        public uint PointerToRelocations
        {
            get => PeFile.ReadUInt(Offset + 0x18);
            set => PeFile.WriteUInt(Offset + 0x18, value);
        }

        /// <summary>
        ///     Pointer to the beginning of the line-numbers in the file.
        ///     Zero if there are no line-numbers in the file.
        /// </summary>
        public uint PointerToLinenumbers
        {
            get => PeFile.ReadUInt(Offset + 0x1C);
            set => PeFile.WriteUInt(Offset + 0x1C, value);
        }

        /// <summary>
        ///     The number of relocations for the section. Is zero for executable images.
        /// </summary>
        public ushort NumberOfRelocations
        {
            get => PeFile.ReadUShort(Offset + 0x20);
            set => PeFile.WriteUShort(Offset + 0x20, value);
        }

        /// <summary>
        ///     The number of line-number entries for the section.
        /// </summary>
        public ushort NumberOfLinenumbers
        {
            get => PeFile.ReadUShort(Offset + 0x22);
            set => PeFile.WriteUShort(Offset + 0x22, value);
        }

        /// <summary>
        ///     Section characteristics. Can be resolved with
        /// </summary>
        public uint Characteristics
        {
            get => PeFile.ReadUInt(Offset + 0x24);
            set => PeFile.WriteUInt(Offset + 0x24, value);
        }
    }
}
