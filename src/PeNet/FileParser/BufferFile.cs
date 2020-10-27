using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PeNet.FileParser
{
    public class BufferFile : IRawFile
    {
        private Memory<byte> _buffer;

        public long Length => _buffer.Length;

        public BufferFile(byte[] file) => _buffer = file;

        public string ReadAsciiString(long offset)
        {
            var nullTerminator = byte.MinValue;

            var stringLength = _buffer.Span.Slice((int) offset).IndexOf(nullTerminator);

            return Encoding.ASCII.GetString(_buffer.Span.Slice((int) offset, stringLength));
        }

        public Span<byte> AsSpan(long offset, long length) => _buffer.Span.Slice((int) offset, (int) length);

        public string ReadUnicodeString(long offset)
        {
            Span<byte> nullTerminator = stackalloc byte[] {byte.MinValue, byte.MinValue};

            var stringLength = _buffer.Span.Slice((int) offset).IndexOf(nullTerminator) + 1;

            return Encoding.Unicode.GetString(_buffer.Span.Slice((int) offset, stringLength));
        }

        public string ReadUnicodeString(long offset, long length) => Encoding.Unicode.GetString(_buffer.Span.Slice((int) offset, (int) length * 2));

        public byte ReadByte(long offset) => _buffer.Span[(int) offset];

        public uint ReadUInt(long offset) => MemoryMarshal.Read<uint>(_buffer.Span.Slice((int) offset));

        public ulong ReadULong(long offset) => MemoryMarshal.Read<ulong>(_buffer.Span.Slice((int) offset));

        public ushort ReadUShort(long offset) => MemoryMarshal.Read<ushort>(_buffer.Span.Slice((int) offset));

        public void WriteByte(long offset, byte value) => _buffer.Span[(int) offset] = value;

        public void WriteBytes(long offset, Span<byte> bytes) => bytes.CopyTo(_buffer.Span.Slice((int) offset));

        public void WriteUInt(long offset, uint value) => MemoryMarshal.Write(_buffer.Span.Slice((int) offset), ref value);

        public void WriteULong(long offset, ulong value) => MemoryMarshal.Write(_buffer.Span.Slice((int) offset), ref value);

        public void WriteUShort(long offset, ushort value) => MemoryMarshal.Write(_buffer.Span.Slice((int) offset), ref value);

        public byte[] ToArray() => _buffer.ToArray();

        public void RemoveRange(long offset, long length)
        {
            var newBuffer = new Memory<byte>(new byte[_buffer.Length - length]);

            _buffer.Slice(0, (int) offset).CopyTo(newBuffer);
            _buffer.Slice((int) (offset + length)).CopyTo(newBuffer.Slice((int) offset));

            _buffer = newBuffer;
        }

        public int AppendBytes(Span<byte> bytes)
        {
            var oldLength = _buffer.Length;

            var newBuffer = new Memory<byte>(new byte[_buffer.Length + bytes.Length]);
            _buffer.CopyTo(newBuffer);
            bytes.CopyTo(newBuffer.Span.Slice(oldLength));
            _buffer = newBuffer;

            return oldLength;
        }
    }
}
