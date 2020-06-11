using PeNet.FileParser;
using System;
using System.Text;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The ImageImportByName structure is used to
    ///     describes imports of functions or symbols by their name.
    ///     The AddressOfData in the ImageThunkData from the
    ///     ImageImportDescriptor points to it.
    /// </summary>
    public class ImageImportByName : AbstractStructure
    {
        /// <summary>
        ///     Create new ImageImportByName object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the ImageImportByName.</param>
        public ImageImportByName(IRawFile peFile, uint offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Hint.
        /// </summary>
        public ushort Hint
        {
            get => PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, value);
        }

        /// <summary>
        ///     Name of the function to import as a C-string (null terminated).
        /// </summary>
        public string Name
        {
            get => PeFile.ReadAsciiString(Offset + 0x2);
            set {
                var source = Encoding.ASCII.GetBytes(value);
                var dest = new byte[source.Length + 1];
                Array.Copy(source, dest, source.Length);
                PeFile.WriteBytes(Offset + 0x2, dest);
            }
        }
    }
}