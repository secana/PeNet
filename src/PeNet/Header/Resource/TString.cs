using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    /// Contains a string that describes specifics of a file. For example
    /// the version, copyright information or original file name.
    /// </summary>
    public class TString : AbstractStructure
    {
        /// <summary>
        /// Create a new TString structure.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of a String structure in the PE file.</param>
        public TString(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// Length of the String structure in bytes.
        /// </summary>
        public ushort WLength
        {
            get => PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, value);
        }

        /// <summary>
        /// Size of the Value member in words.
        /// </summary>
        public ushort WValueLength
        {
            get => PeFile.ReadUShort(Offset + 0x2);
            set => PeFile.WriteUShort(Offset + 0x2, value);
        }

        /// <summary>
        /// Type of the data in the version resource. Contains a 1 if the data
        /// is text data and a 0 if it contains binary data.
        /// </summary>
        public ushort WType
        {
            get => PeFile.ReadUShort(Offset + 0x4);
            set => PeFile.WriteUShort(Offset + 0x4, value);
        }

        /// <summary>
        /// Arbitrary Unicode string, which describes the kind of
        /// the following data, e.g. "Comments", "CompanyName" and so
        /// on.
        /// </summary>
        public string SzKey => PeFile.ReadUnicodeString(Offset + 0x6);

        /// <summary>
        /// Arbitrary string which contains the information for the
        /// szKey member.
        /// </summary>
        public string Value {
            get
            {
                var currentOffset = Offset + 0x6 + SzKey.UStringByteLength() +
                                    (Offset + 0x6 + SzKey.UStringByteLength()).PaddingBytes(32);

                return PeFile.ReadUnicodeString(currentOffset);
            }
        }
    }
}