using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PeNet.FileParser
{
    public class StreamFile : IRawFile, IDisposable
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
                if(stringOffset < stream.Length)
                {
                    stream.Seek(stringOffset, SeekOrigin.Begin);
                    var currentLength = 0;
                    while (stream.ReadByte() != 0x00)
                    {
                        currentLength++;
                        if((currentLength + stringOffset) >= stream.Length)
                        {
                            return 0;
                        }
                    }
                    return currentLength;
                }
                else
                {
                    return 0;
                }
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
            var sLen = _stream.Length - offset < 0 ? offset - _stream.Length: _stream.Length - offset;
            
            while(true)
            {
                if (sLen == chars.Count || sLen == chars.Count-1) {
                    break;
                }
                
                var b1 = (byte) _stream.ReadByte();
                var b2 = (byte) _stream.ReadByte();

                if (b1 + b2 == 0)
                    break;

                chars.Add(b1);
                chars.Add(b2);
            }

            return Encoding.Unicode.GetString(chars.ToArray());
        }

        public string ReadUnicodeString(long offset, long length)
        {
            _stream.Seek(offset, SeekOrigin.Begin);
            var chars = new List<byte>();
            for (var i = 1; i <= length; i++)
            {
                var b1 = (byte) _stream.ReadByte();
                var b2 = (byte) _stream.ReadByte();

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
#if NET48 || NETSTANDARD2_0
            return BitConverter.ToUInt32(s.ToArray(), 0);
#else
            return BitConverter.ToUInt32(s);
#endif
        }

        public ulong ReadULong(long offset)
        {
            Span<byte> s = stackalloc byte[8];
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Read(s);
#if NET48 || NETSTANDARD2_0
            return BitConverter.ToUInt64(s.ToArray(), 0);
#else
            return BitConverter.ToUInt64(s);
#endif
        }

        public ushort ReadUShort(long offset)
        {
            Span<byte> s = stackalloc byte[2];
            _stream.Seek(offset, SeekOrigin.Begin);
            _stream.Read(s);
#if NET48 || NETSTANDARD2_0
            return BitConverter.ToUInt16(s.ToArray(), 0);
#else
            return BitConverter.ToUInt16(s);
#endif
        }

        public byte[] ToArray()
        {
            using var ms = new MemoryStream {Position = 0};
            _stream.Position = 0;
            _stream.CopyTo(ms);
            return ms.ToArray();
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

        public int AppendBytes(Span<byte> bytes)
        {
            throw new NotImplementedException("This features is not available for stream files.");
        }
    }
}
