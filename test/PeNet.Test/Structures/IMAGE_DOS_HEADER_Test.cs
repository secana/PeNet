using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_DOS_HEADER_Test
    {
        [Fact]
        public void ImageDosHeaderConstructorWorks_Test()
        {
            var idh = new IMAGE_DOS_HEADER(RawStructures.RawDosHeader, 0);
            Assert.Equal((uint) 0x1100, idh.e_magic);
            Assert.Equal((uint) 0x3322, idh.e_cblp);
            Assert.Equal((uint) 0x5544, idh.e_cp);
            Assert.Equal((uint) 0x7766, idh.e_crlc);
            Assert.Equal((uint) 0x9988, idh.e_cparhdr);
            Assert.Equal((uint) 0xbbaa, idh.e_minalloc);
            Assert.Equal((uint) 0xddcc, idh.e_maxalloc);
            Assert.Equal((uint) 0x00ff, idh.e_ss);
            Assert.Equal((uint) 0x2211, idh.e_sp);
            Assert.Equal((uint) 0x4433, idh.e_csum);
            Assert.Equal((uint) 0x6655, idh.e_ip);
            Assert.Equal((uint) 0x8877, idh.e_cs);
            Assert.Equal((uint) 0xaa99, idh.e_lfarlc);
            Assert.Equal((uint) 0xccbb, idh.e_ovno);
            AssertEqual(new ushort[]
            {
                0xeedd,
                0x00ff,
                0x2211,
                0x4433
            }, idh.e_res);
            Assert.Equal((uint) 0x6655, idh.e_oemid);
            Assert.Equal((uint) 0x8877, idh.e_oeminfo);
            AssertEqual(new ushort[]
            {
                0xaa99,
                0xccbb,
                0xeedd,
                0x11ff,
                0x3322,
                0x5544,
                0x7766,
                0x9988,
                0xbbaa,
                0xbbcc
            }, idh.e_res2);
        }

        private void AssertEqual(ushort[] expected, ushort[] actual)
        {
            Assert.Equal(expected.Length, actual.Length);
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }
    }
}