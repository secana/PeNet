using PeNet.Utilities;
using System.IO;

namespace PeNet.Structures
{
    /// <summary>
    /// Optional Resource member which contains version information
    /// and compile time information about the PE file.
    /// </summary>
    public class VS_VERSIONINFO : AbstractStructure
    {
        private VS_FIXEDFILEINFO? _vsFixedFileInfo;
        private StringFileInfo? _stringFileInfo;
        private VarFileInfo? _varFileInfo;
        private uint VsFixedFileInfoOffset =>
            (uint)(Offset + 6 + szKey.LengthInByte()
                   + (Offset + 6 + szKey.LengthInByte()).PaddingBytes(32));

        /// <summary>
        /// Length of the VS_VERSIONINFO structure without any padding.
        /// </summary>
        public ushort wLength
        {
            get => PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, value);
        }

        /// <summary>
        /// Length of the Value member. If the Value member is 0, there is no value member available
        /// in the VS_VERSIONINFO structure.
        /// </summary>
        public ushort wValueLength
        {
            get => PeFile.ReadUShort(Offset + 0x2);
            set => PeFile.WriteUShort(Offset + 0x2, value);
        }

        /// <summary>
        /// Set to 0 if the version resource contains text and 0 if the version
        /// resource contains binary data.
        /// </summary>
        public ushort wType
        {
            get => PeFile.ReadUShort(Offset + 0x4);
            set => PeFile.WriteUShort(Offset + 0x4, value);
        }

        /// <summary>
        /// Contains the Unicode string "VS_VERSION_INFO"
        /// </summary>
        public string szKey => PeFile.GetUnicodeString(Offset + 0x6);

        /// <summary>
        /// Language and code page independent version information about the PE file.
        /// </summary>
        public VS_FIXEDFILEINFO VsFixedFileInfo {
            get
            {
                var currentOffset = VsFixedFileInfoOffset;

                _vsFixedFileInfo ??= new VS_FIXEDFILEINFO(PeFile, (int) currentOffset);

                return _vsFixedFileInfo;
            }
        }

        /// <summary>
        /// Optional StringFileInfo contained in the Children member.
        /// </summary>
        public StringFileInfo StringFileInfo {
            get
            {
                var currentOffset = VsFixedFileInfoOffset;
                currentOffset += wValueLength;
                currentOffset += currentOffset.PaddingBytes(32);

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
                var currentOffset = VsFixedFileInfoOffset;
                currentOffset += wValueLength;
                currentOffset += currentOffset.PaddingBytes(32);
                currentOffset += StringFileInfo.wLength;
                currentOffset += currentOffset.PaddingBytes(32);

                _varFileInfo ??= new VarFileInfo(PeFile, currentOffset);

                return _varFileInfo;
            }
        }

        /// <summary>
        /// Create a new VS_VERSIONINFO instance.
        /// </summary>
        /// <param name="peFile">Stream that contains a PE file.</param>
        /// <param name="offset">Offset of the VS_VERSIONINFO in the stream.</param>
        public VS_VERSIONINFO(Stream peFile, long offset) 
            : base(peFile, offset)
        {
        }
    }
}