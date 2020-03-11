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
        // ImageDataDirectory
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
        // ImageFileHeader constants
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
        // ImageOptionalHeader
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


        /////////////////
        // UNWINDE_CODE
        /////////////////

       

        //////////////////////////////////////
        // WinCertificate wCertificateType
        //////////////////////////////////////

        /// <summary>
        ///     WinCertificate wCertificateType constants.
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