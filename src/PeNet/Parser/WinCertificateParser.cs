using PeNet.Structures;

namespace PeNet.Parser
{
    internal class WinCertificateParser : SafeParser<WIN_CERTIFICATE>
    {
        internal WinCertificateParser(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        protected override WIN_CERTIFICATE? ParseTarget()
        {
            return Offset == 0 ? null : new WIN_CERTIFICATE(PeFile, Offset);
        }
    }
}