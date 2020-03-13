using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class ImageResourceDataEntryTest
    {
        [Fact]
        public void ImageResourceDataEntryConstructorWorks_Test()
        {
            var resourceDataEntry = new ImageResourceDataEntry(new BufferFile(RawStructures.RawResourceDataEntry), 2);

            Assert.Equal((uint) 0x33221100, resourceDataEntry.OffsetToData);
            Assert.Equal((uint) 0x77665544, resourceDataEntry.Size1);
            Assert.Equal(0xbbaa9988, resourceDataEntry.CodePage);
            Assert.Equal(0xffeeddcc, resourceDataEntry.Reserved);
        }
    }
}