using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Header.Pe
{
    
    public class ImageDataDirectoryTest
    {
        [Fact]
        public void ImageDataDirectoryConstructorWorks_Test()
        {
            var dataDirectory = new ImageDataDirectory(new BufferFile(RawStructures.RawDataDirectory), 2);

            Assert.Equal((uint) 0x44332211, dataDirectory.VirtualAddress);
            Assert.Equal(0x88776655, dataDirectory.Size);
        }
    }
}