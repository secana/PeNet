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
