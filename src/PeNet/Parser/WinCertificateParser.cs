using PeNet.Structures;

namespace PeNet.Parser
{
    internal class WinCertificateParser : SafeParser<WIN_CERTIFICATE>
    {
        internal WinCertificateParser(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        protected override WIN_CERTIFICATE ParseTarget()
        {
            if (_offset == 0)
                return null;

            return new WIN_CERTIFICATE(_buff, _offset);
        }
    }
}