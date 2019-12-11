using System.Collections.Generic;
using System.Linq;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Contains a string that describes specifics of a file. For example
    /// the version, copyright information or original file name.
    /// </summary>
    public class TString : AbstractStructure
    {
        public TString(byte[] buff, uint offset) : base(buff, offset)
        {
            Value = GetValues();
        }

        /// <summary>
        /// Length of the String structure in bytes.
        /// </summary>
        public ushort wLength
        {
            get => Buff.BytesToUInt16(Offset);
            set => Buff.SetUInt16(Offset, value);
        }

        /// <summary>
        /// Size of the Value member in words.
        /// </summary>
        public ushort wValueLength
        {
            get => Buff.BytesToUInt16(Offset + 0x2);
            set => Buff.SetUInt16(Offset + 0x2, value);
        }

        /// <summary>
        /// Type of the data in the version resource. Contains a 1 if the data
        /// is text data and a 0 if it contains binary data.
        /// </summary>
        public ushort wType
        {
            get => Buff.BytesToUInt16(Offset + 0x4);
            set => Buff.SetUInt16(Offset + 0x4, value);
        }

        /// <summary>
        /// Arbitrary Unicode string, which describes the kind of
        /// the following data, e.g. "Comments", "CompanyName" and so
        /// on.
        /// </summary>
        public string szKey => Buff.GetUnicodeString(Offset + 0x6);

        /// <summary>
        /// Arbitrary string which contains the information for the
        /// szKey member.
        /// </summary>
        public string[] Value { get; }

        private string[] GetValues()
        {
            var currentOffset = Offset + 0x6 + szKey.LengthInByte() +
                                (Offset + 0x6 + szKey.LengthInByte()).PaddingBytes(32);

            var values = new List<string>();

            while (currentOffset < Offset + 6 + szKey.LengthInByte() + wValueLength)
            {
                values.Add(Buff.GetUnicodeString((ulong) currentOffset));
                currentOffset += values.Last().LengthInByte();
            }

            return values.ToArray();
        } 
    }
}