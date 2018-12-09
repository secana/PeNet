using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_RESOURCE_DATA_ENTRY_Test
    {
        [Fact]
        public void ImageResourceDataEntryConstructorWorks_Test()
        {
            var resourceDataEntry = new IMAGE_RESOURCE_DATA_ENTRY(RawStructures.RawResourceDataEntry, 2);

            Assert.Equal((uint) 0x33221100, resourceDataEntry.OffsetToData);
            Assert.Equal((uint) 0x77665544, resourceDataEntry.Size1);
            Assert.Equal(0xbbaa9988, resourceDataEntry.CodePage);
            Assert.Equal(0xffeeddcc, resourceDataEntry.Reserved);
        }
    }
}