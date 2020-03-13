using System;
using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    /// Codeview PDB information from the Debug directory.
    /// </summary>
    public class CvInfoPdb70 : AbstractStructure
    {
        public CvInfoPdb70(IRawFile peFile, uint offset) 
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// Codeview signature.
        /// </summary>
        public uint CvSignature
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        /// The PDB signature is a GUID to identify the PDB file
        /// which belongs to the PE file.
        /// </summary>
        public Guid Signature
        {
            get => new Guid(PeFile.AsSpan(Offset + 4, 16));
            set => PeFile.WriteBytes(Offset + 4, value.ToByteArray());
        }

        /// <summary>
        /// PDB Age
        /// </summary>
        public uint Age
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        /// Original file name of the PDB that belongs to the
        /// PE file.
        /// </summary>
        public string PdbFileName => PeFile.ReadAsciiString(Offset + 0x18);
    }
}