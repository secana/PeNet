//
// PKCS7.cs: PKCS #7 - Cryptographic Message Syntax Standard 
//	http://www.rsasecurity.com/rsalabs/pkcs/pkcs-7/index.html
//
// Authors:
//	Sebastien Pouliot <sebastien@ximian.com>
//	Daniel Granath <dgranath#gmail.com>
//
// (C) 2002, 2003 Motus Technologies Inc. (http://www.motus.com)
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
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
//
// https://github.com/mono/mono/blob/0bcbe39b148bb498742fc68416f8293ccd350fb6/mcs/class/Mono.Security/Mono.Security/PKCS7.cs

using System;

namespace PeNet.Authenticode
{
    class X509AuthentiCodeInfo
    {
        public class ContentInfo
        {
            public ASN1   Content     { get; }
            public string ContentType { get; }

            public ContentInfo(byte[] data)
                : this(new ASN1(data)) {}

            public ContentInfo(ASN1 asn1)
            {
                // SEQUENCE with 1 or 2 elements
                if ((asn1.Tag != 0x30) || ((asn1.Count < 1) && (asn1.Count > 2)))
                    throw new ArgumentException("Invalid ASN1");
                if (asn1[0].Tag != 0x06)
                    throw new ArgumentException("Invalid contentType");
                ContentType = ASN1Convert.ToOid(asn1[0]);
                if (asn1.Count <= 1) return;
                if (asn1[1].Tag != 0xA0)
                    throw new ArgumentException("Invalid content");
                Content = asn1[1];
            }
        }

        public class SignedData
        {
            public SignedData(ASN1 asn1)
            {
                if ((asn1[0].Tag != 0x30) || (asn1[0].Count < 4))
                    throw new ArgumentException("Invalid SignedData");

                if (asn1[0][0].Tag != 0x02)
                    throw new ArgumentException("Invalid version");

                ContentInfo = new ContentInfo(asn1[0][2]);
            }

            public ContentInfo ContentInfo { get; }
        }
    }
}