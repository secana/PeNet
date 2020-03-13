using System.Collections.Generic;
using System.Linq;
using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    /// Information about the file not dependent on any language and code page
    /// combination.
    /// </summary>
    public class VarFileInfo : AbstractStructure
    {
        private Var[]? _children;

        /// <summary>
        /// Create a VarFileInfo instance.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of a VarFileInfo in the PE file.</param>
        public VarFileInfo(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// Length of the VarFileInfo structure including
        /// all children.
        /// </summary>
        public ushort WLength
        {
            get => PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, value);
        }

        /// <summary>
        /// Always zero.
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
        /// Unicode string "VarFileInfo"
        /// </summary>
        public string SzKey => PeFile.ReadUnicodeString(Offset + 0x6);


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
                Offset + 6 + SzKey.UStringByteLength()
                + (Offset + 6 + SzKey.UStringByteLength()).PaddingBytes(32);

            var values = new List<Var>();

            while (currentOffset < Offset + WLength)
            {
                values.Add(new Var(PeFile, currentOffset));
                currentOffset += values.Last().WLength;
            }

            return values.ToArray();
        }
    }
}