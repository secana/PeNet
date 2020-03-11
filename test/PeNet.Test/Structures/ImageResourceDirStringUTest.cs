using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class ImageResourceDirStringUTest
    {
        [Fact]
        public void ImageResourceDirStringUConstructorWorks_Test()
        {
            var resourceDirStringU = new ImageResourceDirStringU(new BufferFile(RawStructures.RawResourceDirStringU), 2);
            Assert.Equal((uint) 0x000b, resourceDirStringU.Length);
            Assert.Equal("Hello World", resourceDirStringU.NameString);
        }
    }
}