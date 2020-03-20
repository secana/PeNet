using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Header.Pe
{
    
    public class ImageImportByNameTest
    {
        [Fact]
        public void ImageImportByNameConstructorWorks_Test()
        {
            var importByName = new ImageImportByName(new BufferFile(RawStructures.RawImportByName), 2);
            Assert.Equal((ushort) 0x1100, importByName.Hint);
            Assert.Equal("Hello World", importByName.Name);
        }
    }
}