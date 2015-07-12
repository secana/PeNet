using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public class IMAGE_IMPORT_BY_NAME
    {
        private byte[] _buff;
        private UInt64 _offset;

        public UInt16 Hint
        {
            get { return Utility.BytesToUInt16(_buff, _offset); }
            set { Utility.SetUInt16(value, _offset, _buff); }
        }

        public string Name
        {
            get { return Utility.GetName(_offset + 0x2, _buff); }
        }

        public IMAGE_IMPORT_BY_NAME(byte[] buff, UInt64 offset)
        {
            _offset = offset;
            _buff = buff;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_IMPORT_BY_NAME\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
