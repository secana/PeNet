using System;
using System.Security.Cryptography.X509Certificates;

namespace PeNet.Utilities
{
    /// <summary>
    /// Information about the digital signature of the PE file.
    /// </summary>
    public static class SignatureInformation
    {
        /// <summary>
        ///     Checks is a PE file is digitally signed. It does not
        ///     verify the signature!
        /// </summary>
        /// <param name="filePath">Path to a PE file.</param>
        /// <returns>True if signed, false if not. </returns>
        public static bool IsSigned(string filePath)
        {
            return IsSigned(filePath, out _);
        }

        /// <summary>
        ///     Checks is a PE file is digitally signed. It does not
        ///     verify the signature!
        /// </summary>
        /// <param name="filePath">Path to a PE file.</param>
        /// <param name="cert">The certificate used to sign the binary.</param>
        /// <returns>True if signed, false if not. </returns>
        public static bool IsSigned(string filePath, out X509Certificate2 cert)
        {
            cert = null;
            try
            {
                var peFile = new PeFile(filePath);
                if (peFile.PKCS7 == null)
                {
                    return false;
                }
                else
                {
                    cert = peFile.PKCS7;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="filePath">Path to a PE file.</param>
        /// <param name="online">Check certificate chain online or offline.</param>
        /// <returns>True of cert chain is valid and from a trusted CA.</returns>
        public static bool IsValidCertChain(string filePath, bool online)
        {
            return IsSigned(filePath, out var cert) 
                   && IsValidCertChain(cert, online);
        }

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="filePath">Path to a PE file.</param>
        /// <param name="urlRetrievalTimeout">Timout to validate the certificate online.</param>
        /// <param name="excludeRoot">True if the root certificate should not be validatet. False if the whole chain should be validated.</param>
        /// <returns>True of cert chain is valid and from a trusted CA.</returns>
        public static bool IsValidCertChain(string filePath, TimeSpan urlRetrievalTimeout, bool excludeRoot = true)
        {
            return IsSigned(filePath, out var cert) 
                   && IsValidCertChain(cert, urlRetrievalTimeout, excludeRoot);
        }

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="cert">X509 Certificate</param>
        /// <param name="online">Check certificate chain online or offline. The online check defaults to a one minute timeout.</param>
        /// <returns>True of cert chain is valid and from a trusted CA.</returns>
        public static bool IsValidCertChain(X509Certificate2 cert, bool online)
        {
            var chain = new X509Chain
            {
                ChainPolicy =
                {
                    RevocationFlag = X509RevocationFlag.ExcludeRoot,
                    RevocationMode = online ? X509RevocationMode.Online : X509RevocationMode.Offline,
                    UrlRetrievalTimeout = new TimeSpan(0, 1, 0),
                    VerificationFlags = X509VerificationFlags.NoFlag
                }
            };
            return chain.Build(cert);
        }

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="cert">X509 Certificate</param>
        /// <param name="urlRetrievalTimeout">Timout to validate the certificate online.</param>
        /// <param name="excludeRoot">True if the root certificate should not be validatet. False if the whole chain should be validated.</param>
        /// <returns>True of cert chain is valid and from a trusted CA.</returns>
        public static bool IsValidCertChain(X509Certificate2 cert, TimeSpan urlRetrievalTimeout, bool excludeRoot = true)
        {
            var chain = new X509Chain
            {
                ChainPolicy =
                {
                    RevocationFlag      = excludeRoot ? X509RevocationFlag.ExcludeRoot : X509RevocationFlag.EntireChain,
                    RevocationMode      = X509RevocationMode.Online,
                    UrlRetrievalTimeout = urlRetrievalTimeout,
                    VerificationFlags   = X509VerificationFlags.NoFlag
                }
            };
            return chain.Build(cert);
        }

        /// <summary>
        ///     Checks if the digital signature of a PE file is valid.
        /// </summary>
        /// <param name="filePath">Path to a PE file.</param>
        /// <returns>True if the signature is valid, else false.</returns>
        [Obsolete("use `new PeFile(filePath).IsSignatureValid`", true)]
        public static bool IsSignatureValid(string filePath)
        {
            return new PeFile(filePath).IsSignatureValid;
        }
    }
}