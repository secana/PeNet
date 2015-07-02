using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PeParser
{
    /// <summary>
    /// http://2.bp.blogspot.com/-e_0ck42SsMI/TYolcp5bivI/AAAAAAAAAAU/ktRY5QSQXV4/s1600/PE_Format.png
    /// http://en.wikibooks.org/wiki/X86_Disassembly/Windows_Executable_Files#PE_Header
    /// Class to represent an Portable Executable Header
    /// </summary>
    public class PEHeader
    {
        private static readonly string _tableFormat = "\t{0,-35}:\t{1,30}\n";
        private static string ToStringReflection(object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sb = new StringBuilder();
            foreach (var p in properties)
            {
                if(p.PropertyType.IsArray)
                    continue;
                sb.AppendFormat(_tableFormat, p.Name, p.GetValue(obj, null));
            }
                
            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder("-- PE HEADER --\n");
            sb.Append(MSDOSHeader.ToString());
            sb.Append(PESignature.ToString());
            sb.Append(COFFHeader.ToString());
            sb.Append(StandardFields.ToString());
            sb.Append(WindowsSpecificFields.ToString());
            sb.Append(DataDirectories.ToString());
            sb.Append(SectionTable.ToString());

            return sb.ToString();
        }

        public class MSDOSHEADER
        {
            public ushort MZ { get; set; }
            public ushort lastSize { get; set; }
            public ushort PageInFiles { get; set; }
            public ushort relocations { get; set; }
            public ushort headerSizeInParagraph { get; set; }
            public ushort MinExtraParagraphNeeded { get; set; }
            public ushort MaxExtraParagraphNeeded { get; set; }
            public ushort InitialRelativeSS { get; set; }
            public ushort InitialRelativeSP { get; set; }
            public ushort checksum { get; set; }
            public ushort InitialIP { get; set; }
            public ushort InitialRelativeCS { get; set; }
            public ushort FileAddOfRelocTable { get; set; }
            public ushort OverlayNumber { get; set; }
            /*
             * 4 * ushort reserved
             */
            public ushort OEMIdentifier { get; set; }
            public ushort OEMInformation { get; set; }
            /*
             * 10 * ushort reserved
             */
            public UInt32 OffsetPEToSignature { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder("MSDOS HEADER\n");
                sb.Append(ToStringReflection(this));
                return sb.ToString();
            }
        }

        public class PESIGNATURE
        {
            public UInt32 PESignature { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder("PE Signature\n");
                sb.Append(ToStringReflection(this));
                return sb.ToString();
            }
        }

        public class COFFHEADER
        {
            public ushort TargetMachine { get; set; }        // This field determines what machine the file was compiled for. 
            public ushort NumberOfSections { get; set; }     // The number of sections that are described at the end of the PE headers.
            public UInt32 TimeDateStamp { get; set; }        // 32 bit time at which this header was generated: is used in the process of "Binding".
            public UInt32 PointerToSymbolTable { get; set; }
            public UInt32 NumberOfSymbols { get; set; }
            public ushort SizeOfOptionalHeaders { get; set; } // This field shows how long the "PE Optional Header" is that follows the COFF header.
            public ushort Characteristics { get; set; }

            /// <summary>
            /// Resolves the target machine number to a string containing
            /// the name of the target machine.
            /// </summary>
            /// <param name="targetMachine">Target machine value from the COFF header.</param>
            /// <returns>Name of the target machine as string.</returns>
            public static string ResolveTargetMachine(ushort targetMachine)
            {
                string tm = "unknown";
                switch (targetMachine)
                {
                    case 0x14c:
                        tm = "Intel 386";
                        break;
                    case 0x14d:
                        tm = "Intel i860";
                        break;
                    case 0x162:
                        tm = "MIPS R3000";
                        break;
                    case 0x166:
                        tm = "MIPS little endian (R4000)";
                        break;
                    case 0x168:
                        tm = "MIPS R10000";
                        break;
                    case 0x169:
                        tm = "MIPS little endian WCI v2";
                        break;
                    case 0x183:
                        tm = "old Alpha AXP";
                        break;
                    case 0x184:
                        tm = "Alpha AXP";
                        break;
                    case 0x1a2:
                        tm = "Hitachi SH3";
                        break;
                    case 0x1a3:
                        tm = "Hitachi SH3 DSP";
                        break;
                    case 0x1a6:
                        tm = "Hitachi SH4";
                        break;
                    case 0x1a8:
                        tm = "Hitachi SH5";
                        break;
                    case 0x1c0:
                        tm = "ARM little endian";
                        break;
                    case 0x1c2:
                        tm = "Thumb";
                        break;
                    case 0x1d3:
                        tm = "Matsushita AM33";
                        break;
                    case 0x1f0:
                        tm = "PowerPC little endian";
                        break;
                    case 0x1f1:
                        tm = "PowerPC with floating point support";
                        break;
                    case 0x200:
                        tm = "Intel IA64";
                        break;
                    case 0x266:
                        tm = "MIPS16";
                        break;
                    case 0x268:
                        tm = "Motorola 68000 series";
                        break;
                    case 0x284:
                        tm = "Alpha AXP 64-bit";
                        break;
                    case 0x366:
                        tm = "MIPS with FPU";
                        break;
                    case 0x466:
                        tm = "MIPS16 with FPU";
                        break;
                    case 0xebc:
                        tm = "EFI Byte Code";
                        break;
                    case 0x8664:
                        tm = "AMD AMD64";
                        break;
                    case 0x9041:
                        tm = "Mitsubishi M32R little endian";
                        break;
                    case 0xc0ee:
                        tm = "clr pure MSIL";
                        break;
                }

                return tm;
            }

            /// <summary>
            /// Resolves the characteristics attribute from the COFF header to a human
            /// readable string.
            /// </summary>
            /// <param name="characteristics">COFF header characteristics.</param>
            /// <returns>Human readable characteristics string.</returns>
            public static string ResolveCharacteristics(ushort characteristics)
            {
                var c = "unknown";
                if ((characteristics & 0x02) == 0x02)
                    c = "EXE";
                else if ((characteristics & 0x200) == 0x200)
                    c = "File is non-relocatable (addresses are absolute, not RVA).";
                else if ((characteristics & 0x2000) == 0x2000)
                    c = "DLL";
                return c;
            }

            public override string ToString()
            {
                var sb = new StringBuilder("COFF Header\n");
                sb.Append(ToStringReflection(this));
                sb.AppendFormat(_tableFormat, "Resolved TargetMachine", ResolveTargetMachine(TargetMachine));
                sb.AppendFormat(_tableFormat, "Resolved Characteristic", ResolveCharacteristics(Characteristics));
                return sb.ToString();
            }
        }

        public class STANDARDFIELDS
        {
            public ushort Exe { get; set; }                      // decimal number 267 for 32 bit, and 523 for 64 bit.
            public byte lnMajVer { get; set; }                   // The version, in x.y format of the linker used to create the PE.
            public byte lnMnrVer { get; set; }
            public UInt32 SizeOfCode { get; set; }               // Size of the .text (.code) section
            public UInt32 SizeOfInitializedData { get; set; }    // Size of .data section
            public UInt32 SizeOfUninitializedData { get; set; }
            public UInt32 AddressOfEntryPoint { get; set; }
            public UInt32 BaseOfCode { get; set; }               // RVA of the .text section
            public UInt32 BaseOfData { get; set; }               // RVA of .data section

            public override string ToString()
            {
                var sb = new StringBuilder("Standard Fields\n");
                sb.Append(ToStringReflection(this));
                return sb.ToString();
            }
        }

        public class WINDOWSSPECIFICFIELDS
        {
            public UInt32 ImageBase { get; set; }   // Preferred location in memory for the module to be based at
            public UInt32 SectionAlignment { get; set; }
            public UInt32 FileAlignment { get; set; }
            public ushort MajorOSVersion { get; set; }
            public ushort MinorOSVersion { get; set; }
            public ushort MajorImageVersion { get; set; }
            public ushort MinorImageVersion { get; set; }
            public ushort MajorSubSystemVersion { get; set; }
            public ushort MinorSubSystemVersion { get; set; }
            public UInt32 Win32VersionValue { get; set; }
            public UInt32 SizeOfImage { get; set; }
            public UInt32 SizeOfHeaders { get; set; }
            public UInt32 Checksum { get; set; }        // Checksum of the file, only used to verify validity of modules being loaded into kernel space. The formula used to calculate PE file checksums is proprietary, although Microsoft provides API calls that can calculate the checksum for you.
            public ushort Subsystem { get; set; }       // The Windows subsystem that will be invoked to run the executable
            public ushort DllCharacteristics { get; set; }
            public UInt32 SizeOfStackReverse { get; set; }
            public UInt32 SizeOfStackCommit { get; set; }
            public UInt32 SizeOfHeapReverse { get; set; }
            public UInt32 SizeOfHeapCommit { get; set; }
            public UInt32 LoaderFlags { get; set; }
            public UInt32 NumberOfRVAandSizes { get; set; }

            /// <summary>
            /// Resolve the subsystem attribute to a human readable string.
            /// </summary>
            /// <param name="subsystem">Subsystem attribute.</param>
            /// <returns>Subsystem as readable string.</returns>
            public static string ResolveSubsystem(ushort subsystem)
            {
                var ss = "unknown";
                switch (subsystem)
                {
                    case 1:
                        ss = "native";
                        break;
                    case 2:
                        ss = "Windows/GUI";
                        break;
                    case 3:
                        ss = "Windows non-GUI";
                        break;
                    case 5:
                        ss = "OS/2";
                        break;
                    case 7:
                        ss = "POSIX";
                        break;
                    case 8:
                        ss = "Native Windows 9x Driver";
                        break;
                    case 9:
                        ss = "Windows CE";
                        break;
                    case 0xA:
                        ss = "EFI Application";
                        break;
                    case 0xB:
                        ss = "EFI boot service device";
                        break;
                    case 0xC:
                        ss = "EFI runtime driver";
                        break;
                    case 0xD:
                        ss = "EFI ROM";
                        break;
                    case 0xE:
                        ss = "XBox";
                        break;
                }
                return ss;
            }

            public override string ToString()
            {
                var sb = new StringBuilder("Windows Specific Fields\n");
                sb.Append(ToStringReflection(this));
                sb.AppendFormat(_tableFormat, "Resolved Subsystem", ResolveSubsystem(Subsystem));
                return sb.ToString();
            }
        }

        public class DATADIRECTORIES
        {
            public UInt32 edataOffset { get; set; }
            public UInt32 edataSize { get; set; }
            public UInt32 idataOffset { get; set; }
            public UInt32 idataSize { get; set; }
            public UInt32 rsrcOffset { get; set; }
            public UInt32 rsrcSize { get; set; }
            public UInt32 pdataOffset { get; set; }
            public UInt32 pdataSize { get; set; }
            public UInt32 AttributeCertificateOffsetImage { get; set; }
            public UInt32 AttributeCertificateSizeImage { get; set; }
            public UInt32 relocOffsetImage { get; set; }
            public UInt32 relocSizeImage { get; set; }
            public UInt32 debugOffset { get; set; }
            public UInt32 debugSize { get; set; }
            public UInt32 Architecture1 { get; set; }
            public UInt32 Architecture2 { get; set; }
            public UInt32 GlobalPtrOffset { get; set; }
            public UInt32 Zero { get; set; }
            public UInt32 tlsOffset { get; set; }
            public UInt32 tlsSize { get; set; }
            public UInt32 LoadConfigTableOffsetImage { get; set; }
            public UInt32 LoadConfigTableSizeImage { get; set; }
            public UInt32 BoundImportTableOffset { get; set; }
            public UInt32 BoundImportTableSize { get; set; }
            public UInt32 ImportAddressTableOffset { get; set; }
            public UInt32 ImportAddressTableSize { get; set; }
            public UInt32 DelayImportDescriptorOffsetImage { get; set; }
            public UInt32 DelayImportDescriptorSizeImage { get; set; }
            public UInt32 CLRRuntimeHeaderOffsetObject { get; set; }
            public UInt32 CLRRuntimeHeaderSizeObject { get; set; }
            public IMAGE_EXPORT_DIRECTORY ImageExportDirectory { get; set; }
            public ExportFunction[] ExportFunctions { get; set; }
            public IMAGE_IMPORT_DESCRIPTOR[] ImageImportDescriptors { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder("Data Directories\n");
                sb.Append(ToStringReflection(this));
                
                if(ExportFunctions != null)
                {
                    sb.Append("Export Functions\n");
                    foreach (var e in ExportFunctions)
                        sb.Append(e.ToString());
                }

                if(ImageImportDescriptors != null)
                {
                    sb.Append("Image Import Descriptors");
                    foreach (var i in ImageImportDescriptors)
                        sb.Append(i.ToString());
                }
                
                return sb.ToString();
            }

            /*
             * 2 * UInt32 reserved
             */

            public class IMAGE_EXPORT_DIRECTORY
            {
                public UInt32 Characteristics { get; set; }
                public UInt32 TimeDateStamp { get; set; }
                public ushort MajorVersion { get; set; }
                public ushort MinorVersion { get; set; }
                public UInt32 Name { get; set; }
                public UInt32 Base { get; set; }
                public UInt32 NumberOfFuncions { get; set; }
                public UInt32 NumberOfNames { get; set; }
                public UInt32 AddressOfFunctions { get; set; }
                public UInt32 AddressOfNames { get; set; }
                public UInt32 AddressOfNameOrdinals { get; set; }

                public override string ToString()
                {
                    var sb = new StringBuilder("IMAGE_EXPORT_DIRECTORY\n");
                    sb.Append(ToStringReflection(this));
                    return sb.ToString();
                }
            }

            public class ExportFunction
            {
                public UInt32 Address { get; private set; }
                public string Name { get; private set; }
                public ushort Ordinal { get; private set; }

                public ExportFunction(UInt32 address, string name, ushort ordinal)
                {
                    Address = address;
                    Name = name;
                    Ordinal = ordinal;
                }

                public override string ToString()
                {
                    var sb = new StringBuilder("Export Function\n");
                    sb.Append(ToStringReflection(this));
                    return sb.ToString();
                }
            }

            public class IMAGE_IMPORT_DESCRIPTOR
            {
                public UInt32 OriginalFirstThunk { get; set; }
                public UInt32 TimeDataStamp { get; set; }
                public UInt32 ForwarderChain { get; set; }
                public UInt32 Name { get; set; }
                public UInt32 FirstThunk { get; set; }

                public IMAGE_THUNK_DATA32 ImageThunkData32 { get; set; }
                public String NameResolved { get; set; }

                public override string ToString()
                {
                    var sb = new StringBuilder("IMAGE_IMPORT_DESCRIPTOR\n");
                    sb.Append(ToStringReflection(this));
                    return sb.ToString();
                }
            }

            public class IMAGE_THUNK_DATA32
            {
                private UInt32 _value;

                public UInt32 ForwarderString  // PBYTE
                { 
                    get { return _value; }
                    set { _value = value; }
                }

                public UInt32 Function  // PDWORD
                {
                    get { return _value; }
                    set { _value = value; }
                }

                public UInt32 Ordinal
                {
                    get { return _value; }
                    set { _value = value; }
                }

                public UInt32 AddressOfData  // PIMAGE_IMPORT_BY_NAME
                {
                    get { return _value; }
                    set { _value = value; }
                }

                public DATADIRECTORIES.IMAGE_IMPORT_BY_NAME[] ImageImportByName;

                public override string ToString()
                {
                    var sb = new StringBuilder("IMAGE_THUNK_DATA32\n");
                    sb.Append(ToStringReflection(this));

                    if(ImageImportByName != null)
                    {
                        sb.Append("Image Imports By Name\n");
                        foreach (var i in ImageImportByName)
                            sb.Append(i.ToString());
                    }
                    
                    return sb.ToString();
                }
            }

            public class IMAGE_IMPORT_BY_NAME
            {
                public UInt16 Hint { get; set; }
                public String Name { get; set; }

                public override string ToString()
                {
                    var sb = new StringBuilder("IMAGE_IMPORT_BY_NAME\n");
                    sb.Append(ToStringReflection(this));
                    return sb.ToString();
                }
            }
        }

        public class SECTIONTABLE
        {
            /// <summary>
            /// A section table can have multiple sections with each a size of 40 bytes
            /// </summary>
            public class SECTIONHEADER
            {
                public byte[] SectionHeaderName { get; set; } // 8 bytes
                public UInt32 VirtualSize { get; set; }
                public UInt32 VirutalAddress { get; set; }
                public UInt32 SizeOfRawData { get; set; }
                public UInt32 PointerToRawData { get; set; }
                public UInt32 PointerToRelocations { get; set; }
                public UInt32 PointerToLineNumbers { get; set; }
                public UInt32 NumberOfRelocations { get; set; }
                public UInt32 NumberofLineNumbers { get; set; }
                public UInt32 SectionFlags { get; set; }

                public override string ToString()
                {
                    var sb = new StringBuilder("Section Header\n");
                    sb.Append(ToStringReflection(this));
                    sb.Append("Resolved SectionFlags\n");
                    var sf = ResolveSectionFlags(SectionFlags);
                    foreach(var f in sf)
                    {
                        sb.AppendFormat(_tableFormat, "Flag", f.ToString());
                    }
                    return sb.ToString();
                }
            }

            public SECTIONHEADER[] SectionHeaders
            {
                get;
                set;
            }

            [Flags]
            public enum SectionFlags : uint
            {
                IMAGE_SCN_TYPE_NO_PAD               = 0x00000008,  // Reserved.
                IMAGE_SCN_CNT_CODE                  = 0x00000020,  // Section contains code.
                IMAGE_SCN_CNT_INITIALIZED_DATA      = 0x00000040,  // Section contains initialized data.
                IMAGE_SCN_CNT_UNINITIALIZED_DATA    = 0x00000080,  // Section contains uninitialized data.
                IMAGE_SCN_LNK_OTHER                 = 0x00000100,  // Reserved.
                IMAGE_SCN_LNK_INFO                  = 0x00000200,  // Section contains comments or some  other type of information.
                IMAGE_SCN_LNK_REMOVE                = 0x00000800,  // Section contents will not become part of image.
                IMAGE_SCN_LNK_COMDAT                = 0x00001000,  // Section contents comdat.
                IMAGE_SCN_NO_DEFER_SPEC_EXC         = 0x00004000,  // Reset speculative exceptions handling bits in the TLB entries for this section.
                IMAGE_SCN_GPREL                     = 0x00008000,  // Section content can be accessed relative to GP
                IMAGE_SCN_MEM_FARDATA               = 0x00008000,
                IMAGE_SCN_MEM_PURGEABLE             = 0x00020000,
                IMAGE_SCN_MEM_16BIT                 = 0x00020000,
                IMAGE_SCN_MEM_LOCKED                = 0x00040000,
                IMAGE_SCN_MEM_PRELOAD               = 0x00080000,
                IMAGE_SCN_ALIGN_1BYTES              = 0x00100000,  //
                IMAGE_SCN_ALIGN_2BYTES              = 0x00200000,  //
                IMAGE_SCN_ALIGN_4BYTES              = 0x00300000,  //
                IMAGE_SCN_ALIGN_8BYTES              = 0x00400000,  //
                IMAGE_SCN_ALIGN_16BYTES             = 0x00500000,  // Default alignment if no others are specified.
                IMAGE_SCN_ALIGN_32BYTES             = 0x00600000,  //
                IMAGE_SCN_ALIGN_64BYTES             = 0x00700000,  //
                IMAGE_SCN_ALIGN_128BYTES            = 0x00800000,  //
                IMAGE_SCN_ALIGN_256BYTES            = 0x00900000,  //
                IMAGE_SCN_ALIGN_512BYTES            = 0x00A00000,  //
                IMAGE_SCN_ALIGN_1024BYTES           = 0x00B00000,  //
                IMAGE_SCN_ALIGN_2048BYTES           = 0x00C00000,  //
                IMAGE_SCN_ALIGN_4096BYTES           = 0x00D00000,  //
                IMAGE_SCN_ALIGN_8192BYTES           = 0x00E00000,  //
                IMAGE_SCN_ALIGN_MASK                = 0x00F00000,
                IMAGE_SCN_LNK_NRELOC_OVFL           = 0x01000000,  // Section contains extended relocations.
                IMAGE_SCN_MEM_DISCARDABLE           = 0x02000000,  // Section can be discarded.
                IMAGE_SCN_MEM_NOT_CACHED            = 0x04000000,  // Section is not cache-able.
                IMAGE_SCN_MEM_NOT_PAGED             = 0x08000000,  // Section is not page-able.
                IMAGE_SCN_MEM_SHARED                = 0x10000000,  // Section is shareable.
                IMAGE_SCN_MEM_EXECUTE               = 0x20000000,  // Section is executable.
                IMAGE_SCN_MEM_READ                  = 0x40000000,  // Section is readable.
                IMAGE_SCN_MEM_WRITE                 = 0x80000000   // Section is write-able.
            }

            /// <summary>
            /// Resolves the section flags to human readable strings. 
            /// </summary>
            /// <param name="sectionFlags">Sections flags from the SectionHeader object.</param>
            /// <returns>List with flag names for the section.</returns>
            public static List<string> ResolveSectionFlags(UInt32 sectionFlags)
            {
                var st = new List<string>();
                foreach (var flag in (SectionFlags[])Enum.GetValues(typeof(SectionFlags)))
                {
                    if ((sectionFlags & (uint)flag) == (uint)flag)
                    {
                        st.Add(flag.ToString());
                    }
                }
                return st;
            }

            public override string ToString()
            {
                var sb = new StringBuilder("Section Table\n");
                sb.Append(ToStringReflection(this));
                if(SectionHeaders != null)
                {
                    foreach (var s in SectionHeaders)
                        sb.Append(s.ToString());
                }

                return sb.ToString();
            }
        }

        public MSDOSHEADER MSDOSHeader
        {
            get;
            private set;
        }

        public PESIGNATURE PESignature
        {
            get;
            private set;
        }

        public COFFHEADER COFFHeader
        {
            get;
            private set;
        }

        public STANDARDFIELDS StandardFields
        {
            get;
            private set;
        }

        public WINDOWSSPECIFICFIELDS WindowsSpecificFields
        {
            get;
            private set;
        }

        public DATADIRECTORIES DataDirectories
        {
            get;
            private set;
        }

        public SECTIONTABLE SectionTable
        {
            get;
            private set;
        }

        /// <summary>
        /// Checks if the file starts with the "MZ" header.
        /// </summary>
        /// <returns>True if file starts with "MZ", else false.</returns>
        private bool IsPeFile()
        {
            if (MSDOSHeader.MZ == 0x5a4d)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Parses a PE file given as a byte array.
        /// </summary>
        /// <param name="peFile">Byte array of a PE file.</param>
        public PEHeader(byte[] peFile)
        {
            MSDOSHeader = ParseMSDOSHEADER(peFile);
            if (!IsPeFile())
                throw new ArgumentException("File is not a valid PE file!");

            UInt32 offset           = MSDOSHeader.OffsetPEToSignature;
            PESignature             = ParsePESignature(peFile, offset);
            COFFHeader              = ParseCOFFHeader(peFile, offset + 4);
            StandardFields          = ParseStandardFields(peFile, offset + 24);
            WindowsSpecificFields   = ParseWindowsSpecificFields(peFile, offset + 52);
            DataDirectories         = ParseDataDirectories(peFile, offset + 120);
            SectionTable            = ParseSectionTable(peFile, COFFHeader.NumberOfSections, offset + 248);

            if (DataDirectories.edataOffset != 0)
            {
                try
                {
                    DataDirectories.ImageExportDirectory = ParseImageExportDirectory(peFile, DataDirectories, SectionTable);
                    DataDirectories.ExportFunctions = ParseExportFunctions(peFile, DataDirectories.ImageExportDirectory, SectionTable);
                }
                catch
                {
                    // Has no export directory, or invalid.
                }
            }

            if(DataDirectories.idataOffset != 0)
                DataDirectories.ImageImportDescriptors = ParseImageImportDescriptors(peFile, DataDirectories, SectionTable);         
        }

        /// <summary>
        /// Parses a PE file.
        /// </summary>
        /// <param name="peFile">Path to a PE file.</param>
        public PEHeader(string peFile)
            : this(System.IO.File.ReadAllBytes(peFile)) {}

        /// <summary>
        /// Returns if the PE file is a EXE, LIB and which architecture
        /// is used (32/64/IA).
        /// </summary>
        /// <returns>PE type and architecture.</returns>
        public String GetFileType()
        {
            string fileType;

            // first determine if it is an executable or a library
            if (StandardFields.Exe == 0x10B)
                fileType = "EXE";
            else if (StandardFields.Exe == 0x20B)
                fileType = "LIB";
            else
                fileType = "UNKNOWN";

            // than add the architecture
            if (COFFHeader.TargetMachine == 0x014C)
                fileType += "_PE32";
            else if (COFFHeader.TargetMachine == 0x8664)
                fileType += "_PE64";
            else if (COFFHeader.TargetMachine == 0x0200)
                fileType += "_PEIA64";
            else
                fileType += "_UNKNOWN";

            return fileType;
        }

        /// <summary>
        /// Mandiant’s imphash convention requires the following:
        /// Resolving ordinals to function names when they appear.
        /// Converting both DLL names and function names to all lowercase.
        /// Removing the file extensions from imported module names.
        /// Building and storing the lowercased strings in an ordered list.
        /// Generating the MD5 hash of the ordered list.
        /// 
        /// oleaut32, ws2_32 and wsock32 can resolve ordinals to functions names.
        /// The implementation is equal to the python module "pefile" 1.2.10-139
        /// https://code.google.com/p/pefile/
        /// </summary>
        /// <returns>The ImpHash of the PE file.</returns>
        public String GetImpHash()
        {
            if (DataDirectories.ImageImportDescriptors == null)
                return null;

            // Get all imported functions and add them to a list
            var imp = new List<string>();
            foreach (var iid in DataDirectories.ImageImportDescriptors)
            {
                if (iid.ImageThunkData32.ImageImportByName != null) // Could be an import only by ordinal
                {
                    foreach (var iibn in iid.ImageThunkData32.ImageImportByName)
                    {
                        var tmp = iid.NameResolved.Split('.')[0]; // the module name
                        tmp += ".";
                        tmp += iibn.Name;
                        tmp = tmp.ToLower();
                        imp.Add(tmp);
                    }
                }
                else
                {
                    var ordinal = iid.ImageThunkData32.Ordinal;
                    string name = null;
                    if (iid.NameResolved.ToLower() == "oleaut32.dll")
                    {
                        name = OrdinalSymbolMapping.Lookup(OrdinalSymbolMapping.Modul.oleaut32, ordinal);
                    }
                    else if (iid.NameResolved.ToLower() == "ws2_32.dll")
                    {
                        name = OrdinalSymbolMapping.Lookup(OrdinalSymbolMapping.Modul.ws2_32, ordinal);
                    }
                    else if (iid.NameResolved.ToLower() == "wsock32.dll")
                    {
                        name = OrdinalSymbolMapping.Lookup(OrdinalSymbolMapping.Modul.wsock32, ordinal);
                    }
                    else // cannot resolve ordinal to a function name
                    {
                        name = "ord";
                        name += ordinal.ToString();
                    }

                    if (name != null)
                    {
                        var tmp = iid.NameResolved.Split('.')[0]; // the module name
                        tmp += ".";
                        tmp += name;
                        tmp = tmp.ToLower();
                        imp.Add(tmp);
                    }
                }
            }

            // Concatenate all imports to one string.
            var imports = "";
            var ib = new StringBuilder();
            for (int i = 0; i < imp.Count; i++)
            {
                if (i < imp.Count - 1)
                {
                    ib.Append(imp[i]);
                    ib.Append(",");
                }
                else
                    ib.Append(imp[i]);
            }
            imports = ib.ToString();

            var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(imports);
            var hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Parse the IMAGE_IMPORT_DESCRIPTOR structures.
        /// http://resources.infosecinstitute.com/the-import-directory-part-1/
        /// </summary>
        /// <param name="buff">The PE file as an byte array.</param>
        /// <param name="dd">The DATADIRECTORY of the PE file.</param>
        /// <param name="st">The SECTIONTABLE of the PE file.</param>
        /// <returns></returns>
        private DATADIRECTORIES.IMAGE_IMPORT_DESCRIPTOR[] ParseImageImportDescriptors(byte[] buff, DATADIRECTORIES dd, SECTIONTABLE st)
        {
            var list = new List<DATADIRECTORIES.IMAGE_IMPORT_DESCRIPTOR>();
            var idataOffset = dd.idataOffset;
            var fileMappedidataOffset = RVAtoFileMapping(idataOffset, st);
            UInt32 sizeOfIMAGE_IMPORT_DESCRIPTOR = 20; // IMAGE_IMPORT_DESCRIPTOR is 20 bytes long
            var num = (dd.idataSize / sizeOfIMAGE_IMPORT_DESCRIPTOR) - 1; // Unsure why -1 one but kernel32.dll has a idataSize of 500 but only 24 IMAGE_IMPORT_DESCRIPTORS and not 25  (500/20 = 25) 

            for (UInt32 i = 0; i < num; i++)
            {
                var orgft   = BytesToUInt32(buff, fileMappedidataOffset + i * sizeOfIMAGE_IMPORT_DESCRIPTOR);
                var tds     = BytesToUInt32(buff, fileMappedidataOffset + sizeof(UInt32) + i * sizeOfIMAGE_IMPORT_DESCRIPTOR);
                var fc      = BytesToUInt32(buff, fileMappedidataOffset + 2 * sizeof(UInt32) + i * sizeOfIMAGE_IMPORT_DESCRIPTOR);
                var n       = BytesToUInt32(buff, fileMappedidataOffset + 3 * sizeof(UInt32) + i * sizeOfIMAGE_IMPORT_DESCRIPTOR);
                var ft      = BytesToUInt32(buff, fileMappedidataOffset + 4 * sizeof(UInt32) + i * sizeOfIMAGE_IMPORT_DESCRIPTOR);
                var nr      = "";

                try
                {
                    // If no name can be resolved, break
                    nr = GetName(RVAtoFileMapping(BytesToUInt32(buff, fileMappedidataOffset + 3 * sizeof(UInt32) + i * sizeOfIMAGE_IMPORT_DESCRIPTOR), st), buff);
                }
                catch
                {
                    break;
                }
                list.Add(new DATADIRECTORIES.IMAGE_IMPORT_DESCRIPTOR()
                {
                    OriginalFirstThunk = orgft,
                    TimeDataStamp = tds,
                    ForwarderChain = fc,
                    Name = n,
                    FirstThunk = ft,
                    // Resolve the name of the module
                    NameResolved = nr
                });
            }
            list.OrderBy(n => (n.OriginalFirstThunk != 0) ? n.OriginalFirstThunk : n.FirstThunk);
            // Parse the IMAGE_THUNK_DATA32 elements of each IMAGE_IMPORT_DESCRIPTOR.
            // The  OriginalFirstThunk contains the RVA to the IMAGE_THUNK_DATA32 of each IMAGE_IMPORT_DESCRIPTOR
            for (int i = 0; i < list.Count; i++)
            {
                if (i < list.Count - 1)
                {
                    UInt32 nextItdAddress = 0;
                    for (int j = i; j < list.Count - 1; j++)
                    {
                        nextItdAddress = (list[j + 1].OriginalFirstThunk != 0) ? list[j + 1].OriginalFirstThunk : list[j + 1].FirstThunk;
                        nextItdAddress = BytesToUInt32(buff, RVAtoFileMapping(nextItdAddress, st));
                        if ((nextItdAddress & 0x80000000) != 0x80000000)
                        {
                            break;
                        }
                        nextItdAddress = 0;
                    }

                    list[i].ImageThunkData32 = ParseImageThunkData32(list[i], nextItdAddress, buff, st);
                }
                else
                    list[i].ImageThunkData32 = ParseImageThunkData32(list[i], 0, buff, st);
            }

            return list.ToArray();
        }

        private DATADIRECTORIES.IMAGE_THUNK_DATA32 ParseImageThunkData32(DATADIRECTORIES.IMAGE_IMPORT_DESCRIPTOR iid, UInt32 nextItdAddress, byte[] buff, SECTIONTABLE st)
        {
            var itd32 = new DATADIRECTORIES.IMAGE_THUNK_DATA32();
            bool mode2 = false;
            if (iid.OriginalFirstThunk != 0)
            {
                itd32.Ordinal = BytesToUInt32(buff, RVAtoFileMapping(iid.OriginalFirstThunk, st));
            }
            else
            {
                itd32.Ordinal = BytesToUInt32(buff, RVAtoFileMapping(iid.FirstThunk, st));
                mode2 = true;
            }

            // Check if import by name or by ordinal.
            // If it is an import by ordinal, the most significant bit of "Ordinal" is "1" and the ordinal can
            // be extracted from the least significant bits.
            // Else it is an import by name and the link to the IMAGE_IMPORT_BY_NAME has to be followed
            if ((itd32.Ordinal & 0x80000000) == 0x80000000)
            {
                itd32.Ordinal = (itd32.Ordinal & 0x7FFFFFFF);
            }
            else
            {
                var ordinal = RVAtoFileMapping(itd32.Ordinal, st);
                itd32.ImageImportByName = ParseImageImportByName(itd32, nextItdAddress, mode2, buff, st);
            }

            return itd32;
        }

        private DATADIRECTORIES.IMAGE_IMPORT_BY_NAME[] ParseImageImportByName(DATADIRECTORIES.IMAGE_THUNK_DATA32 itd32, UInt32 nextItdAddress, bool mode2, byte[] buff, SECTIONTABLE st)
        {
            var iibns = new List<DATADIRECTORIES.IMAGE_IMPORT_BY_NAME>();
            UInt32 address = RVAtoFileMapping(itd32.AddressOfData, st);

            if (nextItdAddress != 0)
            {
                nextItdAddress = RVAtoFileMapping(nextItdAddress, st);
                // Runs until the address of the next IMAGE_IMPORT_BY_NAME structure is reached.
                while (address + 1 < nextItdAddress)
                {
                    var name = GetName(address + 2, buff);
                    if (name == "" || name[0] < 0x41 || name[0] > 0x7a)
                    {
                        address += 1;
                        continue;
                    }
                    var length = GetNameLength(address + 2, buff);
                    ushort hint = BytesToUshort(buff, address);

                    if (!mode2)
                    {
                        address += (UInt32)(2 + length + 1); // 2 byte hint, length of name, one terminating null byte
                    }
                    else
                    {
                        address += (UInt32)length + 2;
                        for (int i = 0; i < 4; i++)
                        {
                            if (buff[address] != 0)
                            {
                                address -= 2;
                                break;
                            }
                            address++;
                            if (i == 3)
                                break;
                        }
                    }
                    iibns.Add(new DATADIRECTORIES.IMAGE_IMPORT_BY_NAME() { Hint = hint, Name = name });
                }
            }
            else
            {
                int counter = 0;
                while (counter <= 3)
                {
                    var name = GetName(address + 2, buff);
                    if (name == "" || name[0] < 0x41 || name[0] > 0x7a)
                    {
                        address += 1;
                        counter++;
                        continue;
                    }
                    counter = 0;
                    var length = GetNameLength(address + 2, buff);
                    ushort hint = BytesToUshort(buff, address);

                    address += (UInt32)(2 + length + 1); // 2 byte hint, length of name, one terminating null byte
                    iibns.Add(new DATADIRECTORIES.IMAGE_IMPORT_BY_NAME() { Hint = hint, Name = name });
                }
            }

            return iibns.ToArray();
        }

        private MSDOSHEADER ParseMSDOSHEADER(byte[] buff)
        {
            var header = new MSDOSHEADER();
            header.MZ = BytesToUshort(buff, 0);
            header.lastSize                 = BytesToUshort(buff, 2);
            header.PageInFiles              = BytesToUshort(buff, 4);
            header.relocations              = BytesToUshort(buff, 6);
            header.headerSizeInParagraph    = BytesToUshort(buff, 8);
            header.MinExtraParagraphNeeded  = BytesToUshort(buff, 0xA);
            header.MaxExtraParagraphNeeded  = BytesToUshort(buff, 0xC);
            header.InitialRelativeSS        = BytesToUshort(buff, 0xE);
            header.InitialRelativeSP        = BytesToUshort(buff, 0x10);
            header.checksum                 = BytesToUshort(buff, 0x12);
            header.InitialIP                = BytesToUshort(buff, 0x14);
            header.InitialRelativeCS        = BytesToUshort(buff, 0x16);
            header.FileAddOfRelocTable      = BytesToUshort(buff, 0x18);
            header.OverlayNumber            = BytesToUshort(buff, 0x1A);
            header.OEMIdentifier            = BytesToUshort(buff, 0x24);
            header.OEMInformation           = BytesToUshort(buff, 0x26);
            header.OffsetPEToSignature      = BytesToUInt32(buff, 0x3C);
            return header;
        }

        private PESIGNATURE ParsePESignature(byte[] buff, UInt32 offset)
        {
            var pesig = new PESIGNATURE();
            pesig.PESignature = BytesToUInt32(buff, offset);
            return pesig;
        }

        private COFFHEADER ParseCOFFHeader(byte[] buff, UInt32 offset)
        {
            var coffHeader                      = new COFFHEADER();
            coffHeader.TargetMachine            = BytesToUshort(buff, offset);
            coffHeader.NumberOfSections         = BytesToUshort(buff, offset + 2);
            coffHeader.TimeDateStamp            = BytesToUInt32(buff, offset + 4);
            coffHeader.PointerToSymbolTable     = BytesToUInt32(buff, offset + 8);
            coffHeader.NumberOfSymbols          = BytesToUInt32(buff, offset + 12);
            coffHeader.SizeOfOptionalHeaders    = BytesToUshort(buff, offset + 16);
            coffHeader.Characteristics          = BytesToUshort(buff, offset + 18);
            return coffHeader;
        }

        private STANDARDFIELDS ParseStandardFields(byte[] buff, UInt32 offset)
        {
            var sfields                     = new STANDARDFIELDS();
            sfields.Exe                     = BytesToUshort(buff, offset);
            sfields.lnMajVer                = buff[offset + 2];
            sfields.lnMnrVer                = buff[offset + 3];
            sfields.SizeOfCode              = BytesToUInt32(buff, offset + 4);
            sfields.SizeOfInitializedData   = BytesToUInt32(buff, offset + 8);
            sfields.SizeOfUninitializedData = BytesToUInt32(buff, offset + 12);
            sfields.AddressOfEntryPoint     = BytesToUInt32(buff, offset + 16);
            sfields.BaseOfCode              = BytesToUInt32(buff, offset + 20);
            sfields.BaseOfData              = BytesToUInt32(buff, offset + 24);
            return sfields;
        }

        private WINDOWSSPECIFICFIELDS ParseWindowsSpecificFields(byte[] buff, UInt32 offset)
        {
            var wsf                     = new WINDOWSSPECIFICFIELDS();
            wsf.ImageBase               = BytesToUInt32(buff, offset);
            wsf.SectionAlignment        = BytesToUInt32(buff, offset + 4);
            wsf.FileAlignment           = BytesToUInt32(buff, offset + 8);
            wsf.MajorOSVersion          = BytesToUshort(buff, offset + 12);
            wsf.MinorOSVersion          = BytesToUshort(buff, offset + 14);
            wsf.MajorImageVersion       = BytesToUshort(buff, offset + 16);
            wsf.MinorImageVersion       = BytesToUshort(buff, offset + 18);
            wsf.MajorSubSystemVersion   = BytesToUshort(buff, offset + 20);
            wsf.MinorImageVersion       = BytesToUshort(buff, offset + 22);
            wsf.Win32VersionValue       = BytesToUInt32(buff, offset + 24);
            wsf.SizeOfImage             = BytesToUInt32(buff, offset + 28);
            wsf.SizeOfHeaders           = BytesToUInt32(buff, offset + 32);
            wsf.Checksum                = BytesToUInt32(buff, offset + 36);
            wsf.Subsystem               = BytesToUshort(buff, offset + 40);
            wsf.DllCharacteristics      = BytesToUshort(buff, offset + 42);
            wsf.SizeOfStackReverse      = BytesToUInt32(buff, offset + 44);
            wsf.SizeOfStackCommit       = BytesToUInt32(buff, offset + 48);
            wsf.SizeOfHeapReverse       = BytesToUInt32(buff, offset + 52);
            wsf.SizeOfHeapCommit        = BytesToUInt32(buff, offset + 56);
            wsf.LoaderFlags             = BytesToUInt32(buff, offset + 60);
            wsf.NumberOfRVAandSizes     = BytesToUInt32(buff, offset + 64);
            return wsf;
        }

        private DATADIRECTORIES ParseDataDirectories(byte[] buff, UInt32 offset)
        {
            var dd = new DATADIRECTORIES();
            dd.edataOffset                      = BytesToUInt32(buff, offset);      // Offset to the ExportTable
            dd.edataSize                        = BytesToUInt32(buff, offset + 4);  // Size of the ExportTable
            dd.idataOffset                      = BytesToUInt32(buff, offset + 8);  // Offset to the ImportTable
            dd.idataSize                        = BytesToUInt32(buff, offset + 12); // Size of the ImportTable
            dd.rsrcOffset                       = BytesToUInt32(buff, offset + 16);
            dd.rsrcSize                         = BytesToUInt32(buff, offset + 20);
            dd.pdataOffset                      = BytesToUInt32(buff, offset + 24);
            dd.pdataSize                        = BytesToUInt32(buff, offset + 28);
            dd.AttributeCertificateOffsetImage  = BytesToUInt32(buff, offset + 32);
            dd.AttributeCertificateSizeImage    = BytesToUInt32(buff, offset + 36);
            dd.relocOffsetImage                 = BytesToUInt32(buff, offset + 40);
            dd.relocSizeImage                   = BytesToUInt32(buff, offset + 44);
            dd.debugOffset                      = BytesToUInt32(buff, offset + 48);
            dd.debugSize                        = BytesToUInt32(buff, offset + 52);
            dd.Architecture1                    = BytesToUInt32(buff, offset + 56);
            dd.Architecture2                    = BytesToUInt32(buff, offset + 60);
            dd.GlobalPtrOffset                  = BytesToUInt32(buff, offset + 64);
            dd.Zero                             = BytesToUInt32(buff, offset + 68); // Always 0x00
            dd.tlsOffset                        = BytesToUInt32(buff, offset + 72);
            dd.tlsSize                          = BytesToUInt32(buff, offset + 76);
            dd.LoadConfigTableOffsetImage       = BytesToUInt32(buff, offset + 80);
            dd.LoadConfigTableSizeImage         = BytesToUInt32(buff, offset + 84);
            dd.BoundImportTableOffset           = BytesToUInt32(buff, offset + 88);
            dd.BoundImportTableSize             = BytesToUInt32(buff, offset + 92);
            dd.ImportAddressTableOffset         = BytesToUInt32(buff, offset + 96);
            dd.ImportAddressTableSize           = BytesToUInt32(buff, offset + 100);
            dd.DelayImportDescriptorOffsetImage = BytesToUInt32(buff, offset + 104);
            dd.DelayImportDescriptorSizeImage   = BytesToUInt32(buff, offset + 108);
            dd.CLRRuntimeHeaderOffsetObject     = BytesToUInt32(buff, offset + 112);
            dd.CLRRuntimeHeaderSizeObject       = BytesToUInt32(buff, offset + 116);
            return dd;
        }

        private DATADIRECTORIES.IMAGE_EXPORT_DIRECTORY ParseImageExportDirectory(byte[] buff, DATADIRECTORIES dd, SECTIONTABLE st)
        {
            // Test if an export directory exists.
            if (dd.edataOffset == 0)
            {
                return null;
            }
            var et = new DATADIRECTORIES.IMAGE_EXPORT_DIRECTORY();
            var offset                  = RVAtoFileMapping(dd.edataOffset, st);
            et.Characteristics          = BytesToUInt32(buff, offset);
            et.TimeDateStamp            = BytesToUInt32(buff, offset + 4);
            et.MajorVersion             = BytesToUshort(buff, offset + 8);
            et.MinorVersion             = BytesToUshort(buff, offset + 0x0A);
            et.Name                     = BytesToUInt32(buff, offset + 0x0C);
            et.Base                     = BytesToUInt32(buff, offset + 0x10);
            et.NumberOfFuncions         = BytesToUInt32(buff, offset + 0x14);
            et.NumberOfNames            = BytesToUInt32(buff, offset + 0x18);
            et.AddressOfFunctions       = BytesToUInt32(buff, offset + 0x1C);
            et.AddressOfNames           = BytesToUInt32(buff, offset + 0x20);
            et.AddressOfNameOrdinals    = BytesToUInt32(buff, offset + 0x24);

            return et;
        }
        /// <summary>
        /// Create an array with all exported functions with a name. Functions without a name
        /// are not listed here.
        /// </summary>
        /// <param name="buff">PE file as binary buffer.</param>
        /// <param name="et">The IMAGE_EXPORT_DIRECTORY of the PE file.</param>
        /// <param name="st">The SECTIONTABLE of the PE File.</param>
        /// <returns>List with exported (by name) functions) of the PE file.</returns>
        private DATADIRECTORIES.ExportFunction[] ParseExportFunctions(byte[] buff, DATADIRECTORIES.IMAGE_EXPORT_DIRECTORY et, SECTIONTABLE st)
        {
            var ef = new DATADIRECTORIES.ExportFunction[et.NumberOfNames];
            var funcOffsetPointer   = RVAtoFileMapping(et.AddressOfFunctions, st);
            var ordOffset           = RVAtoFileMapping(et.AddressOfNameOrdinals, st);
            var nameOffsetPointer   = RVAtoFileMapping(et.AddressOfNames, st);

            var funcOffset          = BytesToUInt32(buff, funcOffsetPointer);

            for (UInt32 i = 0; i < ef.Length; i++)
            {
                var name            = GetName(RVAtoFileMapping(BytesToUInt32(buff, nameOffsetPointer + sizeof(UInt32) * i), st), buff);
                var ordinalIndex    = (UInt32)GetOrdinal(ordOffset + sizeof(ushort) * i, buff);
                var ordinal         = ordinalIndex + et.Base;
                var address         = BytesToUInt32(buff, funcOffsetPointer + sizeof(UInt32) * ordinalIndex);

                ef[i] = new DATADIRECTORIES.ExportFunction(address, name, (ushort)ordinal);
            }

            return ef;
        }

        private ushort GetOrdinal(UInt32 ordinal, byte[] buff)
        {
            return BitConverter.ToUInt16(new byte[2] { buff[ordinal], buff[ordinal + 1] }, 0);
        }

        private string GetName(UInt32 name, byte[] buff)
        {
            var length = GetNameLength(name, buff);
            var tmp = new char[length];
            for (int i = 0; i < length; i++)
            {
                tmp[i] = (char)buff[name + i];
            }

            return new string(tmp);
        }

        private int GetNameLength(UInt32 name, byte[] buff)
        {
            var offset = name;
            int length = 0;
            while (buff[offset] != 0x00)
            {
                length++;
                offset++;
            }
            return length;
        }

        /// <summary>
        /// Resolve an RVA to an file mapped address.
        /// This function is only for external use because it
        /// returns 0 if something fails and the RVA if the RVA
        /// is smaller than the smallest section voffset. This is
        /// needed for packers who are hiding their entry point like
        /// the "looper" malware does.
        /// </summary>
        /// <param name="RVA">RVA that should be resolved to a file mapped offset.</param>
        /// <returns>The file mapped offset for the RVA.</returns>
        public UInt32 RVAtoFileMapping(UInt32 RVA)
        {
            UInt32 add = 0;
            try
            {
                add = RVAtoFileMapping(RVA, this.SectionTable);
            }
            catch
            {
                add = 0;
            }

            // For the case that a packer hides the entry point in the first section
            // The malware Looper does this for example...
            if (RVA < this.SectionTable.SectionHeaders[0].VirutalAddress)
            {
                add = RVA;
            }

            return add;
        }

        private UInt32 RVAtoFileMapping(UInt32 RVA, SECTIONTABLE st)
        {
            var sortedSt = st.SectionHeaders.OrderBy(sh => sh.VirutalAddress).ToList();
            UInt32 vOffset = 0, rOffset = 0;
            bool secFound = false;
            for (int i = 0; i < sortedSt.Count - 1; i++)
            {
                if (sortedSt[i].VirutalAddress <= RVA && sortedSt[i + 1].VirutalAddress > RVA)
                {
                    vOffset = sortedSt[i].VirutalAddress;
                    rOffset = sortedSt[i].PointerToRawData;
                    secFound = true;
                    break;
                }
            }

            // try last section
            if (secFound == false)
            {
                if (RVA >= sortedSt.Last().VirutalAddress && RVA <= sortedSt.Last().VirtualSize + sortedSt.Last().VirutalAddress)
                {
                    vOffset = sortedSt.Last().VirutalAddress;
                    rOffset = sortedSt.Last().PointerToRawData;
                }
                else
                {
                    throw new Exception("Cannot find corresponding section.");
                }
            }

            return (RVA - vOffset) + rOffset;
        }

        private SECTIONTABLE ParseSectionTable(byte[] buff, ushort numberOfSections, UInt32 offset)
        {
            var st = new SECTIONTABLE();
            st.SectionHeaders = new SECTIONTABLE.SECTIONHEADER[numberOfSections];
            UInt32 size = 0x28; // Every section header is 40 bytes
            for (UInt32 i = 0; i < numberOfSections; i++)
            {
                st.SectionHeaders[i] = new SECTIONTABLE.SECTIONHEADER();
                st.SectionHeaders[i].SectionHeaderName      = new byte[8];
                Array.Copy(buff, (UInt32)offset + i * size, st.SectionHeaders[i].SectionHeaderName, (UInt32)0, (UInt32)8);
                st.SectionHeaders[i].VirtualSize            = BytesToUInt32(buff, offset + i * size + 8);
                st.SectionHeaders[i].VirutalAddress         = BytesToUInt32(buff, offset + i * size + 12);
                st.SectionHeaders[i].SizeOfRawData          = BytesToUInt32(buff, offset + i * size + 16);
                st.SectionHeaders[i].PointerToRawData       = BytesToUInt32(buff, offset + i * size + 20);
                st.SectionHeaders[i].PointerToRelocations   = BytesToUInt32(buff, offset + i * size + 24);
                st.SectionHeaders[i].PointerToLineNumbers   = BytesToUInt32(buff, offset + i * size + 30);
                st.SectionHeaders[i].NumberOfRelocations    = BytesToUshort(buff, offset + i * size + 32);
                st.SectionHeaders[i].NumberofLineNumbers    = BytesToUshort(buff, offset + i * size + 34);
                st.SectionHeaders[i].SectionFlags           = BytesToUInt32(buff, offset + i * size + 38);
            }

            return st;
        }

        private ushort BytesToUshort(byte b1, byte b2)
        {
            return BitConverter.ToUInt16(new byte[2] { b1, b2 }, 0);
        }

        private ushort BytesToUshort(byte[] buff, UInt32 i)
        {
            return BytesToUshort(buff[i], buff[i + 1]);
        }

        private UInt32 BytesToUInt32(byte b1, byte b2, byte b3, byte b4)
        {
            return BitConverter.ToUInt32(new byte[4] { b1, b2, b3, b4 }, 0);
        }

        private UInt32 BytesToUInt32(byte[] buff, UInt32 i)
        {
            return BytesToUInt32(buff[i], buff[i + 1], buff[i + 2], buff[i + 3]);
        }

        /// <summary>
        /// Tries to parse the PE file. If no exceptions are thrown, true
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsValidPEFile(string file)
        {
            var ret = true;
            try
            {
                var peHeader = new PeParser.PEHeader(file);
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }
    }
}
