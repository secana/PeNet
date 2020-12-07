using System;

namespace PeNet.Crypto
{
    // For reference see: https://github.com/bcgit/bc-csharp/blob/master/crypto/src/crypto/digests/Sha256Digest.cs
    internal class Sha256 : Hash
    {
        protected override int DigestLength { get; } = 32;
        private uint _h1, _h2, _h3, _h4, _h5, _h6, _h7, _h8;
        private readonly uint[] _x = new uint[64];
        private int _xOff;
        private static readonly uint[] K = {
            0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5,
            0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
            0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
            0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
            0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc,
            0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
            0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
            0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
            0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
            0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
            0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3,
            0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
            0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5,
            0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
            0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
            0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
        };

        public Sha256() => InitHs();

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

            Pack.UInt32_To_BE((uint)_h1, output, outOff);
            Pack.UInt32_To_BE((uint)_h2, output, outOff + 4);
            Pack.UInt32_To_BE((uint)_h3, output, outOff + 8);
            Pack.UInt32_To_BE((uint)_h4, output, outOff + 12);
            Pack.UInt32_To_BE((uint)_h5, output, outOff + 16);
            Pack.UInt32_To_BE((uint)_h6, output, outOff + 20);
            Pack.UInt32_To_BE((uint)_h7, output, outOff + 24);
            Pack.UInt32_To_BE((uint)_h8, output, outOff + 28);

            Reset();
        }

        protected override void Reset()
        {
            base.Reset();
            InitHs();
            _xOff = 0;
            Array.Clear(_x, 0, _x.Length);
        }

        private void InitHs()
        {
            _h1 = 0x6a09e667;
            _h2 = 0xbb67ae85;
            _h3 = 0x3c6ef372;
            _h4 = 0xa54ff53a;
            _h5 = 0x510e527f;
            _h6 = 0x9b05688c;
            _h7 = 0x1f83d9ab;
            _h8 = 0x5be0cd19;
        }

        protected override void ProcessBlock()
        {
            for (var ti = 16; ti <= 63; ti++)
            {
                _x[ti] = Theta1(_x[ti - 2]) + _x[ti - 7] + Theta0(_x[ti - 15]) + _x[ti - 16];
            }

            var a = _h1;
            var b = _h2;
            var c = _h3;
            var d = _h4;
            var e = _h5;
            var f = _h6;
            var g = _h7;
            var h = _h8;

            var t = 0;
            for (var i = 0; i < 8; ++i)
            {
                h += Sum1Ch(e, f, g) + K[t] + _x[t];
                d += h;
                h += Sum0Maj(a, b, c);
                ++t;

                g += Sum1Ch(d, e, f) + K[t] + _x[t];
                c += g;
                g += Sum0Maj(h, a, b);
                ++t;

                f += Sum1Ch(c, d, e) + K[t] + _x[t];
                b += f;
                f += Sum0Maj(g, h, a);
                ++t;

                e += Sum1Ch(b, c, d) + K[t] + _x[t];
                a += e;
                e += Sum0Maj(f, g, h);
                ++t;

                d += Sum1Ch(a, b, c) + K[t] + _x[t];
                h += d;
                d += Sum0Maj(e, f, g);
                ++t;

                c += Sum1Ch(h, a, b) + K[t] + _x[t];
                g += c;
                c += Sum0Maj(d, e, f);
                ++t;

                b += Sum1Ch(g, h, a) + K[t] + _x[t];
                f += b;
                b += Sum0Maj(c, d, e);
                ++t;

                a += Sum1Ch(f, g, h) + K[t] + _x[t];
                e += a;
                a += Sum0Maj(b, c, d);
                ++t;
            }

            _h1 += a;
            _h2 += b;
            _h3 += c;
            _h4 += d;
            _h5 += e;
            _h6 += f;
            _h7 += g;
            _h8 += h;
            _xOff = 0;
            Array.Clear(_x, 0, 16);
        }

        private static uint Sum1Ch(uint x, uint y, uint z) =>
            (((x >> 6) | (x << 26)) ^ ((x >> 11) | (x << 21)) ^ ((x >> 25) | (x << 7)))
            + (z ^ (x & (y ^ z)));
        private static uint Sum0Maj(uint x, uint y, uint z) => (((x >> 2) | (x << 30)) ^ ((x >> 13) | (x << 19)) ^ ((x >> 22) | (x << 10)))
                + ((x & y) | (z & (x ^ y)));
        private static uint Theta0(uint x) => ((x >> 7) | (x << 25)) ^ ((x >> 18) | (x << 14)) ^ (x >> 3);
        private static uint Theta1(uint x) => ((x >> 17) | (x << 15)) ^ ((x >> 19) | (x << 13)) ^ (x >> 10);
    }
}