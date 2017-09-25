using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_DATA_DIRECTORY_Test
    {
        [Fact]
        public void ImageDataDirectoryConstructorWorks_Test()
        {
            var dataDirectory = new IMAGE_DATA_DIRECTORY(RawStructures.RawDataDirectory, 2);

            Assert.Equal((uint) 0x44332211, dataDirectory.VirtualAddress);
            Assert.Equal(0x88776655, dataDirectory.Size);
        }
    }
}