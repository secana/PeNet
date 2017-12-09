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
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace PeNet.Authenticode

    // References:
    // a.	http://www.cs.auckland.ac.nz/~pgut001/pubs/authenticode.txt
{
    public static class Authenticode
    {
        public static bool CheckSignature(PeFile peFile)
        {
            var signedHash = GetSignedHash(peFile);
            if (signedHash == null) return false;
            HashAlgorithm ha;
            switch (signedHash.Length)
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
            var firstBlockData = ProcessFirstBlock(peFile);
            if (firstBlockData == null) return false;
            var hash = GetHash(ha, peFile, firstBlockData);
            return signedHash.SequenceEqual(hash);
        }

        private static byte[] GetSignedHash(PeFile file)
        {
            var cert = file.WinCertificate;
            var ci = new X509AuthentiCodeInfo.ContentInfo(cert.bCertificate);
            if (ci.ContentType != "1.2.840.113549.1.7.2")
            {
                return null;
            }

            var sd = new X509AuthentiCodeInfo.SignedData(ci.Content);
            if (sd.ContentInfo.ContentType != "1.3.6.1.4.1.311.2.1.4")
            {
                return null;
            }

            var spc = sd.ContentInfo.Content;
            var signedHash = spc[0][1][1];
            return signedHash.Value;
        }


        private static FirstBlockData ProcessFirstBlock(PeFile peFile)
        {
            peFile.Stream.Position = 0;
            var fileblock = new byte[4096];
            // read first block - it will include (100% sure) 
            // the MZ header and (99.9% sure) the PE header
            var blockLength = peFile.Stream.Read(fileblock, 0, fileblock.Length);
            if (blockLength < 64)
                return null; // invalid PE file

            // 1.2. Find the offset of the PE header
            var peOffset = peFile.ImageDosHeader.e_lfanew;
            if (peOffset > fileblock.Length)
            {
                // just in case (0.1%) this can actually happen
                throw new NotSupportedException($"Header size too big (> {fileblock.Length} bytes).");
            }
            if (peOffset > peFile.Stream.Length)
                return null;

            // 2. Read between DOS header and first part of PE header
            // 2.1. Check for magic PE at start of header
            //	PE - NT header ('P' 'E' 0x00 0x00)
            if (peFile.ImageNtHeaders.Signature != 0x4550)
                return null;

            return new FirstBlockData
            {
                BlockLength = blockLength,
                FileBlock = fileblock,
            };
        }

        private static IEnumerable<byte> GetHash(HashAlgorithm hash, PeFile peFile, FirstBlockData firstBlockData)
        {
            var blockLength = firstBlockData.BlockLength;
            // Locate IMAGE_DIRECTORY_ENTRY_SECURITY (offset)
            var dirSecurityOffset = (int) peFile.WinCertificate.Offset;
            // COFF symbol tables are deprecated - we'll strip them if we see them!
            // (otherwise the signature won't work on MS and we don't want to support COFF for that)
            var coffSymbolTableOffset = (int) peFile.ImageNtHeaders.FileHeader.PointerToSymbolTable;
            var peOffset = peFile.ImageDosHeader.e_lfanew;
            var fileblock = firstBlockData.FileBlock;

            peFile.Stream.Position = firstBlockData.BlockLength;

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
                addsize = (int) (peFile.Stream.Length & 7);
                if (addsize > 0)
                    addsize = 8 - addsize;

                n = peFile.Stream.Length - blockLength;
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
                    peFile.Stream.Read(fileblock, 0, fileblock.Length);
                    hash.TransformBlock(fileblock, 0, fileblock.Length, fileblock, 0);
                }
                // remainder
                if (peFile.Stream.Read(fileblock, 0, remainder) != remainder)
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