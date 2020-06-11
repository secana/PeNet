using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Header.Pe
{
    public class ImageResourceDirStringUTest
    {
        [Fact]
        public void ImageResourceDirStringUConstructorWorks_Test()
        {
            var resourceDirStringU = new ImageResourceDirStringU(new BufferFile(RawStructures.RawResourceDirStringU), 2);
            Assert.Equal((uint) 0x000b, resourceDirStringU.Length);
            Assert.Equal("Hello World", resourceDirStringU.NameString);

            var secondResourceDirStringU = new ImageResourceDirStringU(new BufferFile(RawStructures.RawResourceDirStringU), 2 + 2 + (uint) 0x000b * 2 /* start offset + size of first length + length of first in bytes */);
            Assert.Equal((uint) 0x0006, secondResourceDirStringU.Length);
            Assert.Equal("Second", secondResourceDirStringU.NameString);
        }
    }
}