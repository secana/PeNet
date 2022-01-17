using System;
using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     Represents the optional header in
    ///     the NT header.
    /// </summary>
    public class ImageOptionalHeader : AbstractStructure
    {
        private readonly bool _is64Bit;

        /// <summary>
        ///     The Data Directories.
        /// </summary>
        public readonly ImageDataDirectory[] DataDirectory;

        /// <summary>
        ///     Create a new ImageOptionalHeader object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset to the optional header.</param>
        /// <param name="is64Bit">Set to true, if header is for a x64 application.</param>
        public ImageOptionalHeader(IRawFile peFile, long offset, bool is64Bit)
            : base(peFile, offset)
        {
            _is64Bit = is64Bit;

            DataDirectory = new ImageDataDirectory[16];

            for (uint i = 0; i < 16; i++)
            {
                if (!_is64Bit)
                    DataDirectory[i] = new ImageDataDirectory(peFile, offset + 0x60 + i*0x8);
                else
                    DataDirectory[i] = new ImageDataDirectory(peFile, offset + 0x70 + i*0x8);
            }
        }

        /// <summary>
        ///     Flag if the file is x32, x64 or a ROM image.
        /// </summary>
        public MagicType Magic
        {
            get => (MagicType) PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, (ushort) value);
        }

        /// <summary>
        ///     Major linker version.
        /// </summary>
        public byte MajorLinkerVersion
        {
            get => PeFile.ReadByte(Offset + 0x2);
            set => PeFile.WriteByte(Offset + 0x2, value);
        }

        /// <summary>
        ///     Minor linker version.
        /// </summary>
        public byte MinorLinkerVersion
        {
            get => PeFile.ReadByte(Offset + 0x3);
            set => PeFile.WriteByte(Offset + 03, value);
        }

        /// <summary>
        ///     Size of all code sections together.
        /// </summary>
        public uint SizeOfCode
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Size of all initialized data sections together.
        /// </summary>
        public uint SizeOfInitializedData
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        ///     Size of all uninitialized data sections together.
        /// </summary>
        public uint SizeOfUninitializedData
        {
            get => PeFile.ReadUInt(Offset + 0xC);
            set => PeFile.WriteUInt(Offset + 0xC, value);
        }

        /// <summary>
        ///     RVA of the entry point function.
        /// </summary>
        public uint AddressOfEntryPoint
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        ///     RVA to the beginning of the code section.
        /// </summary>
        public uint BaseOfCode
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        ///     RVA to the beginning of the data section.
        /// </summary>
        public uint BaseOfData
        {
            get => _is64Bit ? 0 : PeFile.ReadUInt(Offset + 0x18);
            set
            {
                if (!_is64Bit)
                    PeFile.WriteUInt(Offset + 0x18, value);
                else
                    throw new Exception("ImageOptionalHeader->BaseOfCode does not exist in 64 bit applications.");
            }
        }

        /// <summary>
        ///     Preferred address of the image when it's loaded to memory.
        /// </summary>
        public ulong ImageBase
        {
            get =>
                _is64Bit
                    ? PeFile.ReadULong(Offset + 0x18)
                    : PeFile.ReadUInt(Offset + 0x1C);
            set
            {
                if (!_is64Bit)
                    PeFile.WriteUInt(Offset + 0x1C, (uint) value);
                else
                    PeFile.WriteULong(Offset + 0x18, value);
            }
        }

        /// <summary>
        ///     Section alignment in memory in bytes. Must be greater or equal to the file alignment.
        /// </summary>
        public uint SectionAlignment
        {
            get => PeFile.ReadUInt(Offset + 0x20);
            set => PeFile.WriteUInt(Offset + 0x20, value);
        }

        /// <summary>
        ///     File alignment of the raw data of the sections in bytes.
        /// </summary>
        public uint FileAlignment
        {
            get => PeFile.ReadUInt(Offset + 0x24);
            set => PeFile.WriteUInt(Offset + 0x24, value);
        }

        /// <summary>
        ///     Major operation system version to run the file.
        /// </summary>
        public ushort MajorOperatingSystemVersion
        {
            get => PeFile.ReadUShort(Offset + 0x28);
            set => PeFile.WriteUShort(Offset + 0x28, value);
        }

        /// <summary>
        ///     Minor operation system version to run the file.
        /// </summary>
        public ushort MinorOperatingSystemVersion
        {
            get => PeFile.ReadUShort(Offset + 0x2A);
            set => PeFile.WriteUShort(Offset + 0x2A, value);
        }

        /// <summary>
        ///     Major image version.
        /// </summary>
        public ushort MajorImageVersion
        {
            get => PeFile.ReadUShort(Offset + 0x2C);
            set => PeFile.WriteUShort(Offset + 0x2C, value);
        }

        /// <summary>
        ///     Minor image version.
        /// </summary>
        public ushort MinorImageVersion
        {
            get => PeFile.ReadUShort(Offset + 0x2E);
            set => PeFile.WriteUShort(Offset + 0x2E, value);
        }

        /// <summary>
        ///     Major version of the subsystem.
        /// </summary>
        public ushort MajorSubsystemVersion
        {
            get => PeFile.ReadUShort(Offset + 0x30);
            set => PeFile.WriteUShort(Offset + 0x30, value);
        }

        /// <summary>
        ///     Minor version of the subsystem.
        /// </summary>
        public ushort MinorSubsystemVersion
        {
            get => PeFile.ReadUShort(Offset + 0x32);
            set => PeFile.WriteUShort(Offset + 0x32, value);
        }

        /// <summary>
        ///     Reserved and must be 0.
        /// </summary>
        public uint Win32VersionValue
        {
            get => PeFile.ReadUInt(Offset + 0x34);
            set => PeFile.WriteUInt(Offset + 0x34, value);
        }

        /// <summary>
        ///     Size of the image including all headers in bytes. Must be a multiple of
        ///     the section alignment.
        /// </summary>
        public uint SizeOfImage
        {
            get => PeFile.ReadUInt(Offset + 0x38);
            set => PeFile.WriteUInt(Offset + 0x38, value);
        }

        /// <summary>
        ///     Sum of the e_lfanwe from the DOS header, the 4 byte signature, size of
        ///     the file header, size of the optional header and size of all section.
        ///     Rounded to the next file alignment.
        /// </summary>
        public uint SizeOfHeaders
        {
            get => PeFile.ReadUInt(Offset + 0x3C);
            set => PeFile.WriteUInt(Offset + 0x3C, value);
        }

        /// <summary>
        ///     Image checksum validated at runtime for drivers, DLLs loaded at boot time and
        ///     DLLs loaded into a critical system.
        /// </summary>
        public uint CheckSum
        {
            get => PeFile.ReadUInt(Offset + 0x40);
            set => PeFile.WriteUInt(Offset + 0x40, value);
        }

        /// <summary>
        ///     The subsystem required to run the image e.g., Windows GUI, XBOX etc.
        /// </summary>
        public SubsystemType Subsystem
        {
            get => (SubsystemType) PeFile.ReadUShort(Offset + 0x44);
            set => PeFile.WriteUShort(Offset + 0x44, (ushort) value);
        }

        /// <summary>
        /// Subsystem resolved to a readable string.
        /// </summary>
        public string SubsystemResolved => ResolveSubsystem(Subsystem);

        /// <summary>
        ///     DLL characteristics of the image.
        /// </summary>
        public DllCharacteristicsType DllCharacteristics
        {
            get => (DllCharacteristicsType) PeFile.ReadUShort(Offset + 0x46);
            set => PeFile.WriteUShort(Offset + 0x46, (ushort) value);
        }

        /// <summary>
        ///     Size of stack reserve in bytes.
        /// </summary>
        public ulong SizeOfStackReserve
        {
            get =>
                _is64Bit
                    ? PeFile.ReadULong(Offset + 0x48)
                    : PeFile.ReadUInt(Offset + 0x48);
            set
            {
                if (!_is64Bit)
                    PeFile.WriteUInt(Offset + 0x48, (uint) value);
                else
                    PeFile.WriteULong(Offset + 0x48, value);
            }
        }

        /// <summary>
        ///     Size of bytes committed for the stack in bytes.
        /// </summary>
        public ulong SizeOfStackCommit
        {
            get =>
                _is64Bit
                    ? PeFile.ReadULong(Offset + 0x50)
                    : PeFile.ReadUInt(Offset + 0x4C);
            set
            {
                if (!_is64Bit)
                    PeFile.WriteUInt(Offset + 0x4C, (uint) value);
                else
                    PeFile.WriteULong(Offset + 0x50, value);
            }
        }

        /// <summary>
        ///     Size of the heap to reserve in bytes.
        /// </summary>
        public ulong SizeOfHeapReserve
        {
            get =>
                _is64Bit
                    ? PeFile.ReadULong(Offset + 0x58)
                    : PeFile.ReadUInt(Offset + 0x50);
            set
            {
                if (!_is64Bit)
                    PeFile.WriteUInt(Offset + 0x50, (uint) value);
                else
                    PeFile.WriteULong(Offset + 0x58, value);
            }
        }

        /// <summary>
        ///     Size of the heap commit in bytes.
        /// </summary>
        public ulong SizeOfHeapCommit
        {
            get =>
                _is64Bit
                    ? PeFile.ReadULong(Offset + 0x60)
                    : PeFile.ReadUInt(Offset + 0x54);
            set
            {
                if (!_is64Bit)
                    PeFile.WriteUInt(Offset + 0x54, (uint) value);
                else
                    PeFile.WriteULong(Offset + 0x60, value);
            }
        }

        /// <summary>
        ///     Obsolete
        /// </summary>
        public uint LoaderFlags
        {
            get =>
                _is64Bit
                    ? PeFile.ReadUInt(Offset + 0x68)
                    : PeFile.ReadUInt(Offset + 0x58);
            set
            {
                if (!_is64Bit)
                    PeFile.WriteUInt(Offset + 0x58, value);
                else
                    PeFile.WriteUInt(Offset + 0x68, value);
            }
        }

        /// <summary>
        ///     Number of directory entries in the remainder of the optional header.
        /// </summary>
        public uint NumberOfRvaAndSizes
        {
            get =>
                _is64Bit
                    ? PeFile.ReadUInt(Offset + 0x6C)
                    : PeFile.ReadUInt(Offset + 0x5C);
            set
            {
                if (!_is64Bit)
                    PeFile.WriteUInt(Offset + 0x5C, value);
                else
                    PeFile.WriteUInt(Offset + 0x6C, value);
            }
        }

        /// <summary>
        ///     Resolve the subsystem attribute to a human readable string.
        /// </summary>
        /// <param name="subsystem">Subsystem attribute.</param>
        /// <returns>Subsystem as readable string.</returns>
        public static string ResolveSubsystem(SubsystemType subsystem)
            => subsystem switch
            {
                SubsystemType.Unknown => "Unknown Subsystem",
                SubsystemType.Native => "Native",
                SubsystemType.WindowsGui => "Windows GUI",
                SubsystemType.WindowsCui => "Windows CUI",
                SubsystemType.Os2Cui => "OS/2 CUI",
                SubsystemType.PosixCui => "POSIX CUI",
                SubsystemType.WindowsCeGui => "Windows CE CUI",
                SubsystemType.EfiApplication => "EFI application",
                SubsystemType.EfiBootServiceDriver => "EFI boot service driver",
                SubsystemType.EfiRuntimeDriver => "EFI runtime service driver",
                SubsystemType.EfiRom => "EFI ROM image",
                SubsystemType.Xbox => "XBox",
                SubsystemType.WindowsBootApplication => "Windows boot application",
                _ => "Unknown Subsystem"
            };
    }

    /// <summary>
    /// Subsystem of the image.
    /// </summary>
    public enum SubsystemType : ushort
    {
        Unknown = 0,
        Native = 1,
        WindowsGui = 2,
        WindowsCui = 3,
        Os2Cui = 5,
        PosixCui = 7,
        WindowsCeGui = 9,
        EfiApplication = 10,
        EfiBootServiceDriver = 11,
        EfiRuntimeDriver = 12,
        EfiRom = 13,
        Xbox = 14,
        WindowsBootApplication = 16
    }

    /// <summary>
    ///     Constants for the Optional header DllCharacteristics
    ///     property.
    /// </summary>
    [Flags]
    public enum DllCharacteristicsType : ushort
    {
        /// <summary>
        ///     DLL can be relocated at load time.
        /// </summary>
        DynamicBase = 0x40,

        /// <summary>
        ///     Enforces integrity checks.
        /// </summary>
        ForceIntegrity = 0x80,

        /// <summary>
        ///     Image is compatible with Data Execution Prevention (DEP).
        /// </summary>
        NxCompat = 0x100,

        /// <summary>
        ///     Image is isolation aware but should not be isolated.
        /// </summary>
        NoIsolation = 0x200,

        /// <summary>
        ///     No Secure Exception Handling (SEH)
        /// </summary>
        NoSeh = 0x400,

        /// <summary>
        ///     Do not bind the image.
        /// </summary>
        NoBind,

        /// <summary>
        ///     Image is a WDM driver.
        /// </summary>
        WdmDriver = 0x2000,

        /// <summary>
        ///     Terminal server aware.
        /// </summary>
        TerminalServerAware = 0x8000
    }

    /// <summary>
    ///     Constants for the Optional header magic property.
    /// </summary>
    [Flags]
    public enum MagicType : ushort
    {
        /// <summary>
        ///     The file is an 32 bit executable.
        /// </summary>
        Bit32 = 0x10b,

        /// <summary>
        ///     The file is an 64 bit executable.
        /// </summary>
        Bit64 = 0x20b,

        /// <summary>
        ///     The file is a ROM image.
        /// </summary>
        Rom = 0x107
    }
}