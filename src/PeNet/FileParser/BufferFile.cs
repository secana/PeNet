using System;
using System.IO;

namespace PeNet.FileParser
{
    class BufferFile : IRawFile
    {
        private readonly byte[] _buff;

        public long Length => throw new NotImplementedException();

        public BufferFile(byte[] file) => (_buff) = (file);

        public string GetCString(long offset)
        {
            throw new NotImplementedException();
        }

        public Span<byte> GetSpan(long offset, long length)
        {
            throw new NotImplementedException();
        }

        public string GetUnicodeString(long offset)
        {
            throw new NotImplementedException();
        }

        public byte ReadByte(long offset)
        {
            throw new NotImplementedException();
        }

        public uint ReadUInt(long offset)
        {
            throw new NotImplementedException();
        }

        public ulong ReadULong(long offset)
        {
            throw new NotImplementedException();
        }

        public ushort ReadUShort(long offset)
        {
            throw new NotImplementedException();
        }

        public Stream ToStream()
        {
            throw new NotImplementedException();
        }

        public void WriteByte(long offset, byte value)
        {
            throw new NotImplementedException();
        }

        public void WriteBytes(long offset, Span<byte> bytes)
        {
            throw new NotImplementedException();
        }

        public void WriteUInt(long offset, uint value)
        {
            throw new NotImplementedException();
        }

        public void WriteULong(long offset, ulong value)
        {
            throw new NotImplementedException();
        }

        public void WriteUShort(long offset, ushort value)
        {
            throw new NotImplementedException();
        }
    }
}
