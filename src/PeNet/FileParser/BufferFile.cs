using System;
using System.IO;
using System.Text;

namespace PeNet.FileParser
{
    public class BufferFile : IRawFile
    {
        private byte[] _buff;

        public long Length => _buff.Length;

        public BufferFile(byte[] file) 
            => (_buff) = (file);

        public string ReadAsciiString(long offset)
        {
            static int GetCStringLength(byte[] buff, long stringOffset)
            {
                var offset = stringOffset;
                var length = 0;
                while (buff[offset] != 0x00)
                {
                    length++;
                    offset++;
                }
                return length;
            }

            var length = GetCStringLength(_buff, offset);
            var tmp = new char[length];
            for (long i = 0; i < length; i++)
            {
                tmp[i] = (char)_buff[offset + i];
            }

            return new string(tmp);
        }

        public Span<byte> AsSpan(long offset, long length) 
            => _buff.AsSpan((int) offset, (int) length);

        public string ReadUnicodeString(long offset)
        {
            var size = 1;
            for (var i = offset; i < _buff.Length - 1; i++)
            {
                if (_buff[i] == 0 && _buff[i + 1] == 0)
                {
                    break;
                }
                size++;
            }
            var bytes = new byte[size];

            Array.Copy(_buff, (int)offset, bytes, 0, size);
            return Encoding.Unicode.GetString(bytes);
        }

        public byte ReadByte(long offset) => _buff[offset];

        public uint ReadUInt(long offset)
            => BitConverter.ToUInt32(_buff, (int) offset);

        public ulong ReadULong(long offset)
            => BitConverter.ToUInt64(_buff, (int) offset);

        public ushort ReadUShort(long offset)
            => BitConverter.ToUInt16(_buff, (int) offset);

        public Stream ToStream() => new MemoryStream(_buff);

        public void WriteByte(long offset, byte value)
        {
            _buff[offset] = value;
        }

        public void WriteBytes(long offset, Span<byte> bytes)
        {
            for(var i = (int) offset; i < bytes.Length; i++)
            {
                _buff[offset] = bytes[i];
            }
        }

        public void WriteUInt(long offset, uint value)
        {
            var x = BitConverter.GetBytes(value);
            _buff[offset] = x[0];
            _buff[offset + 1] = x[1];
            _buff[offset + 2] = x[2];
            _buff[offset + 3] = x[3];
        }

        public void WriteULong(long offset, ulong value)
        {
            var x = BitConverter.GetBytes(value);
            _buff[offset] = x[0];
            _buff[offset + 1] = x[1];
            _buff[offset + 2] = x[2];
            _buff[offset + 3] = x[3];
            _buff[offset + 4] = x[4];
            _buff[offset + 5] = x[5];
            _buff[offset + 6] = x[6];
            _buff[offset + 7] = x[7];
        }

        public void WriteUShort(long offset, ushort value)
        {
            var x = BitConverter.GetBytes(value);
            _buff[offset] = x[0];
            _buff[offset + 1] = x[1];
        }

        public byte[] ToArray() => _buff;

        public void Dispose()
        {
            _buff = new byte[0];
        }
    }
}
