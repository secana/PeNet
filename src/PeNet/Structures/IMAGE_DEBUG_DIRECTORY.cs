using System;
using System.Linq;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The IMAGE_DEBUG_DIRECTORY hold debug information
    ///     about the PE file.
    /// </summary>
    public class IMAGE_DEBUG_DIRECTORY : AbstractStructure
    {
        /// <summary>
        ///     Create a new IMAGE_DEBUG_DIRECTORY object.
        /// </summary>
        /// <param name="buff">PE binary as byte array.</param>
        /// <param name="offset">Offset to the debug struct in the binary.</param>
        public IMAGE_DEBUG_DIRECTORY(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        /// <summary>
        ///     Characteristics of the debug information.
        /// </summary>
        public uint Characteristics
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     Time and date stamp
        /// </summary>
        public uint TimeDateStamp
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Major Version.
        /// </summary>
        public ushort MajorVersion
        {
            get => PeFile.ReadUShort(Offset + 0x8);
            set => PeFile.WriteUShort(Offset + 0x8, value);
        }

        /// <summary>
        ///     Minor Version.
        /// </summary>
        public ushort MinorVersion
        {
            get => PeFile.ReadUShort(Offset + 0xa);
            set => PeFile.WriteUShort(Offset + 0xa, value);
        }

        /// <summary>
        ///     Type
        ///     1: Coff
        ///     2: CV-PDB
        ///     9: Borland
        /// </summary>
        public uint Type
        {
            get => PeFile.ReadUInt(Offset + 0xc);
            set => PeFile.WriteUInt(Offset + 0xc, value);
        }

        /// <summary>
        ///     Size of data.
        /// </summary>
        public uint SizeOfData
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        ///     Address of raw data.
        /// </summary>
        public uint AddressOfRawData
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        ///     Pointer to raw data.
        /// </summary>
        public uint PointerToRawData
        {
            get => PeFile.ReadUInt(Offset + 0x18);
            set => PeFile.WriteUInt(Offset + 0x18, value);
        }

        public Guid PdbSignature
        {
            get
            {
                var bytes = new byte[16];
                Array.Copy(PeFile, PointerToRawData + 4, bytes, 0, 16);
                return new Guid(bytes);
            }
            set => Array.Copy(value.ToByteArray(), 0, PeFile, PointerToRawData + 4, 16);
        }

        public uint PdbAge
        {
            get => PeFile.BytesToUInt32(PointerToRawData + 0x14);
            set => PeFile.SetUInt32(PointerToRawData + 0x14, value);
        }

        public string PdbPath
        {
            get
            {
                var bytes = PeFile.Skip((int) PointerToRawData + 0x18).TakeWhile(x => x != 0x0).ToArray();
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}