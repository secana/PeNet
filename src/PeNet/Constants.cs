using System;

namespace PeNet
{
    /// <summary>
    ///     This class contains constants and flags which are used in a PE file.
    ///     The constants can be used to map a numeric value to an understandable string.
    /// </summary>
    public static class Constants
    {
        ////////////////////////
        // IMAGE_DATA_DIRECTORY
        ////////////////////////

        /// <summary>
        ///     The data directory indices used to resolve
        ///     which directory is which.
        /// </summary>
        public enum DataDirectoryIndex
        {
            /// <summary>
            ///     Export directory.
            /// </summary>
            Export = 0,

            /// <summary>
            ///     Import directory.
            /// </summary>
            Import = 1,

            /// <summary>
            ///     Resource directory.
            /// </summary>
            Resource = 2,

            /// <summary>
            ///     Exception directory for x64.
            /// </summary>
            Exception = 3,

            /// <summary>
            ///     Security directory.
            /// </summary>
            Security = 4,

            /// <summary>
            ///     Relocation directory.
            /// </summary>
            BaseReloc = 5,

            /// <summary>
            ///     Debug directory.
            /// </summary>
            Debug = 6,

            /// <summary>
            ///     Copyright directory (useless).
            /// </summary>
            Copyright = 7,

            /// <summary>
            ///     Global Pointer directory. Only interesting for Itanium systems.
            /// </summary>
            Globalptr = 8,

            /// <summary>
            ///     Thread Local Storage directory.
            /// </summary>
            TLS = 9,

            /// <summary>
            ///     Load Config directory.
            /// </summary>
            LoadConfig = 0xA,

            /// <summary>
            ///     Bound Import directory. Precomputed import addresses
            ///     to speed up module loading.
            /// </summary>
            BoundImport = 0xB,

            /// <summary>
            ///     Import Address Table directory.
            /// </summary>
            IAT = 0xC,

            /// <summary>
            ///     Delayed Import directory. Imports which are loaded
            ///     with a delay for performance reasons.
            /// </summary>
            DelayImport = 0xD,

            /// <summary>
            ///     COM Descriptor directory. For the .Net Header
            /// </summary>
            COM_Descriptor = 0xE,

            /// <summary>
            ///     Reserved for future use.
            /// </summary>
            Reserved = 0xF
        }

        //////////////////////////////
        // IMAGE_FILE_HEADER constants
        //////////////////////////////

       

        /// <summary>
        ///     Constants for the Optional header DllCharacteristics
        ///     property.
        /// </summary>
        [Flags]
        public enum OptionalHeaderDllCharacteristics : ushort
        {
            /// <summary>
            ///     DLL can be relocated at load time.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_DYNAMIC_BASE = 0x40,

            /// <summary>
            ///     Enforces integrity checks.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_FORCE_INTEGRITY = 0x80,

            /// <summary>
            ///     Image is compatible with Data Execution Prevention (DEP).
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_NX_COMPAT = 0x100,

            /// <summary>
            ///     Image is isolation aware but should not be isolated.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_NO_ISOLATION = 0x200,

            /// <summary>
            ///     No Secure Exception Handling (SEH)
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_NO_SEH = 0x400,

            /// <summary>
            ///     Do not bind the image.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_NO_BIND,

            /// <summary>
            ///     Image is a WDM driver.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_WDM_DRIVER = 0x2000,

            /// <summary>
            ///     Terminal server aware.
            /// </summary>
            IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = 0x8000
        }


        //////////////////////////
        // IMAGE_OPTIONAL_HEADER
        //////////////////////////

        /// <summary>
        ///     Constants for the Optional header magic property.
        /// </summary>
        [Flags]
        public enum OptionalHeaderMagic : ushort
        {
            /// <summary>
            ///     The file is an 32 bit executable.
            /// </summary>
            IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b,

            /// <summary>
            ///     The file is an 64 bit executable.
            /// </summary>
            IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b,

