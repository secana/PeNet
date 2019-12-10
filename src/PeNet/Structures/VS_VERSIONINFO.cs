using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Optional Resource member which contains version information
    /// and compile time information about the PE file.
    /// </summary>
    public class VS_VERSIONINFO : AbstractStructure
    {
        /// <summary>
        /// Length of the VS_VERSIONINFO structure without any padding.
        /// </summary>
        public ushort wLength
        {
            get => Buff.BytesToUInt16(Offset);
            set => Buff.SetUInt16(Offset, value);
        }

        /// <summary>
        /// Length of the Value member. If the Value member is 0, there is no value member available
        /// in the VS_VERSIONINFO structure.
        /// </summary>
        public ushort wValueLength
        {
            get => Buff.BytesToUInt16(Offset + 0x2);
            set => Buff.SetUInt16(Offset + 0x2, value);
        }

        /// <summary>
        /// Set to 0 if the version resource contains text and 0 if the version
        /// resource contains binary data.
        /// </summary>
        public ushort wType
        {
            get => Buff.BytesToUInt16(Offset + 0x4);
            set => Buff.SetUInt16(Offset + 0x4, value);
        }

        /// <summary>
        /// Contains the Unicode string "VS_VERSION_INFO"
        /// </summary>
        public string szKey
        {
            get => Buff.GetUnicodeString(Offset + 0x6);
        }

        /// <summary>
        /// Aligns the Value member to 32 bit
        /// </summary>
        public ushort Padding1
        {
            get => Buff.BytesToUInt16(Offset + 0x26);
            set => Buff.SetUInt16(Offset + 0x26, value);
        }

        /// <summary>
        /// Language and code page independent version information about the PE file.
        /// </summary>
        public VS_FIXEDFILEINFO VsFixedFileInfo { get; }

        /// <summary>
        /// Optional StringFileInfo contained in the Children member.
        /// </summary>
        public StringFileInfo StringFileInfo { get; }

        public VS_VERSIONINFO(byte[] buff, uint offset) 
            : base(buff, offset)
        {
            VsFixedFileInfo = new VS_FIXEDFILEINFO(buff, offset + 0x28);
            StringFileInfo = new StringFileInfo(buff, offset + 0x5C);
        }
    }
}