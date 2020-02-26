using System;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Codeview PDB information from the Debug directory.
    /// </summary>
    public class CvInfoPdb70 : AbstractStructure
    {
        public CvInfoPdb70(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        /// <summary>
        /// Codeview signature.
        /// </summary>
        public uint CvSignature
        {
            get => Buff.BytesToUInt32(Offset);
            set => Buff.SetUInt32(Offset, value);
        }

        /// <summary>
        /// The PDB signature is a GUID to identify the PDB file
        /// which belongs to the PE file.
        /// </summary>
        public Guid Signature
        {
            get
            {
                var bytes = new byte[16];
                Array.Copy(Buff, Offset + 4, bytes, 0, 16);
                return new Guid(bytes);
            }
            set => Array.Copy(value.ToByteArray(), 0, Buff, Offset + 4, 16);
        }

        /// <summary>
        /// PDB Age
        /// </summary>
        public uint Age
        {
            get => Buff.BytesToUInt32(Offset + 0x14);
            set => Buff.SetUInt32(Offset + 0x14, value);
        }

        /// <summary>
        /// Original file name of the PDB that belongs to the
        /// PE file.
        /// </summary>
        public string PdbFileName => Buff.GetCString(Offset + 0x18);
    }
}