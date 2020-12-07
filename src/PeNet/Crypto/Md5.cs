using System;

namespace PeNet.Crypto
{
    // For reference see: https://github.com/bcgit/bc-csharp/blob/master/crypto/src/crypto/digests/MD5Digest.cs
    internal sealed class Md5 : Hash
    {
        protected override int DigestLength { get; } = 16;
        private uint _h1, _h2, _h3, _h4;
        private readonly uint[] _x = new uint[16];
        private int _xOff;
        private const int S11 = 7;
        private const int S12 = 12;
        private const int S13 = 17;
        private const int S14 = 22;
        private const int S21 = 5;
        private const int S22 = 9;
        private const int S23 = 14;
        private const int S24 = 20;
        private const int S31 = 4;
        private const int S32 = 11;
        private const int S33 = 16;
        private const int S34 = 23;
        private const int S41 = 6;
        private const int S42 = 10;
        private const int S43 = 15;
        private const int S44 = 21;

        public Md5() => Reset();

        protected override void ProcessWord(Span<byte> input, int inOff)
        {
            _x[_xOff] = Pack.LE_To_UInt32(input, inOff);

            if (++_xOff == 16)
            {
                ProcessBlock();
            }
        }

        protected override void ProcessLength(long bitLength)
        {
            if (_xOff > 14)
            {
                if (_xOff == 15)
                    _x[15] = 0;

                ProcessBlock();
            }

            for (var i = _xOff; i < 14; ++i)
            {
                _x[i] = 0;
            }

            _x[14] = (uint)((ulong)bitLength);
            _x[15] = (uint)((ulong)bitLength >> 32);
        }

        protected override void DoFinal(Span<byte> output, int outOff)
        {
            Finish();

            Pack.UInt32_To_LE(_h1, output, outOff);
            Pack.UInt32_To_LE(_h2, output, outOff + 4);
            Pack.UInt32_To_LE(_h3, output, outOff + 8);
            Pack.UInt32_To_LE(_h4, output, outOff + 12);

            Reset();
        }

        protected override void Reset()
        {
            base.Reset();

            _h1 = 0x67452301;
            _h2 = 0xefcdab89;
            _h3 = 0x98badcfe;
            _h4 = 0x10325476;

            _xOff = 0;

            for (var i = 0; i != _x.Length; i++)
            {
                _x[i] = 0;
            }
        }

        private static uint RotateLeft(uint x, int n) => (x << n) | (x >> (32 - n));
        private static uint F(uint u, uint v, uint w) => (u & v) | (~u & w);
        private static uint G(uint u, uint v, uint w) => (u & w) | (v & ~w);
        private static uint H(uint u, uint v, uint w) => u ^ v ^ w;
        private static uint K(uint u, uint v, uint w) => v ^ (u | ~w);

