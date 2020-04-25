using PeNet.FileParser;
using System;
using System.IO;
using Xunit;

namespace PeNet.Test.FileParser
{
    public class MMFileTest
    {
        [Fact]
        public void Length_ReturnsCorrectLength()
        {
            using var mmf = new MMFile(@"Binaries/firefox_x86.exe");

            Assert.Equal(411_088, mmf.Length);
        }

        [Fact]
        public void AsSpan_ReturnsSpan()
        {
            using var mmf = new MMFile(@"Binaries/mmf_test.bin");

            var s = mmf.AsSpan(0x16, 3);

            Assert.Equal(3, s.Length);
            Assert.Equal(0x11, s[0]);
            Assert.Equal(0x22, s[1]);
            Assert.Equal(0x33, s[2]);
        }

        [Fact]
        public void ReadAsciiString_ReturnsString()
        {
            using var mmf = new MMFile(@"Binaries/mmf_test.bin");

            var actual = mmf.ReadAsciiString(0x26);

            Assert.Equal("hello world.", actual);
        }

        [Fact]
        public void ReadByte_ReturnsByte()
        {
            using var mmf = new MMFile(@"Binaries/mmf_test.bin");

            var actual = mmf.ReadByte(0x2A);

            Assert.Equal(0x6F, actual);
        }

        [Fact]
        public void ReadUInt_ReturnsUInt()
        {
            using var mmf = new MMFile(@"Binaries/mmf_test.bin");

            var actual = mmf.ReadUInt(0x16);

            Assert.Equal(0x44332211u, actual);
        }

        [Fact]
        public void ReadULong_ReturnsULong()
        {
            using var mmf = new MMFile(@"Binaries/mmf_test.bin");

            var actual = mmf.ReadULong(0x16);

            Assert.Equal(0x8877665544332211u, actual);
        }

        [Fact]
        public void ReadUnicodeString_ReturnsString()
        {
            using var mmf = new MMFile(@"Binaries/mmf_test.bin");

            var actual = mmf.ReadUnicodeString(0x34);

            Assert.Equal("hello", actual);
        }

        [Fact]
        public void ReadUShort_ReturnsUShort()
        {
            using var mmf = new MMFile(@"Binaries/mmf_test.bin");

            var actual = mmf.ReadUShort(0x16);

            Assert.Equal((ushort) 0x2211, actual);
        }

        [Fact]
        public void RemoveRange_RemovesRange()
        {
            using var mmf = new MMFile(@"Binaries/mmf_test.bin");

            Assert.Throws<NotImplementedException>(() => mmf.RemoveRange(2, 0x3E));
        }

        [Fact]
        public void ToArray_ReturnsArray()
        {
            using var mmf = new MMFile(@"Binaries/mmf_test.bin");

            var actual = mmf.ToArray();

            Assert.Equal(0x42, actual.Length);
        }

        [Fact]
        public void WriteByte_WritesByte()
        {
            File.Copy(@"Binaries/mmf_test.bin", @"Binaries/mmf_test_writebyte.bin", true);
            using var mmf = new MMFile(@"Binaries/mmf_test_writebyte.bin");

            mmf.WriteByte(0x15, 0xaf);

            Assert.Equal((byte) 0xaf, mmf.ReadByte(0x15));
        }

        [Fact]
        public void WriteBytes_WritesBytes()
        {
            File.Copy(@"Binaries/mmf_test.bin", @"Binaries/mmf_test_writebytes.bin", true);
            using var mmf = new MMFile(@"Binaries/mmf_test_writebytes.bin");

            mmf.WriteBytes(0x15, new byte[] { 0xaf, 0xbf });

            Assert.Equal(0xbfaf, mmf.ReadUShort(0x15));
        }

        [Fact]
        public void WriteUInt_WritesUInt()
        {
            File.Copy(@"Binaries/mmf_test.bin", @"Binaries/mmf_test_writeuint.bin", true);
            using var mmf = new MMFile(@"Binaries/mmf_test_writeuint.bin");

            mmf.WriteUInt(0x15, 0xafbfcfdf);

            Assert.Equal(0xafbfcfdfu, mmf.ReadUInt(0x15));
        }

        [Fact]
        public void WriteULong_WritesULong()
        {
            File.Copy(@"Binaries/mmf_test.bin", @"Binaries/mmf_test_writeulong.bin", true);
            using var mmf = new MMFile(@"Binaries/mmf_test_writeulong.bin");

            mmf.WriteULong(0x15, 0xafbfcfdfafbfcfdf);

            Assert.Equal(0xafbfcfdfafbfcfdf, mmf.ReadULong(0x15));
        }

        [Fact]
        public void WriteUShort_WritesUShort()
        {
            File.Copy(@"Binaries/mmf_test.bin", @"Binaries/mmf_test_writeushort.bin", true);
            using var mmf = new MMFile(@"Binaries/mmf_test_writeushort.bin");

            mmf.WriteUShort(0x15, 0xafbf);

            Assert.Equal((ushort) 0xafbf, mmf.ReadUShort(0x15));
        }
    }
}
