using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public class IMAGE_NT_HEADERS
    {
        UInt32 _offset;
        byte[] _buff;
        bool _is64Bit;

        public UInt32 Signature
        {
            get
            {
                return Utility.BytesToUInt32(_buff, _offset);
            }
            set
            {
                Utility.SetUInt32(value, _offset, _buff);
            }
        }

        public readonly IMAGE_FILE_HEADER FileHeader;
        public readonly IMAGE_OPTIONAL_HEADER OptionalHeader;
        
        public IMAGE_NT_HEADERS(byte[] buff, UInt32 offset, bool is64Bit)
        {
            _offset = offset;
            _buff = buff;
            _is64Bit = is64Bit;
            FileHeader = new IMAGE_FILE_HEADER(buff, offset + 0x4);
            OptionalHeader = new IMAGE_OPTIONAL_HEADER(buff, offset + 0x18, _is64Bit);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_NT_HEADERS\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));
            sb.Append(FileHeader.ToString());
            sb.Append(OptionalHeader.ToString());

            return sb.ToString();
        }
    }
}
