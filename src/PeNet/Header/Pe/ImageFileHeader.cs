using System;
using System.Collections.Generic;
using PeNet.FileParser;

namespace PeNet.Header.Pe
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
        public FileCharacteristicsType Characteristics
        {
            get => (FileCharacteristicsType) PeFile.ReadUShort(Offset + 0x12);
            set => PeFile.WriteUShort(Offset + 0x12, (ushort) value);
        }

        /// <summary>
        ///     Set of flags which describe the PE file in detail resolved
        ///     to a readable list of strings.
        /// </summary>
        public List<string> CharacteristicsResolved 
            => ResolveFileCharacteristics(Characteristics);

        /// <summary>
        ///     Resolves the characteristics attribute from the COFF header to an
        ///     object which holds all the characteristics a boolean properties.
        /// </summary>
        /// <param name="characteristics">File header characteristics.</param>
        /// <returns>List with set flags.</returns>
        public static List<string> ResolveFileCharacteristics(FileCharacteristicsType characteristics)
        {
            var st = new List<string>();
            foreach (var flag in (FileCharacteristicsType[])Enum.GetValues(typeof(FileCharacteristicsType)))
            {
                if ((characteristics & flag) == flag)
                {
                    st.Add(flag.ToString());
                }
            }
            return st;
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
                MachineType.Wcemipsv2 => "MIPS little endian WCI v2",
                MachineType.OldAlpha => "old Alpha AXP",
                MachineType.Alpha => "Alpha AXP",
                MachineType.Sh3 => "Hitachi SH3",
                MachineType.Sh3Dsp => "Hitachi SH3 DSP",
                MachineType.Sh3E => "Hitachi SH3E",
                MachineType.Sh4 => "Hitachi SH4",
                MachineType.Sh5 => "Hitachi SH5",
                MachineType.Arm => "ARM little endian",
                MachineType.Thumb => "Thumb",
                MachineType.Am33 => "Matsushita AM33",
                MachineType.PowerPc => "PowerPC little endian",
                MachineType.PowerPcFp => "PowerPC with floating point support",
                MachineType.Ia64 => "Intel IA64",
                MachineType.Mips16 => "MIPS16",
                MachineType.M68K => "Motorola 68000 series",
                MachineType.Alpha64 => "Alpha AXP 64-bit",
                MachineType.MipsFpu => "MIPS with FPU",
                MachineType.TriCore => "Tricore",
                MachineType.Cef => "CEF",
                MachineType.MipsFpu16 => "MIPS16 with FPU",
                MachineType.Ebc => "EFI Byte Code",
                MachineType.Amd64 => "AMD64",
                MachineType.M32R => "Mitsubishi M32R little endian",
                MachineType.Cee => "clr pure MSIL",
                MachineType.Arm64 => "ARM64 Little-Endian",
                MachineType.ArmNt => "ARM Thumb-2 Little-Endian",
                MachineType.TargetHost => "Interacts with the host and not a WOW64 guest",
                _ => "unknown"
            };
    }

    /// <summary>
    ///     File characteristics from the file header.
    /// </summary>
    [Flags]
    public enum FileCharacteristicsType : ushort
    {
        /// <summary>
        ///     Relocation stripped.
        /// </summary>
        RelocsStripped = 0x01,

        /// <summary>
        ///     Executable image.
        /// </summary>
        ExecutableImage = 0x02,

        /// <summary>
        ///     Line numbers stripped.
        /// </summary>
        LineNumsStripped = 0x04,

        /// <summary>
        ///     Local symbols stripped.
        /// </summary>
        LocalSymsStripped = 0x08,

        /// <summary>
        ///     (OBSOLTETE) Aggressively trim the working set.
        /// </summary>
        AggresiveWsTrim = 0x10,

        /// <summary>
        ///     Application can handle addresses larger than 2 GB.
        /// </summary>
        LargeAddressAware = 0x20,

        /// <summary>
        ///     (OBSOLTETE) Bytes of word are reversed.
        /// </summary>
        BytesReversedLo = 0x80,

        /// <summary>
        ///     Supports 32 Bit words.
        /// </summary>
        BitMachine32 = 0x100,

        /// <summary>
        ///     Debug stripped and stored in a separate file.
        /// </summary>
        DebugStripped = 0x200,

        /// <summary>
        ///     If the image is on a removable media, copy and run it from the swap file.
        /// </summary>
        RemovableRunFromSwap = 0x400,

        /// <summary>
        ///     If the image is on the network, copy and run it from the swap file.
        /// </summary>
        NetRunFromSwap = 0x800,

        /// <summary>
        ///     The image is a system file.
        /// </summary>
        System = 0x1000,

        /// <summary>
        ///     Is a dynamic loaded library and executable but cannot
        ///     be run on its own.
        /// </summary>
        Dll = 0x2000,

        /// <summary>
        ///     Image should be run only on uniprocessor.
        /// </summary>
        UpSystemOnly = 0x4000,

        /// <summary>
        ///     (OBSOLETE) Reserved.
        /// </summary>
        BytesReversedHi = 0x8000
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
        Unknown = 0x0,

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
        Wcemipsv2 = 0x169,

        /// <summary>
        ///     File header -> machine (CPU): old Alpha AXP
        /// </summary>
        OldAlpha = 0x183,

        /// <summary>
        ///     File header -> machine (CPU): Alpha AXP
        /// </summary>
        Alpha = 0x184,

        /// <summary>
        ///     File header -> machine (CPU): Hitachi SH3
        /// </summary>
        Sh3 = 0x1a2,

        /// <summary>
        ///     File header -> machine (CPU): Hitachi SH3 DSP
        /// </summary>
        Sh3Dsp = 0x1a3,

        /// <summary>
        ///     File header -> machine (CPU): unknown
        /// </summary>
        Sh3E = 0x1a4,

        /// <summary>
        ///     File header -> machine (CPU): Hitachi SH4
        /// </summary>
        Sh4 = 0x1a6,

        /// <summary>
        ///     File header -> machine (CPU): Hitachi SH5
        /// </summary>
        Sh5 = 0x1a8,

        /// <summary>
        ///     File header -> machine (CPU): ARM little endian
        /// </summary>
        Arm = 0x1c0,

        /// <summary>
        ///     File header -> machine (CPU): Thumb
        /// </summary>
        Thumb = 0x1c2,

        /// <summary>
        ///     File header -> machine (CPU): Matsushita AM33
        /// </summary>
        Am33 = 0x1d3,

        /// <summary>
        ///     File header -> machine (CPU): PowerPC little endian
        /// </summary>
        PowerPc = 0x1f0,

        /// <summary>
        ///     File header -> machine (CPU): PowerPC with floating point support
        /// </summary>
        PowerPcFp = 0x1f1,

        /// <summary>
        ///     File header -> machine (CPU): Intel IA64
        /// </summary>
        Ia64 = 0x200,

        /// <summary>
        ///     File header -> machine (CPU): MIPS16
        /// </summary>
        Mips16 = 0x266,

        /// <summary>
        ///     File header -> machine (CPU): Motorola 68000 series
        /// </summary>
        M68K = 0x268,

        /// <summary>
        ///     File header -> machine (CPU): Alpha AXP 64-bit
        /// </summary>
        Alpha64 = 0x284,

        /// <summary>
        ///     File header -> machine (CPU): MIPS with FPU
        /// </summary>
        MipsFpu = 0x366,

        /// <summary>
        ///     File header -> machine (CPU): MIPS16 with FPU
        /// </summary>
        MipsFpu16 = 0x466,

        /// <summary>
        ///     File header -> machine (CPU): Alpha AXP 64-bit
        /// </summary>
        Axp64 = Alpha64,

        /// <summary>
        ///     File header -> machine (CPU): unknown
        /// </summary>
        TriCore = 0x520,

        /// <summary>
        ///     File header -> machine (CPU): unknown
        /// </summary>
        Cef = 0xcef,

        /// <summary>
        ///     File header -> machine (CPU): EFI Byte Code
        /// </summary>
        Ebc = 0xebc,

        /// <summary>
        ///     File header -> machine (CPU): AMD AMD64 (Used for Intel x64, too)
        /// </summary>
        Amd64 = 0x8664,

        /// <summary>
        ///     File header -> machine (CPU): Mitsubishi M32R little endian
        /// </summary>
        M32R = 0x9041,

        /// <summary>
        ///     File header -> machine (CPU): clr pure MSIL (.Net)
        /// </summary>
        Cee = 0xc0ee,

        /// <summary>
        ///     File header -> machine (CPU): ARM65 Little-Endian
        /// </summary>
        Arm64 = 0xAA64,

        /// <summary>
        ///     File header -> machine (CPU): ARM Thumb-2 Little-Endian
        /// </summary>
        ArmNt = 0x01C4,

        /// <summary>
        ///     File header -> machine (CPU): Interacts with the host and not
        ///     a WOW64 guest
        /// </summary>
        TargetHost = 0x0001
    }
}