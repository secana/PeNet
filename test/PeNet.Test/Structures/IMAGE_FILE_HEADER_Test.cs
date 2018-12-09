using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_FILE_HEADER_Test
    {
        [Fact]
        public void ImageFileHeaderConstructorWorks_Test()
        {
            var fileHeader = new IMAGE_FILE_HEADER(RawStructures.RawFileHeader, 2);
            Assert.Equal((ushort) 0x1100, fileHeader.Machine);
            Assert.Equal((ushort) 0x3322, fileHeader.NumberOfSections);
            Assert.Equal((uint) 0x77665544, fileHeader.TimeDateStamp);
            Assert.Equal(0xbbaa9988, fileHeader.PointerToSymbolTable);
            Assert.Equal(0xffeeddcc, fileHeader.NumberOfSymbols);
            Assert.Equal((ushort) 0x2211, fileHeader.SizeOfOptionalHeader);
            Assert.Equal((ushort) 0x4433, fileHeader.Characteristics);
        }
    }
}