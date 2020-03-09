using System;

namespace PeNet.Structures
{
    /// <summary>
    ///     The WIN_CERTIFICATE the information
    ///     in the security directory of the PE file.
    ///     It contains information about any certificates
    ///     used to sign the binary.
    /// </summary>
    public class WIN_CERTIFICATE : AbstractStructure
    {
        /// <summary>
        ///     Create a new WIN_CERTIFICATE object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset to the structure.</param>
        public WIN_CERTIFICATE(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Length of the certificate.
        /// </summary>
        public uint dwLength
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     Revision.
        /// </summary>
        public ushort wRevision
        {
            get => PeFile.ReadUShort(Offset + 0x4);
            set => PeFile.WriteUShort(Offset + 0x4, value);
        }

        /// <summary>
        ///     The certificate type.
        /// </summary>
        public ushort wCertificateType
        {
            get => PeFile.ReadUShort(Offset + 0x6);
            set => PeFile.WriteUShort(Offset + 0x6, value);
        }

        /// <summary>
        ///     The certificate.
        /// </summary>
        public Span<byte> bCertificate
        {
            get
            {
                return PeFile.AsSpan(Offset + 0x8, dwLength - 8);
            }
        }
    }
}