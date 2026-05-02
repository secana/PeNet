using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using PeNet.Asn1;
using PeNet.Header.Pe;

namespace PeNet.Header.Authenticode

// References:
// a.	http://www.cs.auckland.ac.nz/~pgut001/pubs/authenticode.txt
{
    public class AuthenticodeInfo
    {
        private readonly PeFile _peFile;
        private readonly ContentInfo? _contentInfo;
        private readonly SignedCms? _signedCms;

        public string? SignerSerialNumber { get; }
        public byte[]? SignedHash { get; }
        public bool IsAuthenticodeValid { get; }
        public X509Certificate2? SigningCertificate { get; }
        public DateTimeOffset? SigningTimestamp { get; }

        public AuthenticodeInfo(PeFile peFile)
        {
            _peFile = peFile;

            _contentInfo = _peFile.WinCertificate == null
                ? null : new ContentInfo(_peFile.WinCertificate.BCertificate);

            _signedCms = DecodeCms();

            SignerSerialNumber = GetSigningSerialNumber();
            SignedHash = GetSignedHash();
            IsAuthenticodeValid = VerifyHash() && VerifySignature();
            SigningCertificate = GetSigningCertificate();
            SigningTimestamp = GetSigningTimestamp();
        }

        private SignedCms? DecodeCms()
        {
            if (_peFile.WinCertificate is null)
                return null;

            try
            {
                var cms = new SignedCms();
                cms.Decode(_peFile.WinCertificate.BCertificate);
                return cms;
            }
            catch
            {
                return null;
            }
        }

        private X509Certificate2? GetSigningCertificate()
        {
            if (_peFile.WinCertificate?.WCertificateType !=
                WinCertificateType.PkcsSignedData)
            {
                return null;
            }

            if (_signedCms is null)
                return null;

            var signerInfos = _signedCms.SignerInfos.Cast<SignerInfo>().Where(si => string.Equals(si.Certificate?.SerialNumber, SignerSerialNumber, StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (signerInfos.Count == 1)
            {
                return signerInfos[0].Certificate;
            }
            var numberOfSignerInfos = signerInfos.Count == 0 ? "none" : signerInfos.Count.ToString();
            throw new CryptographicException($"Expected to find one certificate with serial number '{SignerSerialNumber}' but found {numberOfSignerInfos}.");
        }

        private DateTimeOffset? GetSigningTimestamp()
        {
            if (_signedCms is null)
                return null;

            foreach (var signerInfo in _signedCms.SignerInfos)
            {
                foreach (var unsignedAttribute in signerInfo.UnsignedAttributes)
                {
                    // RFC 3161 timestamp counter-signature
                    if (unsignedAttribute.Oid.Value == "1.3.6.1.4.1.311.3.3.1")
                    {
                        var timestamp = GetTimestampFromRfc3161(unsignedAttribute);
                        if (timestamp.HasValue)
                            return timestamp;
                    }
                }

                // Legacy Authenticode counter-signature via CounterSignerInfos
                var legacyTimestamp = GetTimestampFromCounterSignerInfos(signerInfo);
                if (legacyTimestamp.HasValue)
                    return legacyTimestamp;
            }

            return null;
        }

        private static DateTimeOffset? GetTimestampFromRfc3161(CryptographicAttributeObject unsignedAttribute)
        {
            if (unsignedAttribute.Values.Count == 0)
                return null;

            try
            {
                var timestampCms = new SignedCms();
                timestampCms.Decode(unsignedAttribute.Values[0].RawData);

                var tstInfo = timestampCms.ContentInfo.Content;
                if (tstInfo is null || tstInfo.Length == 0)
                    return null;

                var reader = new AsnReader(tstInfo, AsnEncodingRules.DER);
                var sequenceReader = reader.ReadSequence();
                sequenceReader.ReadInteger();           // version
                sequenceReader.ReadObjectIdentifier(); // policy
                sequenceReader.ReadSequence();         // messageImprint
                sequenceReader.ReadInteger();           // serialNumber
                var genTime = sequenceReader.ReadGeneralizedTime(); // genTime

                return genTime;
            }
            catch
            {
                return null;
            }
        }

        private static DateTimeOffset? GetTimestampFromCounterSignerInfos(SignerInfo signerInfo)
        {
            try
            {
                foreach (var counterSigner in signerInfo.CounterSignerInfos)
                {
                    foreach (var signedAttr in counterSigner.SignedAttributes)
                    {
                        if (signedAttr.Oid.Value == "1.2.840.113549.1.9.5") // signing-time
                        {
                            if (signedAttr.Values.Count > 0)
                            {
                                var signingTime = (Pkcs9SigningTime)signedAttr.Values[0];
                                return new DateTimeOffset(signingTime.SigningTime);
                            }
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private bool VerifySignature()
        {
            if (_signedCms is null) return false;

            try
            {
                // Throws an exception if the signature is invalid.
                _signedCms.CheckSignature(true);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool VerifyHash()
        {
            if (SignedHash == null) return false;
            // 2.  Initialize a hash algorithm context.
            var algorithmName = SignedHash.Length switch
            {
                16 => HashAlgorithmName.MD5,
                20 => HashAlgorithmName.SHA1,
                32 => HashAlgorithmName.SHA256,
                48 => HashAlgorithmName.SHA384,
                64 => HashAlgorithmName.SHA512,
                _ => (HashAlgorithmName?)null
            };

            if (algorithmName is null) return false;

            var hash = ComputeAuthenticodeHashFromPeFile(algorithmName.Value);
            return hash != null && SignedHash.SequenceEqual(hash);
        }

        private byte[]? GetSignedHash()
        {
            if (_contentInfo?.Content is null)
                return null;

            if (_contentInfo?.ContentType != "1.2.840.113549.1.7.2") //1.2.840.113549.1.7.2 = OID for signedData
            {
                return null;
            }

            var sd = new SignedData(_contentInfo.Content);
            if (sd.ContentInfo.ContentType != "1.3.6.1.4.1.311.2.1.4") // 1.3.6.1.4.1.311.2.1.4 = OID for Microsoft Crypto
            {
                return null;
            }

            var spc = sd.ContentInfo.Content;
            if (spc is null) return null;
            var signedHash = (Asn1OctetString)spc.Nodes[0].Nodes[1].Nodes[1];
            return signedHash.Data;
        }

        private string? GetSigningSerialNumber()
        {
            var asn1 = _contentInfo?.Content;
            if (asn1 is null) return null;
            var x = (Asn1Integer)asn1.Nodes[0].Nodes[4].Nodes[0].Nodes[1].Nodes[1]; // ASN.1 Path to signer serial number: /1/0/4/0/1/1
            return x.Value.ToHexString()[2..].ToUpper();
        }

        [Obsolete("Use ComputeAuthenticodeHashFromPeFile(HashAlgorithmName) instead.")]
        public IEnumerable<byte>? ComputeAuthenticodeHashFromPeFile(HashAlgorithm hash)
        {
            var algorithmName = hash switch
            {
                MD5 => HashAlgorithmName.MD5,
                SHA1 => HashAlgorithmName.SHA1,
                SHA256 => HashAlgorithmName.SHA256,
                SHA384 => HashAlgorithmName.SHA384,
                SHA512 => HashAlgorithmName.SHA512,
                _ => throw new ArgumentException($"Unsupported hash algorithm: {hash.GetType().Name}", nameof(hash))
            };

            return ComputeAuthenticodeHashFromPeFile(algorithmName);
        }

        public byte[]? ComputeAuthenticodeHashFromPeFile(HashAlgorithmName algorithmName)
        {
            using var hash = IncrementalHash.CreateHash(algorithmName);
            var rawFile = _peFile.RawFile;

            // 3.  Hash the image header from its base to immediately before the start of the checksum address, 
            // as specified in Optional Header Windows-Specific Fields.
            var offset = (_peFile.ImageNtHeaders?.OptionalHeader.Offset ?? 0L) + 0x40;
            hash.AppendData(rawFile.AsSpan(0, offset));

            // 4.  Skip over the checksum, which is a 4-byte field.
            offset += 0x4;

            // 6.  Get the Attribute Certificate Table address and size from the Certificate Table entry. 
            // For details, see section 5.7 of the PE/COFF specification.
            var certificateTable = _peFile.ImageNtHeaders?.OptionalHeader.DataDirectory[4];

            // 5.  Hash everything from the end of the checksum field to immediately before the start of the Certificate Table entry,
            // as specified in Optional Header Data Directories.
            var length = (certificateTable?.Offset ?? 0L) - offset;
            hash.AppendData(rawFile.AsSpan(offset, length));
            offset += length + 0x8; // end of Attribute Certificate Table address

            // 7.  Exclude the Certificate Table entry from the calculation and 
            // hash everything from the end of the Certificate Table entry to the end of image header, 
            // including Section Table (headers). The Certificate Table entry is 8 bytes long, as specified in Optional Header Data Directories.
            length = (_peFile.ImageNtHeaders?.OptionalHeader.SizeOfHeaders ?? 0L) - offset;
            hash.AppendData(rawFile.AsSpan(offset, length));

            // 8-13. Hash everything between end of header and certificate
            offset = (_peFile.ImageNtHeaders?.OptionalHeader.SizeOfHeaders ?? 0L);

            if (_peFile.WinCertificate is not null)
            {
                length = (_peFile.WinCertificate?.Offset ?? 0L) - offset;
                hash.AppendData(rawFile.AsSpan(offset, length));

                // Move offset right beyond the Certificate Table
                offset += length + (certificateTable?.Size ?? 0L);
            }

            // 14. Create a value called FILE_SIZE, which is not part of the signature. 
            // Set this value to the image’s file size, acquired from the underlying file system. 
            // If FILE_SIZE is greater than SUM_OF_BYTES_HASHED, the file contains extra data that must be added to the hash. 
            // This data begins at the SUM_OF_BYTES_HASHED file offset, and its length is:
            // (File Size) – ((Size of AttributeCertificateTable) + SUM_OF_BYTES_HASHED)
            // Note: The size of Attribute Certificate Table is specified 
            // in the second ULONG value in the Certificate Table entry (32 bit: offset 132, 64 bit: offset 148) in Optional Header Data Directories.
            // 14. Hash everything from the end of the certificate to the end of the file.
            var fileSize = rawFile.Length;
            if (fileSize > offset)
            {
                length = fileSize - offset;
                if (length != 0)
                {
                    hash.AppendData(rawFile.AsSpan(offset, length));
                }
            }

            // 15. Finalize the hash algorithm context.
            return hash.GetHashAndReset();
        }
    }
}