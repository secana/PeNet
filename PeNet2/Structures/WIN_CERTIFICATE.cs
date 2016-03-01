/***********************************************************************
Copyright 2016 Stefan Hausotte

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*************************************************************************/

using System;
using System.Text;

namespace PeNet.Structures
{
    /// <summary>
    ///     The WIN_CERTIFICATE the information
    ///     in the security directory of the PE file.
    ///     It contains information about any certificates
    ///     used to sign the binary.
    /// </summary>
    public class WIN_CERTIFICATE
    {
        private readonly byte[] _buff;
        private readonly uint _offset;

        /// <summary>
        ///     Create a new WIN_CERTIFICATE object.
        /// </summary>
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset to the structure.</param>
        public WIN_CERTIFICATE(byte[] buff, uint offset)
        {
            _buff = buff;
            _offset = offset;
        }

        /// <summary>
        ///     Length of the certificate.
        /// </summary>
        public uint dwLength
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        /// <summary>
        ///     Revision.
        /// </summary>
        public ushort wRevision
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x4); }
            set { Utility.SetUInt16(value, _offset + 0x4, _buff); }
        }

        /// <summary>
        ///     The certificate type.
        /// </summary>
        public ushort wCertificateType
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x6); }
            set { Utility.SetUInt16(value, _offset + 0x6, _buff); }
        }

        /// <summary>
        ///     The certificate as a byte array.
        /// </summary>
        public byte[] bCertificate
        {
            get
            {
                var cert = new byte[dwLength - 8];
                Array.Copy(_buff, _offset + 0x8, cert, 0, dwLength - 8);
                return cert;
            }
            set { Array.Copy(value, 0, _buff, _offset + 0x8, value.Length); }
        }

        /// <summary>
        ///     Create a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>The WIN_CERTIFICATE properties as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("WIN_CERTIFICATE\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));

            return sb.ToString();
        }
    }
}