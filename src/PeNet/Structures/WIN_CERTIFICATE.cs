using System;
using PeNet.Utilities;

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
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset to the structure.</param>
        public WIN_CERTIFICATE(byte[] buff, uint offset)
            : base(buff, offset)
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
        ///     The certificate as a byte array.
        /// </summary>
        public byte[] bCertificate
        {
            get
            {
                var cert = new byte[dwLength - 8];
                Array.Copy(PeFile, Offset + 0x8, cert, 0, dwLength - 8);
                return cert;
            }
            set => Array.Copy(value, 0, PeFile, Offset + 0x8, value.Length);
        }
    }
}