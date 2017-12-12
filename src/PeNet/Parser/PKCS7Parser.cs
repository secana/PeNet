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

            var collection = new X509Certificate2Collection();
            collection.Import(pkcs7);

            if (collection.Count == 0)
                return null;

            return collection[0];
        }
    }
}