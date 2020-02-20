using System.Collections.Generic;
using System.IO;
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
        private StringTable[]? _stringTable;

        /// <summary>
        /// Create a new StringFileInfo instance.
        /// </summary>
        /// <param name="peFile">Stream containing a PE file.</param>
        /// <param name="offset">Offset of a StringFileInfo structure in the stream.</param>
        public StringFileInfo(Stream peFile, long offset) 
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// Length of the StringFileInfo in bytes, including all children.
        /// </summary>
        public ushort wLength
        {
            get => PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, value);
        }

        /// <summary>
        /// Always zero.
        /// </summary>
        public ushort wValueLength
        {
            get => PeFile.ReadUShort(Offset + 0x2);
            set => PeFile.WriteUShort(Offset + 0x2, value);
        }

        /// <summary>
        /// Type of the data in the version resource. Contains a 1 if the data
        /// is text data and a 0 if it contains binary data.
        /// </summary>
        public ushort wType
        {
            get => PeFile.ReadUShort(Offset + 0x4);
            set => PeFile.WriteUShort(Offset + 0x4, value);
        }

        /// <summary>
        /// Contains the Unicode string "StringFileInfo".
        /// </summary>
        public string szKey => PeFile.GetUnicodeString(Offset + 0x6);

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
                children.Add(new StringTable(PeFile, currentOffset));
                currentOffset += children.Last().wLength;
            }

            return children.ToArray();
        }
    }
}