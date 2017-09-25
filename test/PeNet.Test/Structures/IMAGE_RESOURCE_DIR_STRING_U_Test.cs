using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_RESOURCE_DIR_STRING_U_Test
    {
        [Fact]
        public void ImageResourceDirStringUConstructorWorks_Test()
        {
            var resourceDirStringU = new IMAGE_RESOURCE_DIR_STRING_U(RawStructures.RawResourceDirStringU, 2);
            Assert.Equal((uint) 0x000b, resourceDirStringU.Length);
            Assert.Equal("Hello World", resourceDirStringU.NameString);
        }
    }
}