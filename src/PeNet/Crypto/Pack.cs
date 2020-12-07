using System;

namespace PeNet.Crypto
{
    internal static class Pack
    {
        internal static void UInt32_To_BE(uint n, Span<byte> bs, int off)
        {
            bs[off] = (byte)(n >> 24);
            bs[off + 1] = (byte)(n >> 16);
            bs[off + 2] = (byte)(n >> 8);
            bs[off + 3] = (byte)(n);
        }

        internal static uint BE_To_UInt32(Span<byte> bs, int off)
        {
            return (uint)bs[off] << 24
                | (uint)bs[off + 1] << 16
                | (uint)bs[off + 2] << 8
                | (uint)bs[off + 3];
        }

        internal static void UInt32_To_LE(uint n, Span<byte> bs, int off)
        {
            bs[off] = (byte)(n);
            bs[off + 1] = (byte)(n >> 8);
            bs[off + 2] = (byte)(n >> 16);
            bs[off + 3] = (byte)(n >> 24);
        }

        internal static uint LE_To_UInt32(Span<byte> bs, int off)
        {
            return (uint)bs[off]
                | (uint)bs[off + 1] << 8
                | (uint)bs[off + 2] << 16
                | (uint)bs[off + 3] << 24;
        }
    }
}