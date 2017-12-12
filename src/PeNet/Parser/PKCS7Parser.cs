using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using PeNet.Structures;

namespace PeNet.Parser
{
    internal class PKCS7Parser : SafeParser<X509Certificate2>
    {
        private readonly WIN_CERTIFICATE _winCertificate;

        internal PKCS7Parser(WIN_CERTIFICATE winCertificate)
            : base(null, 0)
        {
            _winCertificate = winCertificate;
        }

        protected override X509Certificate2 ParseTarget()
        {
            if (_winCertificate?.wCertificateType !=
                (ushort) Constants.WinCertificateType.WIN_CERT_TYPE_PKCS_SIGNED_DATA)
            {
                return null;
            }

            var pkcs7 = _winCertificate.bCertificate;

            // Workaround since the X509Certificate2 class does not return
            // the signing certificate in the PKCS7 byte array but crashes on Linux 
            // when using .Net Core.
            // Under Windows with .Net Core the class works as intended.
            // See issue: https://github.com/dotnet/corefx/issues/25828

            #if NET461
                return new X509Certificate2(pkcs7);
            #else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new X509Certificate2(pkcs7);
            }
            else
            {
                return GetSigningCertificateNonWindows(pkcs7);
            }
            #endif
        }

        private X509Certificate2 GetSigningCertificateNonWindows(byte[] pkcs7)
        {
            var collection = new X509Certificate2Collection();
            collection.Import(pkcs7);

            var serial = Authenticode.Authenticode.GetSigningSerialNumber(pkcs7);

            return collection.Cast<X509Certificate2>().FirstOrDefault(cert => string.Equals("0x" + cert.SerialNumber, serial, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}