            /// <summary>
            ///     The file is a ROM image.
            /// </summary>
            IMAGE_ROM_OPTIONAL_HDR_MAGIC = 0x107
        }

        /// <summary>
        ///     Constants for the Optional header subsystem property.
        /// </summary>
        [Flags]
        public enum OptionalHeaderSubsystem : ushort
        {
            /// <summary>
            ///     System driver.
            /// </summary>
            IMAGE_SUBSYSTEM_NATIVE = 0x01,

            /// <summary>
            ///     GUI Subsystem.
            /// </summary>
            IMAGE_SUBSYSTEM_WINDOWS_GUI = 0x02,

            /// <summary>
            ///     Console Subsystem.
            /// </summary>
            IMAGE_SUBSYSTEM_WINDOWS_CUI = 0x03
        }


        //////////////////////////////////
        // IMAGE_RESOURCE_DIRECTORY_ENTRY
        //////////////////////////////////

       

        ////////////////////////
        // IMAGE_SECTION_HEADER
        ////////////////////////

        /// <summary>
        ///     The SectionFlags enumeration lists all possible flags which can
        ///     be set in the section characteristics.
        /// </summary>
        [Flags]
        public enum SectionFlags : uint
        {
            /// <summary>
            ///     Reserved.
            /// </summary>
            IMAGE_SCN_TYPE_NO_PAD = 0x00000008,

            /// <summary>
            ///     Section contains code.
            /// </summary>
            IMAGE_SCN_CNT_CODE = 0x00000020,

            /// <summary>
            ///     Section contains initialized data.
            /// </summary>
            IMAGE_SCN_CNT_INITIALIZED_DATA = 0x00000040,

            /// <summary>
            ///     Section contains uninitialized data.
            /// </summary>
            IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x00000080,

            /// <summary>
            ///     Reserved.
            /// </summary>
            IMAGE_SCN_LNK_OTHER = 0x00000100,

            /// <summary>
            ///     Section contains comments or some  other type of information.
            /// </summary>
            IMAGE_SCN_LNK_INFO = 0x00000200,

            /// <summary>
            ///     Section contents will not become part of image.
            /// </summary>
            IMAGE_SCN_LNK_REMOVE = 0x00000800,

            /// <summary>
            ///     Section contents comdat.
            /// </summary>
            IMAGE_SCN_LNK_COMDAT = 0x00001000,

            /// <summary>
            ///     Reset speculative exceptions handling bits in the TLB entries for this section.
            /// </summary>
            IMAGE_SCN_NO_DEFER_SPEC_EXC = 0x00004000,

            /// <summary>
            ///     Section content can be accessed relative to GP.
            /// </summary>
            IMAGE_SCN_GPREL = 0x00008000,

            /// <summary>
            ///     Unknown.
            /// </summary>
            IMAGE_SCN_MEM_FARDATA = 0x00008000,

            /// <summary>
            ///     Unknown.
            /// </summary>
            IMAGE_SCN_MEM_PURGEABLE = 0x00020000,

            /// <summary>
            ///     Unknown.
            /// </summary>
            IMAGE_SCN_MEM_16BIT = 0x00020000,

            /// <summary>
            ///     Unknown.
            /// </summary>
            IMAGE_SCN_MEM_LOCKED = 0x00040000,

            /// <summary>
            ///     Unknown.
            /// </summary>
            IMAGE_SCN_MEM_PRELOAD = 0x00080000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_1BYTES = 0x00100000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_2BYTES = 0x00200000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_4BYTES = 0x00300000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_8BYTES = 0x00400000,

            /// <summary>
            ///     Default alignment if no others are specified.
            /// </summary>
            IMAGE_SCN_ALIGN_16BYTES = 0x00500000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_32BYTES = 0x00600000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_64BYTES = 0x00700000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_128BYTES = 0x00800000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_256BYTES = 0x00900000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_512BYTES = 0x00A00000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_1024BYTES = 0x00B00000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_2048BYTES = 0x00C00000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_4096BYTES = 0x00D00000,

