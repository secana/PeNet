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
        /// Typically 0x53445352 = 'RSDS'
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
#if NET48 || NETSTANDARD2_0
            get => new Guid(PeFile.AsSpan(Offset + 4, 16).ToArray());
#else
            get => new Guid(PeFile.AsSpan(Offset + 4, 16));
#endif
            set => PeFile.WriteBytes(Offset + 4, value.ToByteArray());
        }

        /// <summary>
        /// PDB Age is the iteration of the PDB. The first iteration is 1. 
        /// The iteration is incremented each time the PDB content is augmented.
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