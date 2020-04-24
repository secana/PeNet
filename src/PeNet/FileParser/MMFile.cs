using System;
using System.Collections;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;

namespace PeNet.FileParser
{
    public class MMFile : IRawFile
    {
        private const int MaxStackAlloc = 1024;
        private MemoryMappedFile _mmf;
        private MemoryMappedViewAccessor _va;
        private MemoryMappedViewStream? _stream;
        private FileInfo _fileInfo;

        public MMFile(string file)
        {
            _mmf = MemoryMappedFile.CreateFromFile(file, FileMode.Open);
            _va = _mmf.CreateViewAccessor();
            _fileInfo = new FileInfo(file);
            RemoveRangeNewFileName = _fileInfo.FullName;
        }

        public string RemoveRangeNewFileName
        {
            get; set;
        }

        public long Length
            => _fileInfo.Length;

        public Span<byte> AsSpan(long offset, long length)
        {
            var array = new byte[length];
            _va.ReadArray(offset, array, 0, (int) length);

            return array.AsSpan();
        }

        public void Dispose()
        {
            _va.Dispose();
            _stream?.Dispose();
            _mmf.Dispose();
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

        public ushort ReadUShort(long offset)
            => _va.ReadUInt16(offset);

        public void RemoveRange(long offset, long length)
        {
            var list = ToArray().ToList();
            list.RemoveRange((int)offset, (int)length);

            _va.Dispose();
            _mmf.Dispose();
            var file = RemoveRangeNewFileName;

            File.WriteAllBytes(file, list.ToArray());

            _mmf = MemoryMappedFile.CreateFromFile(file, FileMode.Open);
            _va = _mmf.CreateViewAccessor();
            _fileInfo = new FileInfo(file);
        }

        public byte[] ToArray()
        {
            var array = new byte[Length];
            _va.ReadArray(0, array, 0, (int) Length);

            return array;
        }

        public Stream ToStream()
        {
            if (_stream is null)
                _stream = _mmf.CreateViewStream();

            return _stream;
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
