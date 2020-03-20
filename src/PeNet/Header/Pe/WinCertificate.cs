using System;
using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The WinCertificate the information
    ///     in the security directory of the PE file.
    ///     It contains information about any certificates
    ///     used to sign the binary.
    /// </summary>
    public class WinCertificate : AbstractStructure
    {
        /// <summary>
        ///     Create a new WinCertificate object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset to the structure.</param>
        public WinCertificate(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Length of the certificate.
        /// </summary>
        public uint DwLength
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     Revision.
        /// </summary>
        public ushort WRevision
        {
            get => PeFile.ReadUShort(Offset + 0x4);
            set => PeFile.WriteUShort(Offset + 0x4, value);
        }

        /// <summary>
        ///     The certificate type.
        /// </summary>
        public WinCertificateType WCertificateType
        {
            get => (WinCertificateType) PeFile.ReadUShort(Offset + 0x6);
            set => PeFile.WriteUShort(Offset + 0x6, (ushort) value);
        }

        /// <summary>
        ///     The certificate.
        /// </summary>
        public Span<byte> BCertificate
            => PeFile.AsSpan(Offset + 0x8, DwLength - 8);
    }

    /// <summary>
    ///     WinCertificate wCertificateType constants.
    /// </summary>
    [Flags]
    public enum WinCertificateType : ushort
    {
        /// <summary>
        ///     Certificate is X509 standard.
        /// </summary>
        X509 = 0x0001,

        /// <summary>
        ///     Certificate is PKCS signed data.
        /// </summary>
        PkcsSignedData = 0x0002,

        /// <summary>
        ///     Reserved
        /// </summary>
        Reserved1 = 0x0003,

        /// <summary>
        ///     Certificate is PKCS1 signature.
        /// </summary>
        Pkcs1Sign = 0x0009
    }
}