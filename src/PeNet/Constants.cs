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