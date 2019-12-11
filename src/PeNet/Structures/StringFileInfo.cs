using System.Collections.Generic;
using System.Linq;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Describes the data representation in a file-version resource.
    /// Information about different languages and codes pages is contained in
    /// this structure.
    /// </summary>
    public class StringFileInfo : AbstractStructure
    {
        private StringTable[] _stringTable;

        public StringFileInfo(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        /// <summary>
        /// Length of the StringFileInfo in bytes, including all children.
        /// </summary>
        public ushort wLength
        {
            get => Buff.BytesToUInt16(Offset);
            set => Buff.SetUInt16(Offset, value);
        }

        /// <summary>
        /// Always zero.
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
        /// Contains the Unicode string "StringFileInfo".
        /// </summary>
        public string szKey => Buff.GetUnicodeString(Offset + 0x6);

        /// <summary>
        /// One ore more StringTable structures, where each tables szKey indicates
        /// the language and code page for displaying the text in the StringTable.
        /// </summary>
        public StringTable[] StringTable {
            get
            {
                _stringTable ??= ReadChildren();
                return _stringTable;
            }
        }

        private StringTable[] ReadChildren()
        {
            var currentOffset =
                Offset + 6 + szKey.LengthInByte() 
                + (Offset + 6 + szKey.LengthInByte()).PaddingBytes(32);

            var children = new List<StringTable>();

            while (currentOffset < Offset + wLength)
            {
                children.Add(new StringTable(Buff, (uint) currentOffset));
                currentOffset += children.Last().wLength;
            }

            return children.ToArray();
        }
    }
}