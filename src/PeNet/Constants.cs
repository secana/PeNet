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

       



        //////////////////////////
        // ImageOptionalHeader
        //////////////////////////

        


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