        protected override void ProcessBlock()
        {
            var a = _h1;
            var b = _h2;
            var c = _h3;
            var d = _h4;

            a = RotateLeft((a + F(b, c, d) + _x[0] + 0xd76aa478), S11) + b;
            d = RotateLeft((d + F(a, b, c) + _x[1] + 0xe8c7b756), S12) + a;
            c = RotateLeft((c + F(d, a, b) + _x[2] + 0x242070db), S13) + d;
            b = RotateLeft((b + F(c, d, a) + _x[3] + 0xc1bdceee), S14) + c;
            a = RotateLeft((a + F(b, c, d) + _x[4] + 0xf57c0faf), S11) + b;
            d = RotateLeft((d + F(a, b, c) + _x[5] + 0x4787c62a), S12) + a;
            c = RotateLeft((c + F(d, a, b) + _x[6] + 0xa8304613), S13) + d;
            b = RotateLeft((b + F(c, d, a) + _x[7] + 0xfd469501), S14) + c;
            a = RotateLeft((a + F(b, c, d) + _x[8] + 0x698098d8), S11) + b;
            d = RotateLeft((d + F(a, b, c) + _x[9] + 0x8b44f7af), S12) + a;
            c = RotateLeft((c + F(d, a, b) + _x[10] + 0xffff5bb1), S13) + d;
            b = RotateLeft((b + F(c, d, a) + _x[11] + 0x895cd7be), S14) + c;
            a = RotateLeft((a + F(b, c, d) + _x[12] + 0x6b901122), S11) + b;
            d = RotateLeft((d + F(a, b, c) + _x[13] + 0xfd987193), S12) + a;
            c = RotateLeft((c + F(d, a, b) + _x[14] + 0xa679438e), S13) + d;
            b = RotateLeft((b + F(c, d, a) + _x[15] + 0x49b40821), S14) + c;

            a = RotateLeft((a + G(b, c, d) + _x[1] + 0xf61e2562), S21) + b;
            d = RotateLeft((d + G(a, b, c) + _x[6] + 0xc040b340), S22) + a;
            c = RotateLeft((c + G(d, a, b) + _x[11] + 0x265e5a51), S23) + d;
            b = RotateLeft((b + G(c, d, a) + _x[0] + 0xe9b6c7aa), S24) + c;
            a = RotateLeft((a + G(b, c, d) + _x[5] + 0xd62f105d), S21) + b;
            d = RotateLeft((d + G(a, b, c) + _x[10] + 0x02441453), S22) + a;
            c = RotateLeft((c + G(d, a, b) + _x[15] + 0xd8a1e681), S23) + d;
            b = RotateLeft((b + G(c, d, a) + _x[4] + 0xe7d3fbc8), S24) + c;
            a = RotateLeft((a + G(b, c, d) + _x[9] + 0x21e1cde6), S21) + b;
            d = RotateLeft((d + G(a, b, c) + _x[14] + 0xc33707d6), S22) + a;
            c = RotateLeft((c + G(d, a, b) + _x[3] + 0xf4d50d87), S23) + d;
            b = RotateLeft((b + G(c, d, a) + _x[8] + 0x455a14ed), S24) + c;
            a = RotateLeft((a + G(b, c, d) + _x[13] + 0xa9e3e905), S21) + b;
            d = RotateLeft((d + G(a, b, c) + _x[2] + 0xfcefa3f8), S22) + a;
            c = RotateLeft((c + G(d, a, b) + _x[7] + 0x676f02d9), S23) + d;
            b = RotateLeft((b + G(c, d, a) + _x[12] + 0x8d2a4c8a), S24) + c;

            a = RotateLeft((a + H(b, c, d) + _x[5] + 0xfffa3942), S31) + b;
            d = RotateLeft((d + H(a, b, c) + _x[8] + 0x8771f681), S32) + a;
            c = RotateLeft((c + H(d, a, b) + _x[11] + 0x6d9d6122), S33) + d;
            b = RotateLeft((b + H(c, d, a) + _x[14] + 0xfde5380c), S34) + c;
            a = RotateLeft((a + H(b, c, d) + _x[1] + 0xa4beea44), S31) + b;
            d = RotateLeft((d + H(a, b, c) + _x[4] + 0x4bdecfa9), S32) + a;
            c = RotateLeft((c + H(d, a, b) + _x[7] + 0xf6bb4b60), S33) + d;
            b = RotateLeft((b + H(c, d, a) + _x[10] + 0xbebfbc70), S34) + c;
            a = RotateLeft((a + H(b, c, d) + _x[13] + 0x289b7ec6), S31) + b;
            d = RotateLeft((d + H(a, b, c) + _x[0] + 0xeaa127fa), S32) + a;
            c = RotateLeft((c + H(d, a, b) + _x[3] + 0xd4ef3085), S33) + d;
            b = RotateLeft((b + H(c, d, a) + _x[6] + 0x04881d05), S34) + c;
            a = RotateLeft((a + H(b, c, d) + _x[9] + 0xd9d4d039), S31) + b;
            d = RotateLeft((d + H(a, b, c) + _x[12] + 0xe6db99e5), S32) + a;
            c = RotateLeft((c + H(d, a, b) + _x[15] + 0x1fa27cf8), S33) + d;
            b = RotateLeft((b + H(c, d, a) + _x[2] + 0xc4ac5665), S34) + c;

            a = RotateLeft((a + K(b, c, d) + _x[0] + 0xf4292244), S41) + b;
            d = RotateLeft((d + K(a, b, c) + _x[7] + 0x432aff97), S42) + a;
            c = RotateLeft((c + K(d, a, b) + _x[14] + 0xab9423a7), S43) + d;
            b = RotateLeft((b + K(c, d, a) + _x[5] + 0xfc93a039), S44) + c;
            a = RotateLeft((a + K(b, c, d) + _x[12] + 0x655b59c3), S41) + b;
            d = RotateLeft((d + K(a, b, c) + _x[3] + 0x8f0ccc92), S42) + a;
            c = RotateLeft((c + K(d, a, b) + _x[10] + 0xffeff47d), S43) + d;
            b = RotateLeft((b + K(c, d, a) + _x[1] + 0x85845dd1), S44) + c;
            a = RotateLeft((a + K(b, c, d) + _x[8] + 0x6fa87e4f), S41) + b;
            d = RotateLeft((d + K(a, b, c) + _x[15] + 0xfe2ce6e0), S42) + a;
            c = RotateLeft((c + K(d, a, b) + _x[6] + 0xa3014314), S43) + d;
            b = RotateLeft((b + K(c, d, a) + _x[13] + 0x4e0811a1), S44) + c;
            a = RotateLeft((a + K(b, c, d) + _x[4] + 0xf7537e82), S41) + b;
            d = RotateLeft((d + K(a, b, c) + _x[11] + 0xbd3af235), S42) + a;
            c = RotateLeft((c + K(d, a, b) + _x[2] + 0x2ad7d2bb), S43) + d;
            b = RotateLeft((b + K(c, d, a) + _x[9] + 0xeb86d391), S44) + c;

            _h1 += a;
            _h2 += b;
            _h3 += c;
            _h4 += d;

            _xOff = 0;
        }
    }
}