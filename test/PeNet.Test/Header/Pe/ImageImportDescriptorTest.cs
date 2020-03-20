using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Header.Pe
{
    
    public class ImageImportDescriptorTest
    {
        [Fact]
        public void ImageImportDescriptorConstructorWorks_Test()
        {
            var importDescriptor = new ImageImportDescriptor(new BufferFile(RawStructures.RawImportDescriptor), 2);
            Assert.Equal((uint) 0x33221100, importDescriptor.OriginalFirstThunk);
            Assert.Equal((uint) 0x77665544, importDescriptor.TimeDateStamp);
            Assert.Equal(0xbbaa9988, importDescriptor.ForwarderChain);
            Assert.Equal(0xffeeddcc, importDescriptor.Name);
            Assert.Equal((uint) 0x44332211, importDescriptor.FirstThunk);
        }
    }
}