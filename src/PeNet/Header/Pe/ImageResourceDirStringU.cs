using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     Represents a Unicode string used for resource names
    ///     in the resource section.
    /// </summary>
    public class ImageResourceDirStringU : AbstractStructure
    {
        /// <summary>
        ///     Create a new ImageResourceDirStringU Unicode string.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the string.</param>
        public ImageResourceDirStringU(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Length of the string in Unicode characters, *not* in bytes.
        ///     1 Unicode char = 2 bytes.
        /// </summary>
        public ushort Length
        {
            get => PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, value);
        }

        /// <summary>
        ///     The Unicode string as a .Net string.
        /// </summary>
        public string NameString
        {
            get => PeFile.ReadUnicodeString(Offset + 2);
        }
    }
}