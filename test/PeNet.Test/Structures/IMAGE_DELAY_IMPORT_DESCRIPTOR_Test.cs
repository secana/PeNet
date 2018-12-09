using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_DELAY_IMPORT_DESCRIPTOR_Test
    {
        [Fact]
        public void ImageDelayImportDescriptorConstructorWorks_Test()
        {
            var delayImportDescriptor = new IMAGE_DELAY_IMPORT_DESCRIPTOR(RawStructures.RawDelayImportDescriptor, 0x2);
            Assert.Equal((uint) 0x33221100, delayImportDescriptor.grAttrs);
            Assert.Equal((uint) 0x77665544, delayImportDescriptor.szName);
            Assert.Equal((uint) 0xbbaa9988, delayImportDescriptor.phmod);
            Assert.Equal((uint) 0xffeeddcc, delayImportDescriptor.pIAT);
            Assert.Equal((uint) 0x44332211, delayImportDescriptor.pINT);
            Assert.Equal((uint) 0xccbbaa99, delayImportDescriptor.pUnloadIAT);
            Assert.Equal((uint) 0x00ffeedd, delayImportDescriptor.dwTimeStamp);
        }
    }
}