using System;

namespace PeNet.Crypto
{
    public static class Hash
    {
        public static string Md5(Span<byte> input)
        {
            var md5 = new Md5();
            return md5.Compute(input);
        }
    }
}