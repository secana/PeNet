using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    /// Optional Resource member which contains version information
    /// and compile time information about the PE file.
    /// </summary>
    public class VsVersionInfo : AbstractStructure
    {
        private VsFixedFileInfo? _vsFixedFileInfo;
        private StringFileInfo? _stringFileInfo;
        private VarFileInfo? _varFileInfo;
        private uint VsFixedFileInfoOffset =>
            (uint)(Offset + 6 + SzKey.UStringByteLength()
                   + (Offset + 6 + SzKey.UStringByteLength()).PaddingBytes(32));

        /// <summary>
        /// Length of the VsVersionInfo structure without any padding.
        /// </summary>
        public ushort WLength
        {
            get => PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, value);
        }

        /// <summary>
        /// Length of the Value member. If the Value member is 0, there is no value member available
        /// in the VsVersionInfo structure.
        /// </summary>
        public ushort WValueLength
        {
            get => PeFile.ReadUShort(Offset + 0x2);
            set => PeFile.WriteUShort(Offset + 0x2, value);
        }

        /// <summary>
        /// Set to 0 if the version resource contains text and 0 if the version
        /// resource contains binary data.
        /// </summary>
        public ushort WType
        {
            get => PeFile.ReadUShort(Offset + 0x4);
            set => PeFile.WriteUShort(Offset + 0x4, value);
        }

        /// <summary>
        /// Contains the Unicode string "VS_VERSION_INFO"
        /// </summary>
        public string SzKey => PeFile.ReadUnicodeString(Offset + 0x6);

        /// <summary>
        /// Language and code page independent version information about the PE file.
        /// </summary>
        public VsFixedFileInfo VsFixedFileInfo {
            get
            {
                var currentOffset = VsFixedFileInfoOffset;

                _vsFixedFileInfo ??= new VsFixedFileInfo(PeFile, (int) currentOffset);

                return _vsFixedFileInfo;
            }
        }

        /// <summary>
        /// Optional StringFileInfo contained in the Children member.
        /// </summary>
        public StringFileInfo StringFileInfo {
            get
            {
                var isFirst = IsStringFileInfoFirstChild();

                var currentOffset = VsFixedFileInfoOffset;
                currentOffset += WValueLength;
                currentOffset += currentOffset.PaddingBytes(32);

                if (!isFirst)
                {
                    currentOffset += VarFileInfo.WLength;
                    currentOffset += currentOffset.PaddingBytes(32);
                }
                    

                _stringFileInfo ??= new StringFileInfo(PeFile, currentOffset);

                return _stringFileInfo;
            }
        }

        /// <summary>
        /// Optional VarFileInfo contained in the Children member.
        /// </summary>
        public VarFileInfo VarFileInfo {
            get
            {
                var notFirst = IsStringFileInfoFirstChild();

                var currentOffset = VsFixedFileInfoOffset;
                currentOffset += WValueLength;
                currentOffset += currentOffset.PaddingBytes(32);

                if (notFirst)
                {
                    currentOffset += StringFileInfo.WLength;
                    currentOffset += currentOffset.PaddingBytes(32);
                }

                _varFileInfo ??= new VarFileInfo(PeFile, currentOffset);

                return _varFileInfo;
            }
        }

        /// <summary>
        /// Create a new VsVersionInfo instance.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of the VsVersionInfo in the PE file.</param>
        public VsVersionInfo(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        private bool IsStringFileInfoFirstChild()
        {
            var currentOffset = VsFixedFileInfoOffset;
            currentOffset += WValueLength;
            currentOffset += currentOffset.PaddingBytes(32);
            currentOffset += 6;

            var readMarker = PeFile.ReadUnicodeString(currentOffset);

            return readMarker == "StringFileInfo";
        }
    }
}