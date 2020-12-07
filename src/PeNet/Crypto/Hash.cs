using System;
using System.Text;

namespace PeNet.Crypto
{
    // For reference see: https://github.com/bcgit/bc-csharp/blob/master/crypto/src/crypto/digests/GeneralDigest.cs
    public abstract class Hash
    {
        private readonly byte[] _xBuf = new byte[4];
        private int _xBufOff;
        private long _byteCount;
        protected abstract int DigestLength { get; }

        private void Update(byte input)
        {
            _xBuf[_xBufOff++] = input;

            if (_xBufOff == _xBuf.Length)
            {
                ProcessWord(_xBuf, 0);
                _xBufOff = 0;
            }

            _byteCount++;
        }

        private void BlockUpdate(
            Span<byte> input,
            int inOff,
            int length)
        {
            length = System.Math.Max(0, length);
            var i = 0;
            if (_xBufOff != 0)
            {
                while (i < length)
                {
                    _xBuf[_xBufOff++] = input[inOff + i++];
                    if (_xBufOff != 4) continue;
                    ProcessWord(_xBuf, 0);
                    _xBufOff = 0;
                    break;
                }
            }

            var limit = ((length - i) & ~3) + i;
            for (; i < limit; i += 4)
            {
                ProcessWord(input, inOff + i);
            }

            while (i < length)
            {
                _xBuf[_xBufOff++] = input[inOff + i++];
            }

            _byteCount += length;
        }

        protected void Finish()
        {
            var bitLength = (_byteCount << 3);
            Update((byte)128);
            while (_xBufOff != 0) Update((byte)0);
            ProcessLength(bitLength);
            ProcessBlock();
        }

        protected virtual void Reset()
        {
            _byteCount = 0;
            _xBufOff = 0;
            Array.Clear(_xBuf, 0, _xBuf.Length);
        }

        private string Compute(Span<byte> input)
        {
            Span<byte> hash = stackalloc byte[DigestLength];
            BlockUpdate(input, 0, input.Length);
            DoFinal(hash, 0);

            var sBuilder = new StringBuilder();
            foreach (var t in hash)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }

        internal static string ComputeHash(Span<byte> input, Algorithm algorithm) =>
            algorithm switch
            {
                Algorithm.Md5 => new Md5().Compute(input),
                Algorithm.Sha1 => new Sha1().Compute(input),
                Algorithm.Sha256 => new Sha256().Compute(input),
                _ => throw new NotImplementedException()
            };

        protected abstract void ProcessWord(Span<byte> input, int inOff);
        protected abstract void ProcessLength(long bitLength);
        protected abstract void ProcessBlock();
        protected abstract void DoFinal(Span<byte> output, int outOff);
    }
}