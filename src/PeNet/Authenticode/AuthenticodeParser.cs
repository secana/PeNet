using PeNet.Parser;

namespace PeNet.Authenticode
{
    internal class AuthenticodeParser : SafeParser<AuthenticodeInfo>
    {
        private readonly PeFile _peFile;

        internal AuthenticodeParser(PeFile peFile)
            : base(null, 0)
        {
            _peFile = peFile;
        }

        protected override AuthenticodeInfo ParseTarget()
        {
            return new AuthenticodeInfo(_peFile);
        }
    }
}