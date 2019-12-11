using System.Collections.Generic;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Usually a list of languages that the
    /// application or DLL supports.
    /// </summary>
    public class Var : AbstractStructure
    {
        private uint[] _value;

        public Var(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        /// <summary>
        /// Length of the Var structure in bytes.
        /// </summary>
        public ushort wLength
        {
            get => Buff.BytesToUInt16(Offset);
            set => Buff.SetUInt16(Offset, value);
        }

        /// <summary>
        /// Length of the Value member in bytes.
        /// </summary>
        public ushort wValueLength
        {
            get => Buff.BytesToUInt16(Offset + 0x2);
            set => Buff.SetUInt16(Offset + 0x2, value);
        }

        /// <summary>
        /// Contains a 1 if the version resource is text and a
        /// 0 if the version resource is binary data.
        /// </summary>
        public ushort wType
        {
            get => Buff.BytesToUInt16(Offset + 0x4);
            set => Buff.SetUInt16(Offset + 04, value);
        }

        /// <summary>
        /// Unicode string "Translation"
        /// </summary>
        public string szKey => Buff.GetUnicodeString(Offset + 0x6);

        /// <summary>
        /// DWORD value where the lower-order word contains the Microsoft
        /// language identifier and the high-order word the IBM code page number.
        /// Both values can be zero, indicating that the file is code page
        /// or language independent.
        /// </summary>
        public uint[] Value {
            get
            {
                _value ??= ReadValues();
                return _value;
            }
        }

        private uint[] ReadValues()
        {
            var currentOffset =
                Offset + 6 + szKey.LengthInByte() 
                + (Offset + 6 + szKey.LengthInByte()).PaddingBytes(32);

            var startOfValues = currentOffset;

            var values = new List<uint>();

            while (currentOffset < startOfValues + wValueLength)
            {
                values.Add(Buff.BytesToUInt32((uint) currentOffset));
                currentOffset += sizeof(uint);
            }

            return values.ToArray();
        }
    }
}