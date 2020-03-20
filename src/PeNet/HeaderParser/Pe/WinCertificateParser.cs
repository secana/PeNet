using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class WinCertificateParser : SafeParser<WinCertificate>
    {
        internal WinCertificateParser(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        protected override WinCertificate? ParseTarget()
        {
            return Offset == 0 ? null : new WinCertificate(PeFile, Offset);
        }
    }
}