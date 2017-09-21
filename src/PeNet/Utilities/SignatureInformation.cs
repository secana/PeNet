using System;
using System.Runtime.InteropServices;
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
            try
            {
                var signer = X509Certificate.CreateFromSignedFile(filePath);
                var cert = new X509Certificate2(signer);
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
            X509Certificate2 cert;

            try
            {
                var signer = X509Certificate.CreateFromSignedFile(filePath);
                cert = new X509Certificate2(signer);
            }
            catch (Exception)
            {
                return false;
            }

            return IsValidCertChain(cert, online);
        }

        /// <summary>
        ///     Checks if the digital signature of a PE file is valid.
        ///     Since .Net has not function for it, PInvoke is used to query
        ///     the native API like here http://geekswithblogs.net/robp/archive/2007/05/04/112250.aspx
        /// </summary>
        /// <param name="filePath">Path to a PE file.</param>
        /// <returns>True if the signature is valid, else false.</returns>
        public static bool IsSignatureValid(string filePath)
        {
            #if NET461
                return SignatureValidation.IsTrusted(filePath);
            #else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
                    throw new PlatformNotSupportedException("This features is currently only supported on Windows");

                return SignatureValidation.IsTrusted(filePath);
            #endif
        }

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="cert">X509 Certificate</param>
        /// <param name="online">Check certificate chain online or offline.</param>
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
    }
}