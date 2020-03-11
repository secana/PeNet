using System;
using PeNet.FileParser;

namespace PeNet.Structures
{
    /// <summary>
    ///     The File header contains information about the structure
    ///     and properties of the PE file.
    /// </summary>
    public class ImageFileHeader : AbstractStructure
    {
        /// <summary>
        ///     Create a new ImageFileHeader object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset to the file header.</param>
        public ImageFileHeader(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     The machine (CPU type) the PE file is intended for.
        ///     For a string use MachineResolved
        /// </summary>
        public MachineType Machine
        {
            get => (MachineType) PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, (ushort) value);
        }

        /// <summary>
        /// String representation of the Machine flag.
        /// </summary>
        public string MachineResolved => ResolveMachine(Machine);

        /// <summary>
        ///     The number of sections in the PE file.
        /// </summary>
        public ushort NumberOfSections
        {
            get => PeFile.ReadUShort(Offset + 0x2);
            set => PeFile.WriteUShort(Offset + 0x2, value);
        }

        /// <summary>
        ///     Time and date stamp.
        /// </summary>
        public uint TimeDateStamp
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Pointer to COFF symbols table. They are rare in PE files,
        ///     but often in obj files.
        /// </summary>
        public uint PointerToSymbolTable
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        ///     The number of COFF symbols.
        /// </summary>
        public uint NumberOfSymbols
        {
            get => PeFile.ReadUInt(Offset + 0xC);
            set => PeFile.WriteUInt(Offset + 0xC, value);
        }

        /// <summary>
        ///     The size of the optional header which follow the file header.
        /// </summary>
        public ushort SizeOfOptionalHeader
        {
            get => PeFile.ReadUShort(Offset + 0x10);
            set => PeFile.WriteUShort(Offset + 0x10, value);
        }

        /// <summary>
        ///     Set of flags which describe the PE file in detail.
        /// </summary>
        public FileCharacteristics Characteristics 
            => ResolveFileCharacteristics(PeFile.ReadUShort(Offset + 0x12));

        /// <summary>
        ///     Resolves the characteristics attribute from the COFF header to an
        ///     object which holds all the characteristics a boolean properties.
        /// </summary>
        /// <param name="characteristics">File header characteristics.</param>
        /// <returns>Object with all characteristics as boolean properties.</returns>
        public static FileCharacteristics ResolveFileCharacteristics(ushort characteristics)
        {
            return new FileCharacteristics(characteristics);
        }

        /// <summary>
        ///     Resolves the target machine number to a string containing
        ///     the name of the target machine.
        /// </summary>
        /// <param name="targetMachine">Target machine value from the COFF header.</param>
        /// <returns>Name of the target machine as string.</returns>
        public static string ResolveMachine(MachineType targetMachine)
            => targetMachine switch
            {
                MachineType.I386 => "Intel 386",
                MachineType.I860 => "Intel i860",
                MachineType.R3000 => "MIPS R3000",
                MachineType.R4000 => "MIPS little endian (R4000)",
                MachineType.R10000 => "MIPS R10000",
                MachineType.WCEMIPSV2 => "MIPS little endian WCI v2",
                MachineType.OLDALPHA => "old Alpha AXP",
                MachineType.ALPHA => "Alpha AXP",
                MachineType.SH3 => "Hitachi SH3",
                MachineType.SH3DSP => "Hitachi SH3 DSP",
                MachineType.SH3E => "Hitachi SH3E",
                MachineType.SH4 => "Hitachi SH4",
                MachineType.SH5 => "Hitachi SH5",
                MachineType.ARM => "ARM little endian",
                MachineType.THUMB => "Thumb",
                MachineType.AM33 => "Matsushita AM33",
                MachineType.POWERPC => "PowerPC little endian",
                MachineType.POWERPCFP => "PowerPC with floating point support",
                MachineType.IA64 => "Intel IA64",
                MachineType.MIPS16 => "MIPS16",
                MachineType.M68K => "Motorola 68000 series",
                MachineType.ALPHA64 => "Alpha AXP 64-bit",
                MachineType.MIPSFPU => "MIPS with FPU",
                MachineType.TRICORE => "Tricore",
                MachineType.CEF => "CEF",
                MachineType.MIPSFPU16 => "MIPS16 with FPU",
                MachineType.EBC => "EFI Byte Code",
                MachineType.AMD64 => "AMD64",
                MachineType.M32R => "Mitsubishi M32R little endian",
                MachineType.CEE => "clr pure MSIL",
                MachineType.ARM64 => "ARM64 Little-Endian",
                MachineType.ARMNT => "ARM Thumb-2 Little-Endian",
                MachineType.TARGET_HOST => "Interacts with the host and not a WOW64 guest",
                _ => "unknown"
            };
    }

