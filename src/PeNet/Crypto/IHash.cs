using System;

namespace PeNet.Crypto
{
    public interface IHash
    {
        string Compute(Span<byte> input);
    }
}