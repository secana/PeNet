using System;

namespace PeNet.Crypto
{
    // For reference see: https://github.com/bcgit/bc-csharp/blob/master/crypto/src/crypto/digests/Sha1Digest.cs
    internal class Sha1 : Hash
    {
        protected override int DigestLength { get; } = 20;
        private uint _h1, _h2, _h3, _h4, _h5;
        private readonly uint[] _x = new uint[80];
        private int _xOff;
        private const uint Y1 = 0x5a827999;
        private const uint Y2 = 0x6ed9eba1;
        private const uint Y3 = 0x8f1bbcdc;
        private const uint Y4 = 0xca62c1d6;

        public Sha1()
        {
            Reset();
        }

        protected override void ProcessWord(Span<byte> input, int inOff)
        {
            _x[_xOff] = Pack.BE_To_UInt32(input, inOff);

            if (++_xOff == 16)
            {
                ProcessBlock();
            }
        }

        protected override void ProcessLength(long bitLength)
        {
            if (_xOff > 14)
            {
                ProcessBlock();
            }

            _x[14] = (uint)((ulong)bitLength >> 32);
            _x[15] = (uint)((ulong)bitLength);
        }

        protected override void DoFinal(Span<byte> output, int outOff)
        {
            Finish();

            Pack.UInt32_To_BE(_h1, output, outOff);
            Pack.UInt32_To_BE(_h2, output, outOff + 4);
            Pack.UInt32_To_BE(_h3, output, outOff + 8);
            Pack.UInt32_To_BE(_h4, output, outOff + 12);
            Pack.UInt32_To_BE(_h5, output, outOff + 16);

            Reset();
        }

        protected sealed override void Reset()
        {
            base.Reset();

            _h1 = 0x67452301;
            _h2 = 0xefcdab89;
            _h3 = 0x98badcfe;
            _h4 = 0x10325476;
            _h5 = 0xc3d2e1f0;

            _xOff = 0;
            Array.Clear(_x, 0, _x.Length);
        }

        private static uint F(uint u, uint v, uint w) => (u & v) | (~u & w);
        private static uint H(uint u, uint v, uint w) => u ^ v ^ w;
        private static uint G(uint u, uint v, uint w) => (u & v) | (u & w) | (v & w);

        protected override void ProcessBlock()
        {
            for (var i = 16; i < 80; i++)
            {
                var t = _x[i - 3] ^ _x[i - 8] ^ _x[i - 14] ^ _x[i - 16];
                _x[i] = t << 1 | t >> 31;
            }

            var a = _h1;
            var b = _h2;
            var c = _h3;
            var d = _h4;
            var e = _h5;
            var idx = 0;

            for (var j = 0; j < 4; j++)
            {
                e += (a << 5 | (a >> 27)) + F(b, c, d) + _x[idx++] + Y1;
                b = b << 30 | (b >> 2);

                d += (e << 5 | (e >> 27)) + F(a, b, c) + _x[idx++] + Y1;
                a = a << 30 | (a >> 2);

                c += (d << 5 | (d >> 27)) + F(e, a, b) + _x[idx++] + Y1;
                e = e << 30 | (e >> 2);

                b += (c << 5 | (c >> 27)) + F(d, e, a) + _x[idx++] + Y1;
                d = d << 30 | (d >> 2);

                a += (b << 5 | (b >> 27)) + F(c, d, e) + _x[idx++] + Y1;
                c = c << 30 | (c >> 2);
            }

            for (var j = 0; j < 4; j++)
            {
                e += (a << 5 | (a >> 27)) + H(b, c, d) + _x[idx++] + Y2;
                b = b << 30 | (b >> 2);

                d += (e << 5 | (e >> 27)) + H(a, b, c) + _x[idx++] + Y2;
                a = a << 30 | (a >> 2);

                c += (d << 5 | (d >> 27)) + H(e, a, b) + _x[idx++] + Y2;
                e = e << 30 | (e >> 2);

                b += (c << 5 | (c >> 27)) + H(d, e, a) + _x[idx++] + Y2;
                d = d << 30 | (d >> 2);

                a += (b << 5 | (b >> 27)) + H(c, d, e) + _x[idx++] + Y2;
                c = c << 30 | (c >> 2);
            }

            for (var j = 0; j < 4; j++)
            {
                e += (a << 5 | (a >> 27)) + G(b, c, d) + _x[idx++] + Y3;
                b = b << 30 | (b >> 2);

                d += (e << 5 | (e >> 27)) + G(a, b, c) + _x[idx++] + Y3;
                a = a << 30 | (a >> 2);

                c += (d << 5 | (d >> 27)) + G(e, a, b) + _x[idx++] + Y3;
                e = e << 30 | (e >> 2);

                b += (c << 5 | (c >> 27)) + G(d, e, a) + _x[idx++] + Y3;
                d = d << 30 | (d >> 2);

                a += (b << 5 | (b >> 27)) + G(c, d, e) + _x[idx++] + Y3;
                c = c << 30 | (c >> 2);
            }

            for (var j = 0; j < 4; j++)
            {
                e += (a << 5 | (a >> 27)) + H(b, c, d) + _x[idx++] + Y4;
                b = b << 30 | (b >> 2);

                d += (e << 5 | (e >> 27)) + H(a, b, c) + _x[idx++] + Y4;
                a = a << 30 | (a >> 2);

                c += (d << 5 | (d >> 27)) + H(e, a, b) + _x[idx++] + Y4;
                e = e << 30 | (e >> 2);

                b += (c << 5 | (c >> 27)) + H(d, e, a) + _x[idx++] + Y4;
                d = d << 30 | (d >> 2);

                a += (b << 5 | (b >> 27)) + H(c, d, e) + _x[idx++] + Y4;
                c = c << 30 | (c >> 2);
            }

            _h1 += a;
            _h2 += b;
            _h3 += c;
            _h4 += d;
            _h5 += e;

            _xOff = 0;
            Array.Clear(_x, 0, 16);
        }
    }
}
