//
// ASN1.cs: Abstract Syntax Notation 1 - micro-parser and generator
//
// Authors:
//	Sebastien Pouliot  <sebastien@ximian.com>
//	Jesper Pedersen  <jep@itplus.dk>
//
// (C) 2002, 2003 Motus Technologies Inc. (http://www.motus.com)
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
// (C) 2004 IT+ A/S (http://www.itplus.dk)
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
// https://github.com/mono/mono/blob/0bcbe39b148bb498742fc68416f8293ccd350fb6/mcs/class/Mono.Security/Mono.Security/ASN1.cs

using System;
using System.Collections;
using System.Linq;

namespace PeNet.Authenticode
{
    // References:
    // a.	ITU ASN.1 standards (free download)
    //	http://www.itu.int/ITU-T/studygroups/com17/languages/

    public sealed class ASN1
    {
        private byte[] m_aValue;
        private ArrayList elist;

        private ASN1(byte tag, byte[] data)
        {
            Tag = tag;
            m_aValue = data;
        }

        public ASN1(byte[] data)
        {
            Tag = data[0];

            var nLenLength = 0;
            int nLength = data[1];

            if (nLength > 0x80)
            {
                // composed length
                nLenLength = nLength - 0x80;
                nLength = 0;
                for (var i = 0; i < nLenLength; i++)
                {
                    nLength *= 256;
                    nLength += data[i + 2];
                }
            }
            else if (nLength == 0x80)
            {
                // undefined length encoding
                throw new NotSupportedException("Undefined length encoding.");
            }

            m_aValue = new byte [nLength];
            Buffer.BlockCopy(data, (2 + nLenLength), m_aValue, 0, nLength);

            if ((Tag & 0x20) != 0x20) return;
            var nStart = (2 + nLenLength);
            Decode(data, ref nStart, data.Length);
        }

        public int Count => elist?.Count ?? 0;

        public byte Tag { get; }

        public int Length => m_aValue?.Length ?? 0;

        public byte[] Value
        {
            get
            {
                if (m_aValue == null)
                    GetBytes();
                return (byte[]) m_aValue.Clone();
            }
        }

        private bool CompareArray(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length) return false;
            return !array1.Where((t, i) => t != array2[i]).Any();
        }

        public bool CompareValue(byte[] value)
        {
            return CompareArray(m_aValue, value);
        }

        private ASN1 Add(ASN1 asn1)
        {
            if (asn1 == null) return null;
            if (elist == null)
                elist = new ArrayList();
            elist.Add(asn1);
            return asn1;
        }

        private byte[] GetBytes()
        {
            byte[] val = null;

            if (Count > 0)
            {
                var esize = 0;
                var al = new ArrayList();
                foreach (ASN1 a in elist)
                {
                    var item = a.GetBytes();
                    al.Add(item);
                    esize += item.Length;
                }
                val = new byte [esize];
                var pos = 0;
                for (var i = 0; i < elist.Count; i++)
                {
                    var item = (byte[]) al[i];
                    Buffer.BlockCopy(item, 0, val, pos, item.Length);
                    pos += item.Length;
                }
            }
            else if (m_aValue != null)
            {
                val = m_aValue;
            }

            byte[] der;
            var nLengthLen = 0;

            if (val != null)
            {
                var nLength = val.Length;
                // special for length > 127
                if (nLength > 127)
                {
                    if (nLength <= Byte.MaxValue)
                    {
                        der = new byte [3 + nLength];
                        Buffer.BlockCopy(val, 0, der, 3, nLength);
                        nLengthLen = 0x81;
                        der[2] = (byte) (nLength);
                    }
                    else if (nLength <= UInt16.MaxValue)
                    {
                        der = new byte [4 + nLength];
                        Buffer.BlockCopy(val, 0, der, 4, nLength);
                        nLengthLen = 0x82;
                        der[2] = (byte) (nLength >> 8);
                        der[3] = (byte) (nLength);
                    }
                    else if (nLength <= 0xFFFFFF)
                    {
                        // 24 bits
                        der = new byte [5 + nLength];
                        Buffer.BlockCopy(val, 0, der, 5, nLength);
                        nLengthLen = 0x83;
                        der[2] = (byte) (nLength >> 16);
                        der[3] = (byte) (nLength >> 8);
                        der[4] = (byte) (nLength);
                    }
                    else
                    {
                        // max (Length is an integer) 32 bits
                        der = new byte [6 + nLength];
                        Buffer.BlockCopy(val, 0, der, 6, nLength);
                        nLengthLen = 0x84;
                        der[2] = (byte) (nLength >> 24);
                        der[3] = (byte) (nLength >> 16);
                        der[4] = (byte) (nLength >> 8);
                        der[5] = (byte) (nLength);
                    }
                }
                else
                {
                    // basic case (no encoding)
                    der = new byte [2 + nLength];
                    Buffer.BlockCopy(val, 0, der, 2, nLength);
                    nLengthLen = nLength;
                }
                if (m_aValue == null)
                    m_aValue = val;
            }
            else
                der = new byte[2];

            der[0] = Tag;
            der[1] = (byte) nLengthLen;

            return der;
        }

        // Note: Recursive
        private void Decode(byte[] asn1, ref int anPos, int anLength)
        {
            // minimum is 2 bytes (tag + length of 0)
            while (anPos < anLength - 1)
            {
                DecodeTLV(asn1, ref anPos, out var nTag, out var nLength, out var aValue);
                // sometimes we get trailing 0
                if (nTag == 0)
                    continue;

                var elm = Add(new ASN1(nTag, aValue));

                if ((nTag & 0x20) == 0x20)
                {
                    var nConstructedPos = anPos;
                    elm.Decode(asn1, ref nConstructedPos, nConstructedPos + nLength);
                }
                anPos += nLength; // value length
            }
        }

        // TLV : Tag - Length - Value
        private void DecodeTLV(byte[] asn1, ref int pos, out byte tag, out int length, out byte[] content)
        {
            tag = asn1[pos++];
            length = asn1[pos++];

            // special case where L contains the Length of the Length + 0x80
            if ((length & 0x80) == 0x80)
            {
                var nLengthLen = length & 0x7F;
                length = 0;
                for (var i = 0; i < nLengthLen; i++)
                    length = length * 256 + asn1[pos++];
            }

            content = new byte [length];
            Buffer.BlockCopy(asn1, pos, content, 0, length);
        }

        public ASN1 this[int index]
        {
            get
            {
                try
                {
                    if ((elist == null) || (index >= elist.Count))
                        return null;
                    return (ASN1) elist[index];
                }
                catch (ArgumentOutOfRangeException)
                {
                    return null;
                }
            }
        }
    }
}