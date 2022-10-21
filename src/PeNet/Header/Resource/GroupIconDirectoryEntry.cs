using System.Linq;
using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    ///     Information about GroupIcons.
    /// </summary>
    public class GroupIconDirectoryEntry : AbstractStructure
    {
        public const ushort Size = 0xE;

        /// <summary>
        ///     Create a new GroupIconDirectoryEntry instance.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of the GroupIconDirectoryEntry structure in the PE file.</param>
        public GroupIconDirectoryEntry(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Width of the corresponding icon.
        /// </summary>
        public byte BWidth
        {
            get => PeFile.ReadByte(Offset);
            set => PeFile.WriteByte(Offset, value);
        }

        /// <summary>
        ///     Height of the corresponding icon.
        /// </summary>
        public byte BHeight
        {
            get => PeFile.ReadByte(Offset + 0x1);
            set => PeFile.WriteByte(Offset + 0x1, value);
        }

        /// <summary>
        ///     Color count of the corresponding icon.
        /// </summary>
        public byte BColorCount
        {
            get => PeFile.ReadByte(Offset + 0x2);
            set => PeFile.WriteByte(Offset + 0x2, value);
        }

        /// <summary>
        ///     Always zero, reserved for generators.
        /// </summary>
        public byte BReserved
        {
            get => PeFile.ReadByte(Offset + 0x3);
            set => PeFile.WriteByte(Offset + 0x3, value);
        }

        /// <summary>
        ///     Color Planes.
        /// </summary>
        public ushort WPlanes
        {
            get => PeFile.ReadUShort(Offset + 0x4);
            set => PeFile.WriteUShort(Offset + 0x4, value);
        }

        /// <summary>
        ///     Bits per pixel.
        /// </summary>
        public ushort WBitCount
        {
            get => PeFile.ReadUShort(Offset + 0x6);
            set => PeFile.WriteUShort(Offset + 0x6, value);
        }

        /// <summary>
        ///     Byte-size of the corresponding icon.
        /// </summary>
        public uint DwBytesInRes
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        ///     ID of the corresponding icon.
        /// </summary>
        public ushort NId
        {
            get => PeFile.ReadUShort(Offset + 0x0C);
            set => PeFile.WriteUShort(Offset + 0x0C, value);
        }

        /// <summary>
        ///     Searching in Resources for Icon with ID of this entry.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <returns>Icon class object corresponding to entry ID, null if no such icon exists.</returns>
        public Icon? AssociatedIcon(PeFile peFile)
        {
            return peFile.Resources?.Icons?.FirstOrDefault(i => i.Id == NId);
        }
    }
}
