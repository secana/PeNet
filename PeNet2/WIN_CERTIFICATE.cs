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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public class WIN_CERTIFICATE
    {
        byte[] _buff;
        UInt32 _offset;

        public UInt32 dwLength
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        public UInt16 wRevision
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x4); }
            set { Utility.SetUInt16(value, _offset + 0x4, _buff); }
        }

        public UInt16 wCertificateType
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x6); }
            set { Utility.SetUInt16(value, _offset + 0x6, _buff); }
        }

        public byte[] bCertificate
        {
            get
            {
                var cert = new byte[dwLength-8];
                Array.Copy(_buff, _offset + 0x8, cert, 0, dwLength - 8);
                return cert;
            }
            set
            {
                Array.Copy(value, 0, _buff, _offset + 0x8, value.Length);
            }
        }

        public WIN_CERTIFICATE(byte[] buff, UInt32 offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("WIN_CERTIFICATE\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));

            return sb.ToString();
        }
    }
}
