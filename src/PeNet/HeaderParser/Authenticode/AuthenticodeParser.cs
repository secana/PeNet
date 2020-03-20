using System;
using PeNet.Header.Authenticode;

namespace PeNet.HeaderParser.Authenticode
{
    internal class AuthenticodeParser
    {
        private readonly PeFile _peFile;

        internal AuthenticodeParser(PeFile peFile)
        {
            _peFile = peFile;
        }

        internal AuthenticodeInfo? ParseTarget()
        {
            try
            {
                return new AuthenticodeInfo(_peFile);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}