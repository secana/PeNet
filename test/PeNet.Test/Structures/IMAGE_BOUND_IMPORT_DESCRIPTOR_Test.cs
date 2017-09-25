using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_BOUND_IMPORT_DESCRIPTOR_Test
    {
        [Fact]
        public void ImageBoundImportDescriptorConstructorWorks_Test()
        {
            var boundImportDescriptor = new IMAGE_BOUND_IMPORT_DESCRIPTOR(RawStructures.RawBoundImportDescriptor, 2);
            Assert.Equal((uint) 0x33221100, boundImportDescriptor.TimeDateStamp);
            Assert.Equal((ushort) 0x5544, boundImportDescriptor.OffsetModuleName);
            Assert.Equal((ushort) 0x7766, boundImportDescriptor.NumberOfModuleForwarderRefs);
        }
    }
}