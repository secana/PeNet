namespace PeNet.Test.Structures
{
    public static class RawStructures
    {
        public static byte[] RawImageLoadConfigDirectory64 =
        {
            0xff, // Junk
            0xff,

            0x00, // Size
            0x11,
            0x22,
            0x33,

            0x44, // TimeDateStamp
            0x55,
            0x66,
            0x77,

            0x88, // MajorVersion
            0x99,

            0xaa, // MinorVersion
            0xbb,

            0xcc, // GlobalFlagsClear
            0xdd,
            0xee,
            0xff,

            0x11, // GlobalFlagsSet
            0x22,
            0x33,
            0x44,

            0x55, // CriticalSectionDefaultTimeout
            0x66,
            0x77,
            0x88,

            0x99, // DeCommitFreeBlockThreshold
            0xaa,
            0xbb,
            0xcc,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0xdd, // DeCommitTotalFreeThreshold
            0xee,
            0xff,
            0x00,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0x22, // LockPrefixTable
            0x33,
            0x44,
            0x55,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0x66, // MaximumAllocationSize
            0x77,
            0x88,
            0x99,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0xaa, // VirtualMemoryThreshold
            0xbb,
            0xcc,
            0xdd,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0x33, // ProcessAffinityMask
            0x44,
            0x55,
            0x66,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0xff, // ProcessHeapFlags
            0x00,
            0x11,
            0x22,
            
            0x77, // CSDVersion
            0x88,

            0x99, // Reserved1
            0xaa,

            0xbb, // EditList
            0xcc,
            0xdd,
            0xff,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0x44, // SecurityCookie
            0x55,
            0x66,
            0x77,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0x88, // SEHandlerTable
            0x99,
            0xaa,
            0xbb,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0xcc, // SEHandlerCount
            0xdd,
            0xee,
            0xff,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0x55, // GuardCFCheckFunctionPointer
            0x66,
            0x77,
            0x88,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0x99, // Reserved2
            0xaa,
            0xbb,
            0xcc,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0xdd, // GuardCFFunctionTable
            0xee,
            0xff,
            0x00,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0x66, // GuardCFFunctionCount
            0x77,
            0x88,
            0x99,
            0xaa,
            0xbb,
            0xcc,
            0xdd,

            0xaa, // GuardFlags
            0xbb,
            0xcc,
            0xdd,
            0xaa,
            0xbb,
            0xcc,
            0xdd
        };

        public static byte[] RawImageLoadConfigDirectory32 =
        { 
            0xff, // Junk
            0xff,

            0x00, // Size
            0x11,
            0x22,
            0x33,
            
            0x44, // TimeDateStamp
            0x55,
            0x66,
            0x77,

            0x88, // MajorVersion
            0x99,

            0xaa, // MinorVersion
            0xbb,

            0xcc, // GlobalFlagsClear
            0xdd,
            0xee,
            0xff,

            0x11, // GlobalFlagsSet
            0x22,
            0x33,
            0x44,

            0x55, // CriticalSectionDefaultTimeout
            0x66,
            0x77,
            0x88,

            0x99, // DeCommitFreeBlockThreshold
            0xaa,
            0xbb,
            0xcc,

            0xdd, // DeCommitTotalFreeThreshold
            0xee,
            0xff,
            0x00,

            0x22, // LockPrefixTable
            0x33,
            0x44,
            0x55,
            
            0x66, // MaximumAllocationSize
            0x77,
            0x88,
            0x99,

            0xaa, // VirtualMemoryThreshold
            0xbb,
            0xcc,
            0xdd,

            0xff, // ProcessHeapFlags
            0x00,
            0x11,
            0x22,

            0x33, // ProcessAffinityMas
            0x44,
            0x55,
            0x66,

            0x77, // CSDVersion
            0x88,

            0x99, // Reserved1
            0xaa,

            0xbb, // EditList
            0xcc,
            0xdd,
            0xff,

            0x44, // SecurityCookie
            0x55,
            0x66,
            0x77,

            0x88, // SEHandlerTable
            0x99,
            0xaa,
            0xbb,

            0xcc, // SEHandlerCount
            0xdd,
            0xee,
            0xff,

            0x55, // GuardCFCheckFunctionPointer
            0x66,
            0x77,
            0x88,
            
            0x99, // Reserved2
            0xaa,
            0xbb,
            0xcc,

            0xdd, // GuardCFFunctionTable
            0xee,
            0xff,
            0x00,

            0x66, // GuardCFFunctionCount
            0x77, 
            0x88,
            0x99,

            0xaa, // GuardFlags
            0xbb,
            0xcc,
            0xdd
        };

        public static readonly byte[] RawDelayImportDescriptor =
        {
            0xff, // Junk
            0xff,

            0x00, // grAttr
            0x11,
            0x22,
            0x33,

            0x44, // szName
            0x55,
            0x66,
            0x77,

            0x88, // phmod
            0x99,
            0xaa,
            0xbb,

            0xcc, // pIAT
            0xdd,
            0xee,
            0xff,

            0x11, // pINT
            0x22,
            0x33,
            0x44,

            0x55, // pBoundIAT
            0x66,
            0x77,
            0x88,

            0x99, // pUnloadIAT
            0xaa,
            0xbb,
            0xcc,

            0xdd, // dwTimeStamp
            0xee,
            0xff,
            0x00
        };

        public static readonly byte[] RawBoundImportDescriptor =
        {
            0xff, // Junk
            0xff,

            0x00, // TimeDataStamp
            0x11,
            0x22,
            0x33,

            0x44, // OffsetModuleName
            0x55,
            
            0x66, // NumberOfModuleForwarderRefs
            0x77
        };

        public static readonly byte[] RawTlsCallback32 =
        {
            0xff, // Junk
            0xff,

            0x00, // Callback
            0x11,
            0x22,
            0x33,
        };

        public static readonly byte[] RawTlsCallback64 =
        {
            0xff, // Junk
            0xff,

            0x00, // Callback
            0x11,
            0x22,
            0x33,
            0x44,
            0x55,
            0x66,
            0x77
        };

        public static readonly byte[] RawTlsDirectory32 =
        {
            0xff, // Junk
            0xff,

            0x00, // StartAddressOfRawData
            0x11,
            0x22,
            0x33,

            0x44, // EndAddressOfRawData
            0x55,
            0x66,
            0x77,

            0xbb, // AddressOfIndex
            0xcc,
            0xdd,
            0xee,

            0x33, // AddressOfCallBacks
            0x44,
            0x55,
            0x66,

            0x11, // SizeOfZeroFill,
            0x22,
            0x33,
            0x44,

            0x66, // Characteristics
            0x77,
            0x88,
            0x99
        };

        public static readonly byte[] RawTlsDirectory64 =
        {
            0xff, // Junk
            0xff,

            0x00, // StartAddressOfRawData
            0x11,
            0x22,
            0x33,
            0x44,
            0x55,
            0x66,
            0x77,

            0x44, // EndAddressOfRawData
            0x55,
            0x66,
            0x77,
            0x88,
            0x99,
            0xaa,
            0xbb,

            0xbb, // AddressOfIndex
            0xcc,
            0xdd,
            0xee,
            0xff,
            0x00,
            0x11,
            0x22,
            
            0x33, // AddressOfCallBacks
            0x44,
            0x55,
            0x66,
            0x77,
            0x88,
            0x99,
            0xaa,
            
            0x11, // SizeOfZeroFill,
            0x22,
            0x33,
            0x44,
            
            0x66, // Characteristics
            0x77,
            0x88,
            0x99
        };

        public static readonly byte[] RawCopyright =
        {
            0xff, // Junk
            0xff,
            99, 111, 112, 121, 114, 105, 103, 104, 116 // "copyright"
        };

        public static readonly byte[] RawWinCertificate =
        {
            0xff, // Junk
            0xff,
            0x0b, // dwLenth
            0x00,
            0x00,
            0x00,
            0x44, // wRevision
            0x55,
            0x66, // wCertificateType
            0x77,

            // bCertificate (dwLength - 8 = 3)
            0x11,
            0x22,
            0x33
        };

        public static readonly byte[] RawUnwindInfo =
        {
            0xff, // Junk
            0xff,
            0x32, // Version/Flags 3:5

            0x33, // SizeOfProlog

            0x01, // CountOfCodes

            0x56, // FrameRegister/Offset 4:4

            // UNWIND_CODE
            0x77, // CodeOffset

            0x89, // UnwindOp/Opinfo 4:4

            0xaa, // FrameOffset
            0xbb,
            //////
            
            0xcc, // ExceptionHandler/FunctionEntry
            0xdd,
            0xee,
            0xff

            // Exception data
            // ??
        };

        public static readonly byte[] RawUnwindCode =
        {
            0xff, // Junk
            0xff,
            0x11, // CodeOffset
            0x23, // UnwindOp/Opinfo 4:4

            0x44, // FrameOffset
            0x55
        };

        public static readonly byte[] RawRuntimeFunction =
        {
            0xff, // Junk
            0xff,
            0x00, // FunctionStart
            0x11,
            0x22,
            0x33,
            0x44, // FunctionEnd
            0x55,
            0x66,
            0x77,
            0x88, // UnwindInfo
            0x99,
            0xaa,
            0xbb
        };

        public static readonly byte[] RawThunkData32 =
        {
            0xff, // Junk
            0xff,
            0x00, // AddressOfData
            0x11,
            0x22,
            0x33
        };

        public static readonly byte[] RawThunkData64 =
        {
            0xff, // Junk
            0xff,
            0x00, // AddressOfData
            0x11,
            0x22,
            0x33,
            0x44,
            0x55,
            0x66,
            0x77
        };

        public static readonly byte[] RawSectionHeader =
        {
            0xff, // Junk
            0xff,

            // Name [8]
            46, // .
            100, // d
            97, // a
            116, // t
            97, // a
            00,
            00,
            00,
            0x00, // VirtualSize
            0x11,
            0x22,
            0x33,
            0x44, // VirtualAddress
            0x55,
            0x66,
            0x77,
            0x88, // SizeOfRawData
            0x99,
            0xaa,
            0xbb,
            0xcc, // PointerToRawData
            0xdd,
            0xee,
            0xff,
            0x11, // PointerToRelocations
            0x22,
            0x33,
            0x44,
            0x55, // PointerToLinenumbers
            0x66,
            0x77,
            0x88,
            0x99, // NumberOfRelocations
            0xaa,
            0xbb, // NumberOfLinenumbers
            0xcc,
            0xdd, // Characteristics
            0xee,
            0xff,
            0x00
        };

        public static readonly byte[] RawResourceDirectoryEntryByName =
        {
            0xff, // Junk
            0xff,
            0x11, // Name
            0x22,
            0x33,
            0x80,
            0x22, // OffsetToData
            0x33,
            0x44,
            0x55
        };

        public static readonly byte[] RawResourceDirectoryEntryById =
        {
            0xff, // Junk
            0xff,
            0x11, // Id
            0x22,
            0x33,
            0x00,
            0x22, // OffsetToData
            0x33,
            0x44,
            0x55
        };

        public static readonly byte[] RawResourceDirectory =
        {
            0xff, // Junk
            0xff,
            0x00, // Characteristics
            0x11,
            0x22,
            0x33,
            0x44, // TimeDateStamp
            0x55,
            0x66,
            0x77,
            0x88, // MajorVersion
            0x99,
            0xaa, // MinorVersion
            0xbb,
            0x01, // NumberOfNamedEntries
            0x00,
            0x01, // NumberOfIdEntries
            0x00,

            // Named entry
            0x11, // Name/Id
            0x22,
            0x33,
            0x44,
            0x55, // OffsetToData
            0x66,
            0x77,
            0x88,

            // Id entry
            0x22, // Name/Id
            0x22,
            0x33,
            0x44,
            0x22, // OffsetToData
            0x66,
            0x77,
            0x88
        };

        public static readonly byte[] RawResourceDirStringU =
        {
            0xff, // Junk
            0xff,
            0x0b, // Length
            0x00,

            // Resource name
            72, // H
            0,
            101, // e
            0,
            108, // l
            0,
            108, // l
            0,
            111, // o
            0,
            32, // ' '
            0,
            87, // W
            0,
            111, // o
            0,
            114, // r
            0,
            108, // l
            0,
            100, // d
            0
        };

        public static readonly byte[] RawResourceDataEntry =
        {
            0xff, // Junk
            0xff,
            0x00, // OffsetToData
            0x11,
            0x22,
            0x33,
            0x44, // Size1
            0x55,
            0x66,
            0x77,
            0x88, // CodePage
            0x99,
            0xaa,
            0xbb,
            0xcc, // Reserved
            0xdd,
            0xee,
            0xff
        };

        public static readonly byte[] RawImageOptionalHeader64Bit =
        {
            0xff, // Crap for offset
            0xff, // Crap

            0x0b, // Magic (32 Bit)
            0x01,
            0x11, // MajorLinkerVersion

            0x33, // MinorLinkerVersion

            0x44, // SizeOfCode
            0x55,
            0x11,
            0x22,
            0x77, // SizeOfInitializedData
            0x88,
            0x99,
            0xaa,
            0x44, // SizeOfUnitializedData
            0x55,
            0x11,
            0x22,
            0x22, // AddressOfEntryPoint
            0x88,
            0x77,
            0xaa,
            0x77, // BaseOfCode
            0x88,
            0xff,
            0xaa,
            0x44, // ImageBase
            0xcc,
            0x99,
            0xaa,
            0x33,
            0x22,
            0x11,
            0x00,
            0x77, // SectionAlignment
            0xcc,
            0x99,
            0xaa,
            0xdd, // FileAlignment
            0xff,
            0x99,
            0xaa,
            0x77, // MajorOperatingSystemVersion
            0x88,
            0x99, // MinorOperatingSystemVersion
            0xaa,
            0xff, // MajorImageVersion
            0x44,
            0xdd, // MinorImageVersion
            0xee,
            0xbb, // MajorSubsystemVersion
            0x88,
            0xee, // MinorSubsystemVersion
            0xaa,
            0x77, // Win32VerionValue
            0x88,
            0x99,
            0xaa,
            0xaa, // SizeOfImage
            0xbb,
            0x99,
            0xaa,
            0x77, // SizeOfHeaders
            0x88,
            0xff,
            0xaa,
            0x77, // Checksum
            0x88,
            0x99,
            0xcc,
            0x77, // Subsystem
            0x88,
            0x99, // DllCharacteristics
            0xaa,
            0xff, // SizeOfStackReserve
            0xdd,
            0x99,
            0xaa,
            0x22,
            0x33,
            0x44,
            0x55,
            0x88, // SizeOfStackCommit
            0x88,
            0x99,
            0xaa,
            0x88,
            0x88,
            0x99,
            0xaa,
            0xee, // SizeOfHeapReserve
            0xcc,
            0x99,
            0xaa,
            0xee,
            0xcc,
            0x99,
            0xaa,
            0x77, // SizeOfHeapCommit
            0x11,
            0x99,
            0xaa,
            0x77,
            0x11,
            0x99,
            0xaa,
            0x22, // LoaderFlags
            0x88,
            0x99,
            0xaa,
            0x05, // NumberOfRvaAndSizes
            0x00,
            0x00,
            0x00,

            // Data Directories

            0x11, // Virtual Address Export
            0x22,
            0x33,
            0x44,
            0x33, // Size of Export
            0x22,
            0x33,
            0x44,
            0x44, // Virtual Address Import
            0x22,
            0x33,
            0x44,
            0x55, // Size of Import
            0x22,
            0x33,
            0x44,
            0x66, // Virtual Address Resource
            0x77,
            0x88,
            0x44,
            0x99, // Size of Resource
            0xaa,
            0x33,
            0x44,
            0x11, // Virtual Address Exception
            0x22,
            0x88,
            0x44,
            0x33, // Size of Exception
            0x44,
            0x33,
            0x44,
            0xbb, // Virtual Address Security
            0x22,
            0x33,
            0x44,
            0xcc, // Size of Security
            0x22,
            0x33,
            0x44,
            0xdd, // Virtual Address Basereloc
            0x22,
            0x33,
            0x44,
            0xee, // Size of Basereloc
            0x22,
            0x33,
            0x44,
            0xff, // Virtual Address Debug
            0x22,
            0x33,
            0x44,
            0x11, // Size of Debug
            0x33,
            0x33,
            0x44,
            0x11, // Virtual Address Copyright
            0x44,
            0x33,
            0x44,
            0x11, // Size of Copyright
            0x55,
            0x33,
            0x44,
            0x11, // Virtual Address Globalprt
            0x66,
            0x33,
            0x44,
            0x11, // Size of Globalprt
            0x77,
            0x33,
            0x44,
            0x11, // Virtual Address TLS
            0x88,
            0x33,
            0x44,
            0x11, // Size of TLS
            0x99,
            0x33,
            0x44,
            0x11, // Virtual Address Load_Config
            0xaa,
            0x33,
            0x44,
            0x11, // Size of Load_Config
            0xbb,
            0x33,
            0x44,
            0xee, // Virtual Address Bound_Import
            0x99,
            0x33,
            0x44,
            0x99, // Size of Bound_Import
            0x00,
            0x33,
            0x44,
            0x11, // Virtual Address IAT
            0xcc,
            0x33,
            0x44,
            0x11, // Size of IAT
            0xdd,
            0x33,
            0x44,
            0x11, // Virtual Address Delay_Import
            0xee,
            0x33,
            0x44,
            0x11, // Size of Delay_Import
            0xff,
            0x33,
            0x44,
            0x11, // Virtual Address Com_Descriptor
            0x22,
            0x44,
            0x44,
            0x11, // Size of Com_Descriptor
            0x22,
            0x55,
            0x44,
            0x11, // Virtual Address of Reserved
            0x22,
            0xcc,
            0xff,
            0x11, // Size of Reserved
            0x22,
            0xcc,
            0x44
        };

        public static readonly byte[] RawImageOptionalHeader32Bit =
        {
            0xff, // Crap for offset
            0xff, // Crap

            0x0b, // Magic (32 Bit)
            0x01,
            0x11, // MajorLinkerVersion

            0x33, // MinorLinkerVersion

            0x44, // SizeOfCode
            0x55,
            0x11,
            0x22,
            0x77, // SizeOfInitializedData
            0x88,
            0x99,
            0xaa,
            0x44, // SizeOfUnitializedData
            0x55,
            0x11,
            0x22,
            0x22, // AddressOfEntryPoint
            0x88,
            0x77,
            0xaa,
            0x77, // BaseOfCode
            0x88,
            0xff,
            0xaa,
            0x44, // BaseOfData
            0x88,
            0x99,
            0xaa,
            0x44, // ImageBase
            0xcc,
            0x99,
            0xaa,
            0x77, // SectionAlignment
            0xcc,
            0x99,
            0xaa,
            0xdd, // FileAlignment
            0xff,
            0x99,
            0xaa,
            0x77, // MajorOperatingSystemVersion
            0x88,
            0x99, // MinorOperatingSystemVersion
            0xaa,
            0xff, // MajorImageVersion
            0x44,
            0xdd, // MinorImageVersion
            0xee,
            0xbb, // MajorSubsystemVersion
            0x88,
            0xee, // MinorSubsystemVersion
            0xaa,
            0x77, // Win32VerionValue
            0x88,
            0x99,
            0xaa,
            0xaa, // SizeOfImage
            0xbb,
            0x99,
            0xaa,
            0x77, // SizeOfHeaders
            0x88,
            0xff,
            0xaa,
            0x77, // Checksum
            0x88,
            0x99,
            0xcc,
            0x77, // Subsystem
            0x88,
            0x99, // DllCharacteristics
            0xaa,
            0xff, // SizeOfStackReserve
            0xdd,
            0x99,
            0xaa,
            0x88, // SizeOfStackCommit
            0x88,
            0x99,
            0xaa,
            0xee, // SizeOfHeapReserve
            0xcc,
            0x99,
            0xaa,
            0x77, // SizeOfHeapCommit
            0x11,
            0x99,
            0xaa,
            0x22, // LoaderFlags
            0x88,
            0x99,
            0xaa,
            0x05, // NumberOfRvaAndSizes
            0x00,
            0x00,
            0x00,

            // Data Directories

            0x11, // Virtual Address Export
            0x22,
            0x33,
            0x44,
            0x33, // Size of Export
            0x22,
            0x33,
            0x44,
            0x44, // Virtual Address Import
            0x22,
            0x33,
            0x44,
            0x55, // Size of Import
            0x22,
            0x33,
            0x44,
            0x66, // Virtual Address Resource
            0x77,
            0x88,
            0x44,
            0x99, // Size of Resource
            0xaa,
            0x33,
            0x44,
            0x11, // Virtual Address Exception
            0x22,
            0x88,
            0x44,
            0x33, // Size of Exception
            0x44,
            0x33,
            0x44,
            0xbb, // Virtual Address Security
            0x22,
            0x33,
            0x44,
            0xcc, // Size of Security
            0x22,
            0x33,
            0x44,
            0xdd, // Virtual Address Basereloc
            0x22,
            0x33,
            0x44,
            0xee, // Size of Basereloc
            0x22,
            0x33,
            0x44,
            0xff, // Virtual Address Debug
            0x22,
            0x33,
            0x44,
            0x11, // Size of Debug
            0x33,
            0x33,
            0x44,
            0x11, // Virtual Address Copyright
            0x44,
            0x33,
            0x44,
            0x11, // Size of Copyright
            0x55,
            0x33,
            0x44,
            0x11, // Virtual Address Globalprt
            0x66,
            0x33,
            0x44,
            0x11, // Size of Globalprt
            0x77,
            0x33,
            0x44,
            0x11, // Virtual Address TLS
            0x88,
            0x33,
            0x44,
            0x11, // Size of TLS
            0x99,
            0x33,
            0x44,
            0x11, // Virtual Address Load_Config
            0xaa,
            0x33,
            0x44,
            0x11, // Size of Load_Config
            0xbb,
            0x33,
            0x44,
            0xee, // Virtual Address Bound_Import
            0x99,
            0x33,
            0x44,
            0x99, // Size of Bound_Import
            0x00,
            0x33,
            0x44,
            0x11, // Virtual Address IAT
            0xcc,
            0x33,
            0x44,
            0x11, // Size of IAT
            0xdd,
            0x33,
            0x44,
            0x11, // Virtual Address Delay_Import
            0xee,
            0x33,
            0x44,
            0x11, // Size of Delay_Import
            0xff,
            0x33,
            0x44,
            0x11, // Virtual Address Com_Descriptor
            0x22,
            0x44,
            0x44,
            0x11, // Size of Com_Descriptor
            0x22,
            0x55,
            0x44,
            0x11, // Virtual Address of Reserved
            0x22,
            0xcc,
            0xff,
            0x11, // Size of Reserved
            0x22,
            0xcc,
            0x44
        };

        public static readonly byte[] RawImportDescriptor =
        {
            0xff, // Junk
            0xff,
            0x00, // OriginalFirstThunk / Characteristics
            0x11,
            0x22,
            0x33,
            0x44, // TimeDateStamp
            0x55,
            0x66,
            0x77,
            0x88, // ForwarderChain
            0x99,
            0xaa,
            0xbb,
            0xcc, // Name
            0xdd,
            0xee,
            0xff,
            0x11, // FirstThunk
            0x22,
            0x33,
            0x44
        };

        public static readonly byte[] RawImportByName =
        {
            0xff, // Junk
            0xff,
            0x00, // Hint
            0x11,

            // Name
            72, // H
            101, // e
            108, // l
            108, // l
            111, // o
            32, // ' '
            87, // W
            111, // o
            114, // r
            108, // l
            100, // d
            00
        };

        public static readonly byte[] RawImageBaseRelocation =
        {
            0xff, // Junk
            0xff,
            0x00, // VirtualAddress
            0x00,
            0x01,
            0x00,
            0x0c, // SizeOfBlock
            0x00,
            0x00,
            0x00,
            0x11, // TypeOffset
            0x22,
            0x33, // TypeOffset
            0x44
        };

        public static readonly byte[] RawFileHeader =
        {
            0xff, // Junk
            0xff,
            0x00, // Machine
            0x11,
            0x22, // NumberOfSections
            0x33,
            0x44, // TimeDateStamp
            0x55,
            0x66,
            0x77,
            0x88, // PointerToSymbolTable
            0x99,
            0xaa,
            0xbb,
            0xcc, // NumberOfSymbols
            0xdd,
            0xee,
            0xff,
            0x11, // SizeOfOptionalHeader
            0x22,
            0x33, // Characteristics
            0x44
        };

        public static readonly byte[] RawExportDirectory =
        {
            0xff, // Junk
            0xff,
            0x00, // Characteristics
            0x11,
            0x22,
            0x33,
            0x44, // TimeDataStamp
            0x55,
            0x66,
            0x77,
            0x88, // MajorVersion
            0x99,
            0xaa, // MinorVersion
            0xbb,
            0xcc, // Name
            0xdd,
            0xee,
            0xff,
            0x22, // Base
            0x33,
            0x44,
            0x55,
            0x11, // NumberOfFunctions
            0x22,
            0x33,
            0x44,
            0x55, // NumberOfNames
            0x66,
            0x77,
            0x88,
            0x99, // AddressOfFunctions
            0xaa,
            0xbb,
            0xcc,
            0xdd, // AddressOfNames
            0xee,
            0xff,
            0x00,
            0x22, // AddressOfNameOrdinals
            0x33,
            0x44,
            0x55
        };

        public static readonly byte[] RawDataDirectory =
        {
            0xff, // Junk
            0xff,
            0x11, // VirtualAddress
            0x22,
            0x33,
            0x44,
            0x55, // Size
            0x66,
            0x77,
            0x88,
            0xff, // Junk
            0xff
        };

        public static readonly byte[] RawDebugDirectory =
        {
            0xff, // Foo for offset test
            0xff,
            0x11, // Characteristics
            0x22,
            0x33,
            0x44,
            0x55, // TimeDateStamp
            0x66,
            0x77,
            0x88,
            0x99, // MajorVersion
            0xaa,
            0xbb, // MinorVersion
            0xcc,
            0xdd, // Type
            0xee,
            0xff,
            0x11,
            0x22, // SizeOfData
            0x33,
            0x44,
            0x55,
            0x66, // AddressOfRawData
            0x77,
            0x88,
            0x99,
            0xaa, // PointerToRawData
            0xbb,
            0xcc,
            0xdd
        };

        public static readonly byte[] RawDosHeader =
        {
            0x00, // e_magic
            0x11,
            0x22, // e_cblp
            0x33,
            0x44, // e_cp
            0x55,
            0x66, // e_crlc
            0x77,
            0x88, // e_cparhdr
            0x99,
            0xaa, // e_minalloc
            0xbb,
            0xcc, // e_maxalloc
            0xdd,
            0xff, // e_ss
            0x00,
            0x11, // e_sp
            0x22,
            0x33, // e_csum
            0x44,
            0x55, // e_ip
            0x66,
            0x77, // e_cs
            0x88,
            0x99, // e_lfalc
            0xaa,
            0xbb, // e_ovno
            0xcc,
            0xdd, // e_res
            0xee,
            0xff,
            0x00,
            0x11,
            0x22,
            0x33,
            0x44,
            0x55, // e_oemid
            0x66,
            0x77, // e_oeminfo
            0x88,
            0x99, // e_res2
            0xaa,
            0xbb,
            0xcc,
            0xdd,
            0xee,
            0xff,
            0x11,
            0x22,
            0x33,
            0x44,
            0x55,
            0x66,
            0x77,
            0x88,
            0x99,
            0xaa,
            0xbb,
            0xcc,
            0xbb
        };

        public static byte[] RawImageNtHeaders64
        {
            get
            {
                var signature = new byte[]
                {
                    0xff, // Junk,
                    0xff,
                    0x00, // Signature
                    0x11,
                    0x22,
                    0x33
                };

                var bytes = new byte[6 + 0x70 + 0x14];

                signature.CopyTo(bytes, 0);

                // File Header
                new byte[0x14].CopyTo(bytes, 6);

                // Optional Header
                new byte[0x70].CopyTo(bytes, 0x1a);

                return bytes;
            }
        }
    }
}