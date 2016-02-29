using System;
using System.Text;

namespace PeNet.Structures
{
    public class IMAGE_RESOURCE_DIR_STRING_U
    {
        private readonly byte[] _buff;
        private readonly uint _offset;

        public IMAGE_RESOURCE_DIR_STRING_U(byte[] buff, uint offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public ushort Length
        {
            get { return Utility.BytesToUInt16(_buff, _offset); }
            set { Utility.SetUInt16(value, _offset, _buff); }
        }

        public string NameString
        {
            get
            {
                var subarray = new byte[Length*2];
                Array.Copy(_buff, _offset + 2, subarray, 0, Length*2);

                return Encoding.Unicode.GetString(subarray);
            }
        }
    }
}