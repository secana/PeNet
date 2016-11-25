/***********************************************************************
Copyright 2016 Stefan Hausotte

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*************************************************************************/

using Xunit;

namespace PeNet.UnitTest.Utilities
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
        public void StringIndexSize_Test(byte heapOffsetSizes, int result)
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
        public void GuidIndexSizeShouldBeTwo_Test(byte heapOffsetSizes, int result)
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
        public void BlobIndexSizeShouldBeTwo_Test(byte heapOffsetSizes, int result)
        {
            var heapOffsetBasedIndexSizes = new PeNet.Utilities.HeapOffsetBasedIndexSizes(heapOffsetSizes);

            Assert.Equal(result, heapOffsetBasedIndexSizes.BlobSize);
        }

        [Fact]
        public void AllIndexesShouldBeFour_Test()
        {
            var heapOffsetBasedIndexSizes = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x07);
            Assert.Equal(4, heapOffsetBasedIndexSizes.StringIndexSize);
            Assert.Equal(4, heapOffsetBasedIndexSizes.GuidIndexSize);
            Assert.Equal(4, heapOffsetBasedIndexSizes.BlobSize);
        }

        [Fact]
        public void AllIndexesShouldBeTwo_Test()
        {
            var heapOffsetBasedIndexSizes = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x00);
            Assert.Equal(2, heapOffsetBasedIndexSizes.StringIndexSize);
            Assert.Equal(2, heapOffsetBasedIndexSizes.GuidIndexSize);
            Assert.Equal(2, heapOffsetBasedIndexSizes.BlobSize);
        }
    }
}