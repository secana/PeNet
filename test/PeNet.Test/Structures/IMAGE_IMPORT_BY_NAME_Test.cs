using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_IMPORT_BY_NAME_Test
    {
        [Fact]
        public void ImageImportByNameConstructorWorks_Test()
        {
            var importByName = new IMAGE_IMPORT_BY_NAME(new BufferFile(RawStructures.RawImportByName), 2);
            Assert.Equal((ushort) 0x1100, importByName.Hint);
            Assert.Equal("Hello World", importByName.Name);
        }
    }
}