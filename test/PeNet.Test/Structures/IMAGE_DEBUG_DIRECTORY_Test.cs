using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_DEBUG_DIRECTORY_Test
    {
        [Fact]
        public void ImageDebugDirectoryConstructorWorks_Test()
        {
            var idd = new IMAGE_DEBUG_DIRECTORY(RawStructures.RawDebugDirectory, 2);

            Assert.Equal((uint) 0x44332211, idd.Characteristics);
            Assert.Equal(0x88776655, idd.TimeDateStamp);
            Assert.Equal((ushort) 0xaa99, idd.MajorVersion);
            Assert.Equal((ushort) 0xccbb, idd.MinorVersion);
            Assert.Equal((uint) 0x11ffeedd, idd.Type);
            Assert.Equal((uint) 0x55443322, idd.SizeOfData);
            Assert.Equal(0x99887766, idd.AddressOfRawData);
            Assert.Equal(0xddccbbaa, idd.PointerToRawData);
        }
    }
}