using PeNet.FileParser;
using PeNet.Structures;

namespace PeNet.Parser
{
    internal class WinCertificateParser : SafeParser<WIN_CERTIFICATE>
    {
        internal WinCertificateParser(IRawFile peFile, long offset)
            : base(peFile, offset)
        {
        }

        protected override WIN_CERTIFICATE? ParseTarget()
        {
            return Offset == 0 ? null : new WIN_CERTIFICATE(PeFile, Offset);
        }
    }
}