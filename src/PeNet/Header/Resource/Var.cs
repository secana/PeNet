using System.Collections.Generic;
using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    /// Usually a list of languages that the
    /// application or DLL supports.
    /// </summary>
    public class Var : AbstractStructure
    {
        private uint[]? _value;

        /// <summary>
        /// Create a new Var instance.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of a Var structure in the PE file.</param>
        public Var(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// Length of the Var structure in bytes.
        /// </summary>
        public ushort WLength
        {
            get => PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, value);
        }

        /// <summary>
        /// Length of the Value member in bytes.
        /// </summary>
        public ushort WValueLength
        {
            get => PeFile.ReadUShort(Offset + 0x2);
            set => PeFile.WriteUShort(Offset + 0x2, value);
        }

        /// <summary>
        /// Contains a 1 if the version resource is text and a
        /// 0 if the version resource is binary data.
        /// </summary>
        public ushort WType
        {
            get => PeFile.ReadUShort(Offset + 0x4);
            set => PeFile.WriteUShort(Offset + 04, value);
        }

        /// <summary>
        /// Unicode string "Translation"
        /// </summary>
        public string SzKey => PeFile.ReadUnicodeString(Offset + 0x6);

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
                Offset + 6 + SzKey.UStringByteLength() 
                + (Offset + 6 + SzKey.UStringByteLength()).PaddingBytes(32);

            var startOfValues = currentOffset;

            var values = new List<uint>();

            while (currentOffset < startOfValues + WValueLength)
            {
                values.Add(PeFile.ReadUInt(currentOffset));
                currentOffset += sizeof(uint);
            }

            return values.ToArray();
        }
    }
}