            /// <summary>
            ///     Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_8192BYTES = 0x00E00000,

            /// <summary>
            ///     Alignment mask.
            /// </summary>
            IMAGE_SCN_ALIGN_MASK = 0x00F00000,

            /// <summary>
            ///     Section contains extended relocations.
            /// </summary>
            IMAGE_SCN_LNK_NRELOC_OVFL = 0x01000000,

            /// <summary>
            ///     Section can be discarded.
            /// </summary>
            IMAGE_SCN_MEM_DISCARDABLE = 0x02000000,

            /// <summary>
            ///     Section is not cache-able.
            /// </summary>
            IMAGE_SCN_MEM_NOT_CACHED = 0x04000000,

            /// <summary>
            ///     Section is not page-able.
            /// </summary>
            IMAGE_SCN_MEM_NOT_PAGED = 0x08000000,

            /// <summary>
            ///     Section is shareable.
            /// </summary>
            IMAGE_SCN_MEM_SHARED = 0x10000000,

            /// <summary>
            ///     Section is executable.
            /// </summary>
            IMAGE_SCN_MEM_EXECUTE = 0x20000000,

            /// <summary>
            ///     Section is readable.
            /// </summary>
            IMAGE_SCN_MEM_READ = 0x40000000,

            /// <summary>
            ///     Section is write-able.
            /// </summary>
            IMAGE_SCN_MEM_WRITE = 0x80000000
        }

        /////////////////
        // UNWINDE_CODE
        /////////////////

        /// <summary>
        ///     UnwindOp Codes for the unwind information
        ///     used to walk the stack in x64 applications.
        /// </summary>
        public enum UnwindOpCodes : byte
        {
            /// <summary>
            ///     Push a non volatile integer.
            /// </summary>
            UWOP_PUSH_NONVOL = 0,

            /// <summary>
            ///     Allocate large size on stack.
            /// </summary>
            UWOP_ALLOC_LARGE = 1,

            /// <summary>
            ///     Allocate small size on stack.
            /// </summary>
            UWOP_ALLOC_SMALL = 2,

            /// <summary>
            ///     Establish frame pointer register.
            /// </summary>
            UWOP_SET_FPREG = 3,

            /// <summary>
            ///     Save non volatile register to stack by a MOV.
            /// </summary>
            UWOP_SAVE_NONVOL = 4,

            /// <summary>
            ///     Save non volatile register to stack with
            ///     a long offset by a MOV.
            /// </summary>
            UWOP_SAVE_NONVOL_FAR = 5,

            /// <summary>
            ///     Save a XMM (128 bit) register to the stack.
            /// </summary>
            UWOP_SAVE_XMM128 = 8,

            /// <summary>
            ///     Save a XMM (128 bit) register to the stack
            ///     with a long offset.
            /// </summary>
            UWOP_SAVE_XMM128_FAR = 9,

            /// <summary>
            ///     Push a machine frame, which is used to record the effect
            ///     of a hardware interrupt.
            /// </summary>
            UWOP_PUSH_MACHFRAME = 10
        }

        //////////////////////////////////////
        // WIN_CERTIFICATE wCertificateType
        //////////////////////////////////////

        /// <summary>
        ///     WIN_CERTIFICATE wCertificateType constants.
        /// </summary>
        [Flags]
        public enum WinCertificateType : ushort
        {
            /// <summary>
            ///     Certificate is X509 standard.
            /// </summary>
            WIN_CERT_TYPE_X509 = 0x0001,

            /// <summary>
            ///     Certificate is PKCS signed data.
            /// </summary>
            WIN_CERT_TYPE_PKCS_SIGNED_DATA = 0x0002,

            /// <summary>
            ///     Reserved
            /// </summary>
            WIN_CERT_TYPE_RESERVED_1 = 0x0003,

            /// <summary>
            ///     Certificate is PKCS1 signature.
            /// </summary>
            WIN_CERT_TYPE_PKCS1_SIGN = 0x0009
        }
    }
}