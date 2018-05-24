using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using PeNet.Asn1;
using PeNet.Utilities;

namespace PeNet.Authenticode

    // References:
    // a.	http://www.cs.auckland.ac.nz/~pgut001/pubs/authenticode.txt
{
    public class AuthenticodeInfo
    {
        private readonly PeFile _peFile;
        private readonly ContentInfo _contentInfo;

        public string SignerSerialNumber { get; }
        public byte[] SignedHash { get; }
        public bool IsAuthenticodeValid { get; }
        public X509Certificate2 SigningCertificate { get; }

        public AuthenticodeInfo(PeFile peFile)
        {
            _peFile = peFile;
            _contentInfo = new ContentInfo(_peFile.WinCertificate.bCertificate);
            SignerSerialNumber = GetSigningSerialNumber();
            SignedHash = GetSignedHash();
            IsAuthenticodeValid = CheckSignature();
            SigningCertificate = GetSigningCertificate();
        }

        private X509Certificate2 GetSigningCertificate()
        {
            if (_peFile.WinCertificate?.wCertificateType !=
                (ushort) Constants.WinCertificateType.WIN_CERT_TYPE_PKCS_SIGNED_DATA)
            {
                return null;
            }

            var pkcs7 = _peFile.WinCertificate.bCertificate;

            // Workaround since the X509Certificate2 class does not return
            // the signing certificate in the PKCS7 byte array but crashes on Linux 
            // when using .Net Core.
            // Under Windows with .Net Core the class works as intended.
            // See issue: https://github.com/dotnet/corefx/issues/25828

#if NETSTANDARD2_0
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 
                new X509Certificate2(pkcs7) : GetSigningCertificateNonWindows(_peFile); 
#else
            return new X509Certificate2(pkcs7);
#endif
        }

        private X509Certificate2 GetSigningCertificateNonWindows(PeFile peFile)
        {
            var collection = new X509Certificate2Collection();
            collection.Import(peFile.WinCertificate.bCertificate);
            return collection.Cast<X509Certificate2>().FirstOrDefault(cert =>
                string.Equals(cert.SerialNumber, SignerSerialNumber, StringComparison.CurrentCultureIgnoreCase));
        }

        private bool CheckSignature()
        {
            if (SignedHash == null) return false;
            // 2.  Initialize a hash algorithm context.
            HashAlgorithm ha;
            switch (SignedHash.Length)
            {
                case 16:
                    ha = MD5.Create();
                    break;
                case 20:
                    ha = SHA1.Create();
                    break;
                case 32:
                    ha = SHA256.Create();
                    break;
                case 48:
                    ha = SHA384.Create();
                    break;
                case 64:
                    ha = SHA512.Create();
                    break;
                default:
                    return false;
            }
            var hash = GetHash(ha);
            return SignedHash.SequenceEqual(hash);
        }

        private byte[] GetSignedHash()
        {
            if (_contentInfo.ContentType != "1.2.840.113549.1.7.2") //1.2.840.113549.1.7.2 = OID for signedData
            {
                return null;
            }

            var sd = new SignedData(_contentInfo.Content);
            if (sd.ContentInfo.ContentType != "1.3.6.1.4.1.311.2.1.4") // 1.3.6.1.4.1.311.2.1.4 = OID for Microsoft Crypto
            {
                return null;
            }

            var spc = sd.ContentInfo.Content;
            var signedHash = (Asn1OctetString)spc.Nodes[0].Nodes[1].Nodes[1];
            return signedHash.Data;
        }

        private string GetSigningSerialNumber()
        {
            var asn1 = _contentInfo.Content;
            var x = (Asn1Integer)asn1.Nodes[0].Nodes[4].Nodes[0].Nodes[1].Nodes[1]; // ASN.1 Path to signer serial number: /1/0/4/0/1/1
            return x.Value.ToHexString().Substring(2).ToUpper();
        }

        private IEnumerable<byte> GetHash(HashAlgorithm hash)
        {
            // 3.  Hash the image header from its base to immediately before the start of the checksum address, 
            // as specified in Optional Header Windows-Specific Fields.
            var offset = Convert.ToInt32(_peFile.ImageNtHeaders.OptionalHeader.Offset) + 0x40;
            hash.TransformBlock(_peFile.Buff, 0, offset, new byte[offset], 0);

            // 4.  Skip over the checksum, which is a 4-byte field.
            offset += 0x4;

            // 6.  Get the Attribute Certificate Table address and size from the Certificate Table entry. 
            // For details, see section 5.7 of the PE/COFF specification.
            var certificateTable = _peFile.ImageNtHeaders.OptionalHeader.DataDirectory[4];

            // 5.  Hash everything from the end of the checksum field to immediately before the start of the Certificate Table entry,
            // as specified in Optional Header Data Directories.
            var length = Convert.ToInt32(certificateTable.Offset) - offset;
            hash.TransformBlock(_peFile.Buff, offset, length, new byte[length], 0);
            offset += length + 0x8;//end of Attribute Certificate Table addres

            // 7.  Exclude the Certificate Table entry from the calculation and 
            // hash everything from the end of the Certificate Table entry to the end of image header, 
            // including Section Table (headers). The Certificate Table entry is 8 bytes long, as specified in Optional Header Data Directories.
            length = Convert.ToInt32(_peFile.ImageNtHeaders.OptionalHeader.SizeOfHeaders) - offset;// end optional header
            hash.TransformBlock(_peFile.Buff, offset, length, new byte[length], 0);

            // 8-13. Hash everything between end of header and certificate
            var sizeOfHeaders = Convert.ToInt32(_peFile.ImageNtHeaders.OptionalHeader.SizeOfHeaders);
            length = Convert.ToInt32(_peFile.WinCertificate.Offset) - sizeOfHeaders;
            hash.TransformBlock(_peFile.Buff, sizeOfHeaders, length, new byte[length], 0);

            // 14. Create a value called FILE_SIZE, which is not part of the signature. 
            // Set this value to the image’s file size, acquired from the underlying file system. 
            // If FILE_SIZE is greater than SUM_OF_BYTES_HASHED, the file contains extra data that must be added to the hash. 
            // This data begins at the SUM_OF_BYTES_HASHED file offset, and its length is:
            // (File Size) – ((Size of AttributeCertificateTable) + SUM_OF_BYTES_HASHED)
            // Note: The size of Attribute Certificate Table is specified 
            // in the second ULONG value in the Certificate Table entry (32 bit: offset 132, 64 bit: offset 148) in Optional Header Data Directories.
            // 14. Hash everything from the end of the certificate to the end of the file.
            var fileSize = _peFile.Buff.Length;
            var sizeOfAttributeCertificateTable = Convert.ToInt32(certificateTable.Size);
            offset = sizeOfAttributeCertificateTable + Convert.ToInt32(_peFile.WinCertificate.Offset);
            if (fileSize > offset)
            {
                length = fileSize - offset;
                if (length != 0)
                {
                    hash.TransformBlock(_peFile.Buff, offset, length, new byte[length], 0);
                }
            }

            // 15. Finalize the hash algorithm context.
            hash.TransformFinalBlock(_peFile.Buff, 0, 0);
            return hash.Hash;
        }
    }
}