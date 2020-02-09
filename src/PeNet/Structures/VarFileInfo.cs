using System.Collections.Generic;
using System.Linq;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Information about the file not dependent on any language and code page
    /// combination.
    /// </summary>
    public class VarFileInfo : AbstractStructure
    {
        private Var[]? _children;

        public VarFileInfo(byte[] buff, uint offset) 
            : base(buff, offset)
        {
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
        public Var[] Children {
            get
            {
                _children ??= ReadChildren();
                return _children;
            }
        }

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
}