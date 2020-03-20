using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Header.Pe
{
    
    public class ImageDelayImportDescriptorTest
    {
        [Fact]
        public void ImageDelayImportDescriptorConstructorWorks_Test()
        {
            var delayImportDescriptor = new ImageDelayImportDescriptor(new BufferFile(RawStructures.RawDelayImportDescriptor), 0x2);
            Assert.Equal((uint) 0x33221100, delayImportDescriptor.GrAttrs);
            Assert.Equal((uint) 0x77665544, delayImportDescriptor.SzName);
            Assert.Equal((uint) 0xbbaa9988, delayImportDescriptor.Phmod);
            Assert.Equal((uint) 0xffeeddcc, delayImportDescriptor.PIat);
            Assert.Equal((uint) 0x44332211, delayImportDescriptor.PInt);
            Assert.Equal((uint) 0xccbbaa99, delayImportDescriptor.PUnloadIAT);
            Assert.Equal((uint) 0x00ffeedd, delayImportDescriptor.DwTimeStamp);
        }
    }
}