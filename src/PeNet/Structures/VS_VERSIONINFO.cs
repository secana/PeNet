using System.Collections.Generic;
using System.Linq;
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
        public string szKey => Buff.GetUnicodeString(Offset + 0x6);

        /// <summary>
        /// Language and code page independent version information about the PE file.
        /// </summary>
        public VS_FIXEDFILEINFO VsFixedFileInfo { get; }

        /// <summary>
        /// Optional StringFileInfo contained in the Children member.
        /// </summary>
        public StringFileInfo StringFileInfo { get; }

        /// <summary>
        /// Optional VarFileInfo contained in the Children member.
        /// </summary>
        public VarFileInfo VarFileInfo { get; }

        public VS_VERSIONINFO(byte[] buff, uint offset) 
            : base(buff, offset)
        {
            var currentOffset = (uint) (Offset + 6 + szKey.LengthInByte()
                                + (Offset + 6 + szKey.LengthInByte()).PaddingBytes(32));

            VsFixedFileInfo = new VS_FIXEDFILEINFO(buff, currentOffset);
            
            currentOffset += wValueLength;
            currentOffset += currentOffset.PaddingBytes(32);
            StringFileInfo = new StringFileInfo(buff, currentOffset);

            currentOffset += StringFileInfo.wLength;
            currentOffset += currentOffset.PaddingBytes(32);
            VarFileInfo = new VarFileInfo(buff, currentOffset);
        }
    }

    /// <summary>
    /// Information about the file not dependent on any language and code page
    /// combination.
    /// </summary>
    public class VarFileInfo : AbstractStructure
    {
        public VarFileInfo(byte[] buff, uint offset) 
            : base(buff, offset)
        {
            Children = ReadChildren();
        }

        /// <summary>
        /// Length of the VarFileInfo structure including
        /// all children.
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
        /// Contains a 1 if the version resource is text and a
        /// 0 if the version resource is binary data.
        /// </summary>
        public ushort wType
        {
            get => Buff.BytesToUInt16(Offset + 0x4);
            set => Buff.SetUInt16(Offset + 04, value);
        }

        /// <summary>
        /// Unicode string "VarFileInfo"
        /// </summary>
        public string szKey => Buff.GetUnicodeString(Offset + 0x6);

        /// <summary>
        /// Usually a list of languages that the
        /// application or DLL supports.
        /// </summary>
        public Var[] Children { get; }

        private Var[] ReadChildren()
        {
            var currentOffset =
                Offset + 6 + szKey.LengthInByte()
                + (Offset + 6 + szKey.LengthInByte()).PaddingBytes(32);

            var values = new List<Var>();

            while (currentOffset < Offset + wLength)
            {
                values.Add(new Var(Buff, (uint) currentOffset));
                currentOffset += values.Last().wLength;
            }

            return values.ToArray();
        }
    }

    /// <summary>
    /// Usually a list of languages that the
    /// application or DLL supports.
    /// </summary>
    public class Var : AbstractStructure
    {
        public Var(byte[] buff, uint offset) 
            : base(buff, offset)
        {
            Value = ReadValues();
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
        public uint[] Value { get; }

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