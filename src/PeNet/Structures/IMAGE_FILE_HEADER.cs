using PeNet.Utilities;
using System.IO;

namespace PeNet.Structures
{
    /// <summary>
    ///     The File header contains information about the structure
    ///     and properties of the PE file.
    /// </summary>
    public class IMAGE_FILE_HEADER : AbstractStructure
    {
        /// <summary>
        ///     Create a new IMAGE_FILE_HEADER object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset to the file header.</param>
        public IMAGE_FILE_HEADER(IRawFile peFile, int offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     The machine (CPU type) the PE file is intended for.
        ///     Can be resolved with Utility.ResolveTargetMachine(machine).
        /// </summary>
        public ushort Machine
        {
            get => PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, value);
        }

        /// <summary>
        ///     The number of sections in the PE file.
        /// </summary>
        public ushort NumberOfSections
        {
            get => PeFile.ReadUShort(Offset + 0x2);
            set => PeFile.WriteUShort(Offset + 0x2, value);
        }

        /// <summary>
        ///     Time and date stamp.
        /// </summary>
        public uint TimeDateStamp
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Pointer to COFF symbols table. They are rare in PE files,
        ///     but often in obj files.
        /// </summary>
        public uint PointerToSymbolTable
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        ///     The number of COFF symbols.
        /// </summary>
        public uint NumberOfSymbols
        {
            get => PeFile.ReadUInt(Offset + 0xC);
            set => PeFile.WriteUInt(Offset + 0xC, value);
        }

        /// <summary>
        ///     The size of the optional header which follow the file header.
        /// </summary>
        public ushort SizeOfOptionalHeader
        {
            get => PeFile.ReadUShort(Offset + 0x10);
            set => PeFile.WriteUShort(Offset + 0x10, value);
        }

        /// <summary>
        ///     Set of flags which describe the PE file in detail.
        ///     Can be resolved with Utility.ResolveCharacteristics(characteristics).
        /// </summary>
        public ushort Characteristics
        {
            get => PeFile.ReadUShort(Offset + 0x12);
            set => PeFile.WriteUShort(Offset + 0x12, value);
        }
    }
}