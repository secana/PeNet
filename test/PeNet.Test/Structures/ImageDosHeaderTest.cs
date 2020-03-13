using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class ImageDosHeaderTest
    {
        [Fact]
        public void ImageDosHeaderConstructorWorks_Test()
        {
            var idh = new ImageDosHeader(new BufferFile(RawStructures.RawDosHeader), 0);
            Assert.Equal((uint) 0x1100, idh.E_magic);
            Assert.Equal((uint) 0x3322, idh.E_cblp);
            Assert.Equal((uint) 0x5544, idh.E_cp);
            Assert.Equal((uint) 0x7766, idh.E_crlc);
            Assert.Equal((uint) 0x9988, idh.E_cparhdr);
            Assert.Equal((uint) 0xbbaa, idh.E_minalloc);
            Assert.Equal((uint) 0xddcc, idh.E_maxalloc);
            Assert.Equal((uint) 0x00ff, idh.E_ss);
            Assert.Equal((uint) 0x2211, idh.E_sp);
            Assert.Equal((uint) 0x4433, idh.E_csum);
            Assert.Equal((uint) 0x6655, idh.E_ip);
            Assert.Equal((uint) 0x8877, idh.E_cs);
            Assert.Equal((uint) 0xaa99, idh.E_lfarlc);
            Assert.Equal((uint) 0xccbb, idh.E_ovno);
            AssertEqual(new ushort[]
            {
                0xeedd,
                0x00ff,
                0x2211,
                0x4433
            }, idh.E_res);
            Assert.Equal((uint) 0x6655, idh.E_oemid);
            Assert.Equal((uint) 0x8877, idh.E_oeminfo);
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
            }, idh.E_res2);
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