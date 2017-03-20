using System;
using System.IO;
using PeNet;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var path = @"C:\Windows\Syswow64\kernel32.dll";
            var file = new PeFile(path);

            Console.WriteLine($"IsSigned: {file.IsSigned}");
            Console.WriteLine($"IsValidCertChain: {file.IsValidCertChain(true)}");
            Console.WriteLine($"Hash: {file.PKCS7.GetHashCode()}");
            Console.WriteLine($"PubKey: {file.PKCS7.PublicKey.EncodedKeyValue.Format(true)}");
            Console.WriteLine($"SigAlg: {file.PKCS7.SignatureAlgorithm.FriendlyName}");
            Console.WriteLine($"Thumbprint: {file.PKCS7.Thumbprint}");
            Console.WriteLine($"IsSignatureValid: {PeNet.Utilities.SignatureInformation.IsSignatureValid(path)}");
            var signedContent = GetSignedContent(file, file.Buff);

            var foo =
                new X509Certificate2(
                    X509Certificate.CreateFromSignedFile(path));

            Console.WriteLine($"My verification {VerifySignature(signedContent, file.WinCertificate.bCertificate, file.PKCS7)}");
            Console.ReadKey();
        }

        private static bool VerifySignature(byte[] content, byte[] signature, X509Certificate2 cert)
        {
            var csp = (RSACryptoServiceProvider) cert.PublicKey.Key;
            var sha256Managed = new SHA256Managed();
            var hashedData = sha256Managed.ComputeHash(content);

            Console.WriteLine($"My hash {PeNet.Utilities.ExtensionMethods.ToHexString(hashedData)}");

            return csp.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA256"), signature);
        }

        private static byte[] GetSignedContent(PeFile peFile, byte[] buffer)
        {
            // Cut out checksum
            uint sizeOfChecksum = 4; // DWORD
            uint addressOfChecksum = 0x150; // Offset from file start
            var output = RemoveBytesAt(buffer, addressOfChecksum, sizeOfChecksum);

            // Cut out signature directory struct
            uint sizeOfSigDir = 8; // 2 * DWORD
            uint addressOfSigDir = 0x190 - sizeOfChecksum;
            output = RemoveBytesAt(output, addressOfSigDir, sizeOfSigDir);

            // Cut out signature
            uint sizeOfSignature = peFile.WinCertificate.dwLength;
            uint addressOfSignature = peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(uint)PeNet.Constants.DataDirectoryIndex.Security].VirtualAddress - sizeOfChecksum - sizeOfSigDir;
            output = RemoveBytesAt(output, addressOfSignature, sizeOfSignature);

            return output;
        }

        private static byte[] RemoveBytesAt(byte[] source, uint offset, uint length)
        {
            var output = new byte[source.Length - length];

            // Copy from the start until the part that should be left out.
            Array.Copy(source, 0, output, 0, offset);

            // Copy from the part that should be left out to the end.
            Array.Copy(source, offset + length, output, offset, source.Length - offset - length);

            return output;
        }
    }
}