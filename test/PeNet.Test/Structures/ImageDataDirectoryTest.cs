using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
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