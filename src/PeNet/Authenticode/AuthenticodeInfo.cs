//
// AuthenticodeDeformatter.cs: Authenticode signature validator
//
// Author:
//	Sebastien Pouliot <sebastien@ximian.com>
//
// (C) 2003 Motus Technologies Inc. (http://www.motus.com)
// Copyright (C) 2004-2006 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// https://github.com/mono/mono/blob/0bcbe39b148bb498742fc68416f8293ccd350fb6/mcs/class/Mono.Security/Mono.Security.Authenticode/AuthenticodeDeformatter.cs
// https://github.com/mono/mono/blob/0bcbe39b148bb498742fc68416f8293ccd350fb6/mcs/class/Mono.Security/Mono.Security.Authenticode/AuthenticodeBase.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using PeNet.Utilities;

namespace PeNet.Authenticode

    // References:
    // a.	http://www.cs.auckland.ac.nz/~pgut001/pubs/authenticode.txt
{
    public class AuthenticodeInfo
    {
        private readonly PeFile _peFile;
        private readonly X509AuthentiCodeInfo.ContentInfo _contentInfo;

        public string SignerSerialNumber { get; }
        public byte[] SignedHash { get; }
        public bool IsAuthenticodeValid { get; }
        public X509Certificate2 SigningCertificate { get; }

        public AuthenticodeInfo(PeFile peFile)
        {
            _peFile = peFile;
            _contentInfo = new X509AuthentiCodeInfo.ContentInfo(_peFile.WinCertificate.bCertificate);
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

            #if NET461
            return new X509Certificate2(pkcs7);
            #else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new X509Certificate2(pkcs7);
            }
            else
            {
                return GetSigningCertificateNonWindows(_peFile);
            }
            #endif
        }

        private X509Certificate2 GetSigningCertificateNonWindows(PeFile peFile)
        {
            var collection = new X509Certificate2Collection();
            collection.Import(peFile.WinCertificate.bCertificate);
            return collection.Cast<X509Certificate2>().FirstOrDefault(cert => string.Equals(cert.SerialNumber, SignerSerialNumber, StringComparison.CurrentCultureIgnoreCase));
        }

        private bool CheckSignature()
        {
            if (SignedHash == null) return false;
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
            var firstBlockData = ProcessFirstBlock();
            if (firstBlockData == null) return false;
            var hash = GetHash(ha, firstBlockData);
            return SignedHash.SequenceEqual(hash);
        }

        private byte[] GetSignedHash()
        {
            if (_contentInfo.ContentType != "1.2.840.113549.1.7.2") //1.2.840.113549.1.7.2 = OID for signedData
            {
                return null;
            }

            var sd = new X509AuthentiCodeInfo.SignedData(_contentInfo.Content);
            if (sd.ContentInfo.ContentType != "1.3.6.1.4.1.311.2.1.4") // 1.3.6.1.4.1.311.2.1.4 = OID for Microsoft Crypto
            {
                return null;
            }

            var spc = sd.ContentInfo.Content;
            var signedHash = spc[0][1][1];
            return signedHash.Value;
        }

        private string GetSigningSerialNumber()
        {
            var asn1 = _contentInfo.Content;
            var x = asn1[0][4][0][1][1].Value; // ASN.1 Path to signer serial number: /1/0/4/0/1/1
            return x.ToHexString().Substring(2).ToUpper();
        }


        private FirstBlockData ProcessFirstBlock()
        {
            _peFile.Stream.Position = 0;
            var fileblock = new byte[4096];
            // read first block - it will include (100% sure) 
            // the MZ header and (99.9% sure) the PE header
            var blockLength = _peFile.Stream.Read(fileblock, 0, fileblock.Length);
            if (blockLength < 64)
                return null; // invalid PE file

            // 1.2. Find the offset of the PE header
            var peOffset = _peFile.ImageDosHeader.e_lfanew;
            if (peOffset > fileblock.Length)
            {
                // just in case (0.1%) this can actually happen
                throw new NotSupportedException($"Header size too big (> {fileblock.Length} bytes).");
            }
            if (peOffset > _peFile.Stream.Length)
                return null;

            // 2. Read between DOS header and first part of PE header
            // 2.1. Check for magic PE at start of header
            //	PE - NT header ('P' 'E' 0x00 0x00)
            if (_peFile.ImageNtHeaders.Signature != 0x4550)
                return null;

            return new FirstBlockData
            {
                BlockLength = blockLength,
                FileBlock = fileblock,
            };
        }

        private IEnumerable<byte> GetHash(HashAlgorithm hash, FirstBlockData firstBlockData)
        {
            var blockLength = firstBlockData.BlockLength;
            // Locate IMAGE_DIRECTORY_ENTRY_SECURITY (offset)
            var dirSecurityOffset = (int) _peFile.WinCertificate.Offset;
            // COFF symbol tables are deprecated - we'll strip them if we see them!
            // (otherwise the signature won't work on MS and we don't want to support COFF for that)
            var coffSymbolTableOffset = (int) _peFile.ImageNtHeaders.FileHeader.PointerToSymbolTable;
            var peOffset = _peFile.ImageDosHeader.e_lfanew;
            var fileblock = firstBlockData.FileBlock;

            _peFile.Stream.Position = firstBlockData.BlockLength;

            // hash the rest of the file
            long n;
            var addsize = 0;
            // minus any authenticode signature (with 8 bytes header)
            if (dirSecurityOffset > 0)
            {
                // it is also possible that the signature block 
                // starts within the block in memory (small EXE)
                if (dirSecurityOffset < blockLength)
                {
                    blockLength = dirSecurityOffset;
                    n = 0;
                }
                else
                {
                    n = dirSecurityOffset - blockLength;
                }
            }
            else if (coffSymbolTableOffset > 0)
            {
                fileblock[peOffset + 12] = 0;
                fileblock[peOffset + 13] = 0;
                fileblock[peOffset + 14] = 0;
                fileblock[peOffset + 15] = 0;
                fileblock[peOffset + 16] = 0;
                fileblock[peOffset + 17] = 0;
                fileblock[peOffset + 18] = 0;
                fileblock[peOffset + 19] = 0;
                // it is also possible that the signature block 
                // starts within the block in memory (small EXE)
                if (coffSymbolTableOffset < blockLength)
                {
                    blockLength = coffSymbolTableOffset;
                    n = 0;
                }
                else
                {
                    n = coffSymbolTableOffset - blockLength;
                }
            }
            else
            {
                addsize = (int) (_peFile.Stream.Length & 7);
                if (addsize > 0)
                    addsize = 8 - addsize;

                n = _peFile.Stream.Length - blockLength;
            }

            // Authenticode(r) gymnastics
            // Hash from (generally) 0 to 215 (216 bytes)
            var pe = (int) peOffset + 88;
            hash.TransformBlock(fileblock, 0, pe, fileblock, 0);
            // then skip 4 for checksum
            pe += 4;
            // Continue hashing from (generally) 220 to 279 (60 bytes)
            hash.TransformBlock(fileblock, pe, 60, fileblock, pe);
            // then skip 8 bytes for IMAGE_DIRECTORY_ENTRY_SECURITY
            pe += 68;

            // everything is present so start the hashing
            if (n == 0)
            {
                // hash the (only) block
                hash.TransformFinalBlock(fileblock, pe, blockLength - pe);
            }
            else
            {
                // hash the last part of the first (already in memory) block
                hash.TransformBlock(fileblock, pe, blockLength - pe, fileblock, pe);

                // hash by blocks of 4096 bytes
                var blocks = n >> 12;
                var remainder = (int) (n - (blocks << 12));
                if (remainder == 0)
                {
                    blocks--;
                    remainder = 4096;
                }
                // blocks
                while (blocks-- > 0)
                {
                    _peFile.Stream.Read(fileblock, 0, fileblock.Length);
                    hash.TransformBlock(fileblock, 0, fileblock.Length, fileblock, 0);
                }
                // remainder
                if (_peFile.Stream.Read(fileblock, 0, remainder) != remainder)
                    return null;

                if (addsize > 0)
                {
                    hash.TransformBlock(fileblock, 0, remainder, fileblock, 0);
                    hash.TransformFinalBlock(new byte [addsize], 0, addsize);
                }
                else
                {
                    hash.TransformFinalBlock(fileblock, 0, remainder);
                }
            }
            return hash.Hash;
        }
    }
}