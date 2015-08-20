using System;
using System.Text;

namespace PeNet
{
    public class IMAGE_FILE_HEADER
    {
        UInt32 _offset;
        byte[] _buff;

        /// <summary>
        /// I386: 0x014c
        /// ARMv7: 0x1c4
        /// AMD64: 0x8664
        /// </summary>
        public UInt16 Machine
        {
            get
            {
                return Utility.BytesToUInt16(_buff, _offset);
            }
            set
            {
                Utility.SetUInt16(value, _offset, _buff);
            }
        }

        public UInt16 NumberOfSections
        {
            get
            {
                return Utility.BytesToUInt16(_buff, _offset + 0x2);
            }
            set
            {
                Utility.SetUInt16(value, _offset + 0x2, _buff);
            }
        }

        public UInt32 TimeDateStamp
        {
            get
            {
                return Utility.BytesToUInt32(_buff, _offset + 0x4);
            }
            set
            {
                Utility.SetUInt32(value, _offset + 0x4, _buff);
            }
        }

        public UInt32 PointerToSymbolTable
        {
            get
            {
                return Utility.BytesToUInt32(_buff, _offset + 0x8);
            }
            set
            {
                Utility.SetUInt32(value, _offset + 0x8, _buff);
            }
        }

        public UInt32 NumberOfSymbols
        {
            get
            {
                return Utility.BytesToUInt32(_buff, _offset + 0xC);
            }
            set
            {
                Utility.SetUInt32(value, _offset + 0xC, _buff);
            }
        }

        public UInt16 SizeOfOptionalHeader
        {
            get
            {
                return Utility.BytesToUInt16(_buff, _offset + 0x10);
            }
            set
            {
                Utility.SetUInt16(value, _offset + 0x10, _buff);
            }
        }

        public UInt16 Characteristics
        {
            get
            {
                return Utility.BytesToUInt16(_buff, _offset + 0x12);
            }
            set
            {
                Utility.SetUInt16(value, _offset + 0x12, _buff);
            }
        }

        public IMAGE_FILE_HEADER(byte[] buff, UInt32 offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_FILE_HEADER\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-15}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
