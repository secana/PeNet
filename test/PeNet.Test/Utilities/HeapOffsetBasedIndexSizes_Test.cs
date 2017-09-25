using Xunit;

namespace PeNet.Test.Utilities
{
    public class HeapOffsetBasedIndexSizes_Test
    {
        [Theory]
        [InlineData(0x00, 2)]
        [InlineData(0x02, 2)]
        [InlineData(0x22, 2)]
        [InlineData(0x01, 4)]
        [InlineData(0x21, 4)]
        [InlineData(0x31, 4)]
        public void StringIndexSize_Test(byte heapOffsetSizes, uint result)
        {
            var heapOffsetBasedIndexSizes = new PeNet.Utilities.HeapOffsetBasedIndexSizes(heapOffsetSizes);
            Assert.Equal(result, heapOffsetBasedIndexSizes.StringIndexSize);
        }

        [Theory]
        [InlineData(0x00, 2)]
        [InlineData(0x01, 2)]
        [InlineData(0x04, 2)]
        [InlineData(0x02, 4)]
        [InlineData(0x03, 4)]
        [InlineData(0x42, 4)]
        public void GuidIndexSizeShouldBeTwo_Test(byte heapOffsetSizes, uint result)
        {
            var heapOffsetBasedIndexSizes = new PeNet.Utilities.HeapOffsetBasedIndexSizes(heapOffsetSizes);
            Assert.Equal(result, heapOffsetBasedIndexSizes.GuidIndexSize);
        }

        [Theory]
        [InlineData(0x00, 2)]
        [InlineData(0x01, 2)]
        [InlineData(0x51, 2)]
        [InlineData(0x04, 4)]
        [InlineData(0x54, 4)]
        [InlineData(0x05, 4)]
        public void BlobIndexSizeShouldBeTwo_Test(byte heapOffsetSizes, uint result)
        {
            var heapOffsetBasedIndexSizes = new PeNet.Utilities.HeapOffsetBasedIndexSizes(heapOffsetSizes);

            Assert.Equal(result, heapOffsetBasedIndexSizes.BlobSize);
        }

        [Fact]
        public void AllIndexesShouldBeFour_Test()
        {
            var heapOffsetBasedIndexSizes = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x07);
            Assert.Equal((uint) 4, heapOffsetBasedIndexSizes.StringIndexSize);
            Assert.Equal((uint) 4, heapOffsetBasedIndexSizes.GuidIndexSize);
            Assert.Equal((uint) 4, heapOffsetBasedIndexSizes.BlobSize);
        }

        [Fact]
        public void AllIndexesShouldBeTwo_Test()
        {
            var heapOffsetBasedIndexSizes = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x00);
            Assert.Equal((uint) 2, heapOffsetBasedIndexSizes.StringIndexSize);
            Assert.Equal((uint) 2, heapOffsetBasedIndexSizes.GuidIndexSize);
            Assert.Equal((uint) 2, heapOffsetBasedIndexSizes.BlobSize);
        }
    }
}