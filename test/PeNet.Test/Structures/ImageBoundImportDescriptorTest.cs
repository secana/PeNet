using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class ImageBoundImportDescriptorTest
    {
        [Fact]
        public void ImageBoundImportDescriptorConstructorWorks_Test()
        {
            var boundImportDescriptor = new ImageBoundImportDescriptor(new BufferFile(RawStructures.RawBoundImportDescriptor), 2);
            Assert.Equal((uint) 0x33221100, boundImportDescriptor.TimeDateStamp);
            Assert.Equal((ushort) 0x5544, boundImportDescriptor.OffsetModuleName);
            Assert.Equal((ushort) 0x7766, boundImportDescriptor.NumberOfModuleForwarderRefs);
        }
    }
}