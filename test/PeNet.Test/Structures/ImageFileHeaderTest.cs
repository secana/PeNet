using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class ImageFileHeaderTest
    {
        [Fact]
        public void ImageFileHeaderConstructorWorks_Test()
        {
            var fileHeader = new ImageFileHeader(new BufferFile(RawStructures.RawFileHeader), 2);
            Assert.Equal(MachineType.AMD64, fileHeader.Machine);
            Assert.Equal("AMD64", fileHeader.MachineResolved);
            Assert.Equal((ushort) 0x3322, fileHeader.NumberOfSections);
            Assert.Equal((uint) 0x77665544, fileHeader.TimeDateStamp);
            Assert.Equal(0xbbaa9988, fileHeader.PointerToSymbolTable);
            Assert.Equal(0xffeeddcc, fileHeader.NumberOfSymbols);
            Assert.Equal((ushort) 0x2211, fileHeader.SizeOfOptionalHeader);
            Assert.True(fileHeader.Characteristics.DLL);
        }
    }
}