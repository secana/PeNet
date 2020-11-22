using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace PeNet.FileParser
{
    /// <summary>
    /// Parse the PE file as a memory mapped file.
    /// This is useful for large files.
    /// </summary>
    public unsafe class MMFile : IRawFile, IDisposable
    {
        private const int MaxStackAlloc = 1024;
        private readonly MemoryMappedFile _mmf;
        private readonly MemoryMappedViewAccessor _va;
        private readonly byte* ptr;

        public MMFile(string file)
        {
            _mmf = MemoryMappedFile.CreateFromFile(file, FileMode.Open);
            _va = _mmf.CreateViewAccessor();
            _va.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
            Length = new FileInfo(file).Length;
        }

        public long Length { private set; get; }

        public int AppendBytes(Span<byte> bytes)
        {
            throw new NotImplementedException("This features is not available for memory mapped files.");
        }

        public Span<byte> AsSpan(long offset, long length)
        {
            return new Span<byte>(ptr + offset, (int) length);
        }

        public void Dispose()
        {
            _va.SafeMemoryMappedViewHandle.ReleasePointer();
            _va.Dispose();
            _mmf.Dispose();
            GC.SuppressFinalize(this);
        }

        public string ReadAsciiString(long offset)
        {
            static int GetCStringLength(MemoryMappedViewAccessor va, long stringOffset)
            {
                var currentOffset = stringOffset;
                var currentLength = 0;
                while (va.ReadByte(currentOffset) != 0x00)
                {
                    currentLength++;
                    currentOffset++;
                }
                return currentLength;
            }

            var length = GetCStringLength(_va, offset);

            var tmp = length > MaxStackAlloc
                ? new char[length]
                : stackalloc char[length];

            for (var i = 0; i < length; i++)
            {
                tmp[i] = (char)_va.ReadByte(offset + i);
            }

            return new string(tmp);
        }

        public byte ReadByte(long offset)
            => _va.ReadByte(offset);


        public uint ReadUInt(long offset)
            => _va.ReadUInt32(offset);

        public ulong ReadULong(long offset)
            => _va.ReadUInt64(offset);

        public string ReadUnicodeString(long offset)
        {
            var size = 1;
            for (var i = offset; i < Length - 1; i++)
            {
                if (_va.ReadByte(i) == 0 && _va.ReadByte(i + 1) == 0)
                {
                    break;
                }
                size++;
            }
            var bytes = new byte[size];

            _va.ReadArray(offset, bytes, 0, size);
            return Encoding.Unicode.GetString(bytes);
        }

        public string ReadUnicodeString(long offset, long length)
        {
            int size = (int) length * 2;
            var bytes = new byte[size];

            _va.ReadArray(offset, bytes, 0, size);
            return Encoding.Unicode.GetString(bytes);
        }
        
        public ushort ReadUShort(long offset)
            => _va.ReadUInt16(offset);

        public void RemoveRange(long offset, long length)
        {
            throw new NotImplementedException($"RemoveRange is not available for memory mapped files");
        }

        public byte[] ToArray()
        {
            var array = new byte[Length];
            _va.ReadArray(0, array, 0, (int) Length);

            return array;
        }

        public void WriteByte(long offset, byte value)
        {
            var tmp = new byte[] { value };
            _va.WriteArray(offset, tmp, 0, 1);

        }

        public void WriteBytes(long offset, Span<byte> bytes)
            => _va.WriteArray(offset, bytes.ToArray(), 0, bytes.Length);

        public void WriteUInt(long offset, uint value)
        {
            var tmp = BitConverter.GetBytes(value);
            _va.WriteArray(offset, tmp, 0, 4);
        }

        public void WriteULong(long offset, ulong value)
        {
            var tmp = BitConverter.GetBytes(value);
            _va.WriteArray(offset, tmp, 0, 8);
        }

        public void WriteUShort(long offset, ushort value)
        {
            var tmp = BitConverter.GetBytes(value);
            _va.WriteArray(offset, tmp, 0, 2);
        }
    }
}
