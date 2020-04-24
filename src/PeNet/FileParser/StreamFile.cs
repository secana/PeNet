using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PeNet.FileParser
{
    public class StreamFile : IRawFile
    {
        private const int MaxStackAlloc = 1024;
        private Stream _stream;

        public long Length => _stream.Length;

        public StreamFile(Stream file) 
            => (_stream) = (file);

        public string ReadAsciiString(long offset)
        {
            static int GetCStringLength(Stream stream, int stringOffset)
            {
                stream.Seek(stringOffset, SeekOrigin.Begin);
                var currentLength = 0;
                while (stream.ReadByte() != 0x00)
                {
                    currentLength++;
                }
                return currentLength;
            }

            var length = GetCStringLength(_stream, (int)offset);

            var tmp = length > MaxStackAlloc
                ? new byte[length]
                : stackalloc byte[length];

            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Read(tmp);
            return Encoding.ASCII.GetString(tmp);
        }

        public Span<byte> AsSpan(long offset, long length)
        {
            Span<byte> s = new byte[(int)length]; 
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Read(s);
            return s;
        }

        public string ReadUnicodeString(long offset)
        {
            _stream.Seek(offset, SeekOrigin.Begin);
            var chars = new List<byte>();
            while(true)
            {
                var b1 = (byte) _stream.ReadByte();
                var b2 = (byte) _stream.ReadByte();

                if (b1 + b2 == 0)
                    break;

                chars.Add(b1);
                chars.Add(b2);
            }

            return Encoding.Unicode.GetString(chars.ToArray());
        }

        public byte ReadByte(long offset)
        {
            _stream.Seek(offset, SeekOrigin.Begin);
            return (byte) _stream.ReadByte();
        }

        public uint ReadUInt(long offset)
        {
            Span<byte> s = stackalloc byte[4];
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Read(s);
            return BitConverter.ToUInt32(s);
        }

        public ulong ReadULong(long offset)
        {
            Span<byte> s = stackalloc byte[8];
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Read(s);
            return BitConverter.ToUInt64(s);
        }

        public ushort ReadUShort(long offset)
        {
            Span<byte> s = stackalloc byte[2];
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Read(s);
            return BitConverter.ToUInt16(s);
        }

        public byte[] ToArray()
        {
            using var ms = new MemoryStream {Position = 0};
            _stream.Position = 0;
            _stream.CopyTo(ms);
            return ms.ToArray();
        }

        public Stream ToStream()
        {
            _stream.Position = 0;
            return _stream;
        }

        public void WriteByte(long offset, byte value)
        {
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.WriteByte(value);
        }

        public void WriteBytes(long offset, Span<byte> bytes)
        {
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Write(bytes);
        }

        public void WriteUInt(long offset, uint value)
        {
            Span<byte> s = BitConverter.GetBytes(value);
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Write(s);
        }

        public void WriteULong(long offset, ulong value)
        {
            Span<byte> s = BitConverter.GetBytes(value);
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Write(s);
        }

        public void WriteUShort(long offset, ushort value)
        {
            Span<byte> s = BitConverter.GetBytes(value);
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Write(s);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public void RemoveRange(long offset, long length)
        {
            var _buff = this.ToArray();
            var x = _buff.ToList();
            x.RemoveRange((int) offset, (int) length);
            _stream.Dispose();
            _stream = new MemoryStream(_buff.ToArray());
        }
    }
}
