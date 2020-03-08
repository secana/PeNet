using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PeNet.FileParser
{
    public class StreamFile : IRawFile
    {
        private readonly Stream _stream;

        public long Length => _stream.Length;

        public StreamFile(Stream file) 
            => (_stream) = (file);

        public string ReadAsciiString(long offset)
        {
            _stream.Seek(offset, SeekOrigin.Begin);
            var list = new List<char>();
            while(true)
            {
                var b = _stream.ReadByte();

                if(b == -1)
                {
                    break;
                }
                else if(b == 0)
                { 
                    break;
                }
                else
                {
                    list.Add((char)b);
                }
            }

            return new string(list.ToArray());
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
            using var ms = new MemoryStream();
            _stream.CopyTo(ms);
            return ms.ToArray();
        }

        public Stream ToStream()
            => _stream;

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
    }
}
