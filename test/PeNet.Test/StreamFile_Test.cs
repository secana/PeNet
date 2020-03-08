using PeNet.FileParser;
using System.IO;
using Xunit;

namespace PeNet.Test
{
    public class StreamFile_Test
    {
        static byte [] _buff = new byte[]
            {
                0x00, 0x11, 0x22, 0x33,
                0x44, 0x55, 0x66, 0x77,
                0x88, 0x99, 0xaa, 0xbb,
                0xcc, 0xdd, 0xee, 0xff
            };
        static MemoryStream _ms = new MemoryStream(_buff);
        static StreamFile _sf = new StreamFile(_ms);

        [Fact]
        public void Length_GivenAFileStream_ReturnsLength()
        {
            using var fs = File.Open(@"./Binaries/pidgin.exe", FileMode.Open);
            var file = new StreamFile(fs);

            Assert.Equal(60176, file.Length);
        }

        [Fact]
        public void ReadAsciiString_GivenAStream_ReturnsAString()
        {
            var buff = new byte[] {
                0x00, 0x00, // garbage
                0x68, // h
                0x65, // e
                0x6c, // l
                0x6c, // l
                0x6f, // o
                0x00, // string termination
                0x11, 0x22  // garbage
            };

            using var ms = new MemoryStream(buff);
            var file = new StreamFile(ms);

            Assert.Equal("hello", file.ReadAsciiString(2));
        }

        [Fact]
        public void ReadUnicodeString_GivenAStream_ReturnsAString()
        {
            var buff = new byte[] {
                0x00, 0x00, // garbage
                0x68, 0x00, // h
                0x65, 0x00, // e
                0x6c, 0x00, // l
                0x6c, 0x00, // l
                0x6f, 0x00, // o
                0x00, 0x00, // string termination
                0x11, 0x22  // garbage
            };

            using var ms = new MemoryStream(buff);
            var file = new StreamFile(ms);

            Assert.Equal("hello", file.ReadUnicodeString(2));
        }

        [Fact]
        public void AsSpan_GivenAStream_ReturnsSpan()
        {
            
            var file = new StreamFile(_ms);

            var s = file.AsSpan(2, 2);

            Assert.Equal(0x22, s[0]);
            Assert.Equal(0x33, s[1]);
        }

        [Fact]
        public void ReadByte_GivenAStream_ReturnsAByte()
        {
            Assert.Equal(0x77, _sf.ReadByte(7));
        }

        [Fact]
        public void ReadUShort_GivenAStream_ReturnsUShort()
        {
            Assert.Equal(0x5544u, _sf.ReadUShort(4));
        }

        [Fact]
        public void ReadUInt_GivenAStream_ReturnsUInt()
        {
            Assert.Equal(0x77665544u, _sf.ReadUInt(4));
        }

        [Fact]
        public void ReadULong_GivenAStream_ReturnsULong()
        {
            Assert.Equal(0xbbaa998877665544u, _sf.ReadULong(4));
        }

        [Fact]
        public void ToArray_GivenAStream_ReturnsStreamAsArray()
        {
            var array = _sf.ToArray();

            Assert.Equal(0x00, array[0]);
            Assert.Equal(0xff, array[15]);
            Assert.Equal(16, array.Length);
        }

        [Fact]
        public void WriteByte_GivenAStreamAndOffset_WritesByteAtOffset()
        {
            var buff = new byte[]
            {
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
            };
            using var ms = new MemoryStream(buff);
            var sf = new StreamFile(ms);

            sf.WriteByte(2, 0xff);

            Assert.Equal(0xff, buff[2]);
        }

        [Fact]
        public void WriteBytes_GivenAStreamAndOffset_WritesBytesAtOffset()
        {
            var buff = new byte[]
            {
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
            };
            using var ms = new MemoryStream(buff);
            var sf = new StreamFile(ms);

            sf.WriteBytes(2, new byte[] { 0x11, 0x22 });

            Assert.Equal(0x11, buff[2]);
            Assert.Equal(0x22, buff[3]);
        }

        [Fact]
        public void WriteUInt_GivenAStreamAndOffset_WritesUIntAtOffset()
        {
            var buff = new byte[]
            {
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
            };
            using var ms = new MemoryStream(buff);
            var sf = new StreamFile(ms);

            sf.WriteUInt(2, 0x11223344);

            Assert.Equal(0x44, buff[2]);
            Assert.Equal(0x33, buff[3]);
            Assert.Equal(0x22, buff[4]);
            Assert.Equal(0x11, buff[5]);
        }

        [Fact]
        public void WriteULong_GivenAStreamAndOffset_WritesULongAtOffset()
        {
            var buff = new byte[]
            {
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
            };
            using var ms = new MemoryStream(buff);
            var sf = new StreamFile(ms);

            sf.WriteULong(2, 0x1122334455667788);

            Assert.Equal(0x88, buff[2]);
            Assert.Equal(0x77, buff[3]);
            Assert.Equal(0x66, buff[4]);
            Assert.Equal(0x55, buff[5]);
            Assert.Equal(0x44, buff[6]);
            Assert.Equal(0x33, buff[7]);
            Assert.Equal(0x22, buff[8]);
            Assert.Equal(0x11, buff[9]);
        }

        [Fact]
        public void WriteUShort_GivenAStreamAndOffset_WritesUShortAtOffset()
        {
            var buff = new byte[]
            {
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
            };
            using var ms = new MemoryStream(buff);
            var sf = new StreamFile(ms);

            sf.WriteUShort(2, 0x1122);

            Assert.Equal(0x22, buff[2]);
            Assert.Equal(0x11, buff[3]);
        }
    }
}