    /// <summary>
    ///     ImageFileHeader machine constants which define
    ///     for which CPU type the PE file is.
    /// </summary>
    [Flags]
    public enum MachineType : ushort
    {
        /// <summary>
        ///     File header -> machine (CPU): unknown
        /// </summary>
        IMAGE_FILE_MACHINE_UNKNOWN = 0x0,

        /// <summary>
        ///     File header -> machine (CPU): Intel 386
        /// </summary>
        I386 = 0x14c,

        /// <summary>
        ///     File header -> machine (CPU): Intel i860
        /// </summary>
        I860 = 0x14d,

        /// <summary>
        ///     File header -> machine (CPU): MIPS R3000
        /// </summary>
        R3000 = 0x162,

        /// <summary>
        ///     File header -> machine (CPU): MIPS little endian (R4000)
        /// </summary>
        R4000 = 0x166,

        /// <summary>
        ///     File header -> machine (CPU): MIPS R10000
        /// </summary>
        R10000 = 0x168,

        /// <summary>
        ///     File header -> machine (CPU): MIPS little endian WCI v2
        /// </summary>
        WCEMIPSV2 = 0x169,

        /// <summary>
        ///     File header -> machine (CPU): old Alpha AXP
        /// </summary>
        OLDALPHA = 0x183,

        /// <summary>
        ///     File header -> machine (CPU): Alpha AXP
        /// </summary>
        ALPHA = 0x184,

        /// <summary>
        ///     File header -> machine (CPU): Hitachi SH3
        /// </summary>
        SH3 = 0x1a2,

        /// <summary>
        ///     File header -> machine (CPU): Hitachi SH3 DSP
        /// </summary>
        SH3DSP = 0x1a3,

        /// <summary>
        ///     File header -> machine (CPU): unknown
        /// </summary>
        SH3E = 0x1a4,

        /// <summary>
        ///     File header -> machine (CPU): Hitachi SH4
        /// </summary>
        SH4 = 0x1a6,

        /// <summary>
        ///     File header -> machine (CPU): Hitachi SH5
        /// </summary>
        SH5 = 0x1a8,

        /// <summary>
        ///     File header -> machine (CPU): ARM little endian
        /// </summary>
        ARM = 0x1c0,

        /// <summary>
        ///     File header -> machine (CPU): Thumb
        /// </summary>
        THUMB = 0x1c2,

        /// <summary>
        ///     File header -> machine (CPU): Matsushita AM33
        /// </summary>
        AM33 = 0x1d3,

        /// <summary>
        ///     File header -> machine (CPU): PowerPC little endian
        /// </summary>
        POWERPC = 0x1f0,

        /// <summary>
        ///     File header -> machine (CPU): PowerPC with floating point support
        /// </summary>
        POWERPCFP = 0x1f1,

        /// <summary>
        ///     File header -> machine (CPU): Intel IA64
        /// </summary>
        IA64 = 0x200,

        /// <summary>
        ///     File header -> machine (CPU): MIPS16
        /// </summary>
        MIPS16 = 0x266,

        /// <summary>
        ///     File header -> machine (CPU): Motorola 68000 series
        /// </summary>
        M68K = 0x268,

        /// <summary>
        ///     File header -> machine (CPU): Alpha AXP 64-bit
        /// </summary>
        ALPHA64 = 0x284,

        /// <summary>
        ///     File header -> machine (CPU): MIPS with FPU
        /// </summary>
        MIPSFPU = 0x366,

        /// <summary>
        ///     File header -> machine (CPU): MIPS16 with FPU
        /// </summary>
        MIPSFPU16 = 0x466,

        /// <summary>
        ///     File header -> machine (CPU): Alpha AXP 64-bit
        /// </summary>
        IMAGE_FILE_MACHINE_AXP64 = ALPHA64,

        /// <summary>
        ///     File header -> machine (CPU): unknown
        /// </summary>
        TRICORE = 0x520,

        /// <summary>
        ///     File header -> machine (CPU): unknown
        /// </summary>
        CEF = 0xcef,

        /// <summary>
        ///     File header -> machine (CPU): EFI Byte Code
        /// </summary>
        EBC = 0xebc,

        /// <summary>
        ///     File header -> machine (CPU): AMD AMD64 (Used for Intel x64, too)
        /// </summary>
        AMD64 = 0x8664,

        /// <summary>
        ///     File header -> machine (CPU): Mitsubishi M32R little endian
        /// </summary>
        M32R = 0x9041,

        /// <summary>
        ///     File header -> machine (CPU): clr pure MSIL (.Net)
        /// </summary>
        CEE = 0xc0ee,

        /// <summary>
        ///     File header -> machine (CPU): ARM65 Little-Endian
        /// </summary>
        ARM64 = 0xAA64,

        /// <summary>
        ///     File header -> machine (CPU): ARM Thumb-2 Little-Endian
        /// </summary>
        ARMNT = 0x01C4,

        /// <summary>
        ///     File header -> machine (CPU): Interacts with the host and not
        ///     a WOW64 guest
        /// </summary>
        TARGET_HOST = 0x0001
    }
}