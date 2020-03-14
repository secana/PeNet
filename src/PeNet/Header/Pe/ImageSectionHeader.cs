using System;
using System.Collections.Generic;
using System.Text;
using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     Represents the section header for one section.
    /// </summary>
    public class ImageSectionHeader : AbstractStructure
    {
        /// <summary>
        ///     Create a new ImageSectionHeader object.
        /// </summary>
        /// <param name="imageBaseAddress">Base address of the image from the Optional header.</param>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset to the section header.</param>
        public ImageSectionHeader(IRawFile peFile, long offset, ulong imageBaseAddress)
            : base(peFile, offset)
        {
            ImageBaseAddress = imageBaseAddress;
        }

        /// <summary>
        /// Base address of the image from the Optional header.
        /// </summary>
        public ulong ImageBaseAddress { get; }

        /// <summary>
        /// The section name as a string.
        /// </summary>
        public string Name 
        { 
            get 
            {
                var s = PeFile.AsSpan(Offset, 8);
                return Encoding.UTF8.GetString(s).TrimEnd((char)0);
            }
            set
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                PeFile.WriteBytes(Offset, bytes);
            }
        }
        

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
        ///     Section characteristics.
        /// </summary>
        public ScnCharacteristicsType Characteristics
        {
            get => (ScnCharacteristicsType) PeFile.ReadUInt(Offset + 0x24);
            set => PeFile.WriteUInt(Offset + 0x24, (uint) value);
        }

        /// <summary>
        /// The section characteristics flags resolved to
        /// readable strings.
        /// </summary>
        public List<string> CharacteristicsResolved => ResolveCharacteristics(Characteristics);

        /// <summary>
        ///     Resolves the section flags to human readable strings.
        /// </summary>
        /// <param name="sectionFlags">Sections flags from the SectionHeader object.</param>
        /// <returns>List with flag names for the section.</returns>
        public static List<string> ResolveCharacteristics(ScnCharacteristicsType sectionFlags)
        {
            var st = new List<string>();
            foreach (var flag in (ScnCharacteristicsType[])Enum.GetValues(typeof(ScnCharacteristicsType)))
            {
                if ((sectionFlags & flag) == flag)
                {
                    st.Add(flag.ToString());
                }
            }
            return st;
        }
    }

    /// <summary>
    ///     The SectionFlags enumeration lists all possible flags which can
    ///     be set in the section characteristics.
    /// </summary>
    [Flags]
    public enum ScnCharacteristicsType : uint
    {
        /// <summary>
        ///     Reserved.
        /// </summary>
        TypeNoPad = 0x00000008,

        /// <summary>
        ///     Section contains code.
        /// </summary>
        CntCode = 0x00000020,

        /// <summary>
        ///     Section contains initialized data.
        /// </summary>
        CntInitializedData = 0x00000040,

        /// <summary>
        ///     Section contains uninitialized data.
        /// </summary>
        CntUninitializedData = 0x00000080,

        /// <summary>
        ///     Reserved.
        /// </summary>
        LnkOther = 0x00000100,

        /// <summary>
        ///     Section contains comments or some  other type of information.
        /// </summary>
        LnkInfo = 0x00000200,

        /// <summary>
        ///     Section contents will not become part of image.
        /// </summary>
        LnkRemove = 0x00000800,

        /// <summary>
        ///     Section contents comdat.
        /// </summary>
        LnkComdat = 0x00001000,

        /// <summary>
        ///     Reset speculative exceptions handling bits in the TLB entries for this section.
        /// </summary>
        NoDeferSpecExc = 0x00004000,

        /// <summary>
        ///     Section content can be accessed relative to GP.
        /// </summary>
        Gprel = 0x00008000,

        /// <summary>
        ///     Unknown.
        /// </summary>
        MemFardata = 0x00008000,

        /// <summary>
        ///     Unknown.
        /// </summary>
        MemPurgeable = 0x00020000,

        /// <summary>
        ///     Unknown.
        /// </summary>
        Mem16Bit = 0x00020000,

        /// <summary>
        ///     Unknown.
        /// </summary>
        MemLocked = 0x00040000,

        /// <summary>
        ///     Unknown.
        /// </summary>
        MemPreload = 0x00080000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align1Bytes = 0x00100000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align2Bytes = 0x00200000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align4Bytes = 0x00300000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align8Bytes = 0x00400000,

        /// <summary>
        ///     Default alignment if no others are specified.
        /// </summary>
        Align16Bytes = 0x00500000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align32Bytes = 0x00600000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align64Bytes = 0x00700000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align128Bytes = 0x00800000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align256Bytes = 0x00900000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align512Bytes = 0x00A00000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align1024Bytes = 0x00B00000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align2048Bytes = 0x00C00000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align4096Bytes = 0x00D00000,

        /// <summary>
        ///     Section alignment.
        /// </summary>
        Align8192Bytes = 0x00E00000,

        /// <summary>
        ///     Alignment mask.
        /// </summary>
        AlignMask = 0x00F00000,

        /// <summary>
        ///     Section contains extended relocations.
        /// </summary>
        LnkNrelocOvfl = 0x01000000,

        /// <summary>
        ///     Section can be discarded.
        /// </summary>
        MemDiscardable = 0x02000000,

        /// <summary>
        ///     Section is not cache-able.
        /// </summary>
        MemNotCached = 0x04000000,

        /// <summary>
        ///     Section is not page-able.
        /// </summary>
        MemNotPaged = 0x08000000,

        /// <summary>
        ///     Section is shareable.
        /// </summary>
        MemShared = 0x10000000,

        /// <summary>
        ///     Section is executable.
        /// </summary>
        MemExecute = 0x20000000,

        /// <summary>
        ///     Section is readable.
        /// </summary>
        MemRead = 0x40000000,

        /// <summary>
        ///     Section is write-able.
        /// </summary>
        MemWrite = 0x80000000
    }
}
