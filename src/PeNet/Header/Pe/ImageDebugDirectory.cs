using PeNet.FileParser;
using PeNet.Header.Resource;
using System;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The ImageDebugDirectory hold debug information
    ///     about the PE file.
    /// </summary>
    public class ImageDebugDirectory : AbstractStructure
    {
        private CvInfoPdb70? _cvInfoPdb70;

        /// <summary>
        ///     Create a new ImageDebugDirectory object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset to the debug structure in the PE file.</param>
        public ImageDebugDirectory(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Characteristics of the debug information.
        /// </summary>
        public uint Characteristics
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     Time and date stamp
        /// </summary>
        public uint TimeDateStamp
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Major Version.
        /// </summary>
        public ushort MajorVersion
        {
            get => PeFile.ReadUShort(Offset + 0x8);
            set => PeFile.WriteUShort(Offset + 0x8, value);
        }

        /// <summary>
        ///     Minor Version.
        /// </summary>
        public ushort MinorVersion
        {
            get => PeFile.ReadUShort(Offset + 0xa);
            set => PeFile.WriteUShort(Offset + 0xa, value);
        }

        /// <summary>
        ///     Type
        ///     1: Coff
        ///     2: CV-PDB
        ///     9: Borland
        /// </summary>
        public uint Type
        {
            get => PeFile.ReadUInt(Offset + 0xc);
            set => PeFile.WriteUInt(Offset + 0xc, value);
        }

        public DebugDirectoryType DebugType
        {
            get => (DebugDirectoryType)Type;
            set => Type = (uint)value;
        }

        /// <summary>
        ///     Size of data.
        /// </summary>
        public uint SizeOfData
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        ///     Address of raw data.
        /// </summary>
        public uint AddressOfRawData
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        ///     Pointer to raw data.
        /// </summary>
        public uint PointerToRawData
        {
            get => PeFile.ReadUInt(Offset + 0x18);
            set => PeFile.WriteUInt(Offset + 0x18, value);
        }

        /// <summary>
        ///     PDB information if the "Type" is IMAGE_DEBUG_TYPE_CODEVIEW.
        /// </summary>
        public CvInfoPdb70? CvInfoPdb70
        {
            get
            {
                if (DebugType != DebugDirectoryType.CodeView)
                    return null;

                _cvInfoPdb70 ??= new CvInfoPdb70(
                    PeFile,
                    PointerToRawData);

                return _cvInfoPdb70;
            }
        }

        /// <summary>
        ///     Flags if the "Type" is IMAGE_DEBUG_TYPE_EX_DLLCHARACTERISTICS
        /// </summary>
        public ExtendedDllCharacteristicsType? ExtendedDllCharacteristics
        {
            get
            {
                if (DebugType != DebugDirectoryType.ExtendedDllCharacteristics)
                    return null;

                return (ExtendedDllCharacteristicsType)PeFile.ReadUInt(PointerToRawData);
            }
        }
    }

    /// <summary>
    ///     An image has potentially multiple entries of ImageDebugDirectory.
    ///     This is the "Type" of an entry.
    ///     Main source is winnt.h
    ///     See https://docs.microsoft.com/en-us/windows/win32/debug/pe-format#debug-type
    ///     and https://docs.microsoft.com/en-us/dotnet/api/system.reflection.portableexecutable.debugdirectoryentrytype?view=net-5.0
    /// </summary>
    public enum DebugDirectoryType : uint
    {
        /// <summary>
        ///     An unknown value that should be ignored by all tools.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     The COFF debug information (line numbers, symbol table, and string table). 
        ///     This type of debug information is also pointed to by fields in the file headers.
        /// </summary>
        Coff = 1,

        /// <summary>
        ///     Associated PDB file description. For more information, see the specification:
        ///     https://github.com/dotnet/runtime/blob/master/docs/design/specs/PE-COFF.md#codeview-debug-directory-entry-type-2
        /// </summary>
        CodeView = 2,

        /// <summary>
        ///     The frame pointer omission (FPO) information. 
        ///     This information tells the debugger how to interpret nonstandard stack frames, 
        ///     which use the EBP register for a purpose other than as a frame pointer.
        ///     Check winnt.h for struct FPO_DATA
        /// </summary>
        FramePointerOmission = 3,

        /// <summary>
        ///     The location of DBG file.
        /// </summary>
        Misc = 4,

        /// <summary>
        ///     A copy of .pdata section.
        /// </summary>
        Exception = 5,

        /// <summary>
        ///     Reserved.
        /// </summary>
        Fixup = 6,

        /// <summary>
        ///     The mapping from an RVA in image to an RVA in source image.
        /// </summary>
        OMapToSource = 7,

        /// <summary>
        ///     The mapping from an RVA in source image to an RVA in image.
        /// </summary>
        OMapFromSource = 8,

        /// <summary>
        ///     Reserved for Borland.
        /// </summary>
        Borland = 9,

        /// <summary>
        ///     Reserved.
        /// </summary>
        Reserved10 = 10,

        /// <summary>
        ///     Reserved.
        /// </summary>
        Clsid = 11,

        /// <summary>
        ///     5 uint values which "dumpbin /headers abc.exe" dumps as e.g.:
        ///     Counts: Pre-VC++ 11.00=0, C/C++=28, /GS=28, /sdl=1, guardN=27
        /// </summary>
        VcFeature = 12,

        /// <summary>
        ///     Profile guided optimization (aka PGO)
        /// </summary>
        Pogo = 13,

        /// <summary>
        ///     
        /// </summary>
        Iltcg = 14,

        /// <summary>
        ///     
        /// </summary>
        Mpx = 15,

        /// <summary>
        ///     The presence of this entry indicates a deterministic PE/COFF file.
        ///     The tool that produced the deterministic PE/COFF file guarantees that 
        ///     the entire content of the file is based solely on documented inputs given 
        ///     to the tool (such as source files, resource files, and compiler options) 
        ///     rather than ambient environment variables (such as the current time, 
        ///     the operating system, and the bitness of the process running the tool). 
        ///     The value of field TimeDateStamp in COFF File Header of a deterministic 
        ///     PE/COFF file does not indicate the date and time when the file was produced 
        ///     and should not be interpreted that way. Instead, the value of the field is 
        ///     derived from a hash of the file content. 
        ///     The algorithm to calculate this value is an implementation detail of the tool 
        ///     that produced the file. The debug directory entry of type Reproducible must 
        ///     have all fields, except for Type zeroed.
        ///     For more information, see the specification:
        ///     https://github.com/dotnet/runtime/blob/master/docs/design/specs/PE-COFF.md#deterministic-debug-directory-entry-type-16
        /// </summary>
        Reproducible = 16,

        /// <summary>
        ///     The entry points to a blob containing Embedded Portable PDB. The Embedded Portable PDB blob has the following format:
        ///     - blob ::= uncompressed-size data
        ///     - Data spans the remainder of the blob and contains a Deflate-compressed Portable PDB.
        ///     For more information, see the specification:
        ///     https://github.com/dotnet/runtime/blob/master/docs/design/specs/PE-COFF.md#embedded-portable-pdb-debug-directory-entry-type-17
        /// </summary>
        EmbeddedPortablePdb = 17,

        /// <summary>
        ///     
        /// </summary>
        Reserved18 = 18,

        /// <summary>
        ///     The entry stores a crypto hash of the content of the symbol file the PE/COFF 
        ///     file was built with. The hash can be used to validate that a given PDB file 
        ///     was built with the PE/COFF file and not altered in any way. 
        ///     More than one entry can be present if multiple PDBs were produced during the 
        ///     build of the PE/COFF file (for example, private and public symbols). 
        ///     For more information, see the specification:
        ///     https://github.com/dotnet/runtime/blob/master/docs/design/specs/PE-COFF.md#pdb-checksum-debug-directory-entry-type-19
        /// </summary>
        PdbChecksum = 19,

        /// <summary>
        ///     Extended DLL characteristics bits.
        ///     Raw data points to 4 bytes of type ExtendedDllCharacteristicsType
        /// </summary>
        ExtendedDllCharacteristics = 20
    }

    /// <summary>
    ///     Possible bit-field values when ImageDebugDirectory.Type is DebugDirectoryType.ExtendedDllCharacteristics
    /// </summary>
    [Flags]
    public enum ExtendedDllCharacteristicsType : uint
    {
        /// <summary>
        ///     
        /// </summary>
        Unknown = 0x00,

        /// <summary>
        ///     Image was linked with /CETCOMPAT thus declares it is CET/Shadow-Stack/HSP compatible.
        /// </summary>
        CetCompat = 0x01,

        /// <summary>
        ///     
        /// </summary>
        CetCompatStrictMode = 0x02,

        /// <summary>
        ///     
        /// </summary>
        CetSetContextIpValidationRelaxMod = 0x04,

        /// <summary>
        ///     
        /// </summary>
        CetDynamicApisAllowInProc = 0x08,

        /// <summary>
        ///     
        /// </summary>
        CetReserved1 = 0x10,

        /// <summary>
        ///     
        /// </summary>
        CetReserved2 = 0x20
    }
}