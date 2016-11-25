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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PeNet.UnitTest.Utilities
{
    [TestClass]
    public class HeapOffsetBasedIndexSizes_Test
    {
        [TestMethod]
        public void StringIndexSizeShouldBeTwo_Test()
        {
            var heapOffsetBasedIndexSizes_1 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x00);
            var heapOffsetBasedIndexSizes_2 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x02);
            var heapOffsetBasedIndexSizes_3 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x22);

            Assert.AreEqual(2, heapOffsetBasedIndexSizes_1.StringIndexSize);
            Assert.AreEqual(2, heapOffsetBasedIndexSizes_2.StringIndexSize);
            Assert.AreEqual(2, heapOffsetBasedIndexSizes_3.StringIndexSize);
        }

        [TestMethod]
        public void StringIndexSizeShouldBeFour_Test()
        {
            var heapOffsetBasedIndexSizes_1 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x01);
            var heapOffsetBasedIndexSizes_2 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x21);
            var heapOffsetBasedIndexSizes_3 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x31);

            Assert.AreEqual(4, heapOffsetBasedIndexSizes_1.StringIndexSize);
            Assert.AreEqual(4, heapOffsetBasedIndexSizes_2.StringIndexSize);
            Assert.AreEqual(4, heapOffsetBasedIndexSizes_3.StringIndexSize);
        }

        [TestMethod]
        public void GuidIndexSizeShouldBeTwo_Test()
        {
            var heapOffsetBasedIndexSizes_1 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x00);
            var heapOffsetBasedIndexSizes_2 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x01);
            var heapOffsetBasedIndexSizes_3 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x04);

            Assert.AreEqual(2, heapOffsetBasedIndexSizes_1.GuidIndexSize);
            Assert.AreEqual(2, heapOffsetBasedIndexSizes_2.GuidIndexSize);
            Assert.AreEqual(2, heapOffsetBasedIndexSizes_3.GuidIndexSize);
        }

        [TestMethod]
        public void GuidIndexSizeShouldBeFour_Test()
        {
            var heapOffsetBasedIndexSizes_1 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x02);
            var heapOffsetBasedIndexSizes_2 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x03);
            var heapOffsetBasedIndexSizes_3 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x42);

            Assert.AreEqual(4, heapOffsetBasedIndexSizes_1.GuidIndexSize);
            Assert.AreEqual(4, heapOffsetBasedIndexSizes_2.GuidIndexSize);
            Assert.AreEqual(4, heapOffsetBasedIndexSizes_3.GuidIndexSize);
        }


        [TestMethod]
        public void BlobIndexSizeShouldBeTwo_Test()
        {
            var heapOffsetBasedIndexSizes_1 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x00);
            var heapOffsetBasedIndexSizes_2 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x01);
            var heapOffsetBasedIndexSizes_3 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x51);

            Assert.AreEqual(2, heapOffsetBasedIndexSizes_1.BlobSize);
            Assert.AreEqual(2, heapOffsetBasedIndexSizes_2.BlobSize);
            Assert.AreEqual(2, heapOffsetBasedIndexSizes_3.BlobSize);
        }

        [TestMethod]
        public void BlobIndexSizeShouldBeFour_Test()
        {
            var heapOffsetBasedIndexSizes_1 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x04);
            var heapOffsetBasedIndexSizes_2 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x54);
            var heapOffsetBasedIndexSizes_3 = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x05);

            Assert.AreEqual(4, heapOffsetBasedIndexSizes_1.BlobSize);
            Assert.AreEqual(4, heapOffsetBasedIndexSizes_2.BlobSize);
            Assert.AreEqual(4, heapOffsetBasedIndexSizes_3.BlobSize);
        }

        [TestMethod]
        public void AllIndexesShouldBeFour_Test()
        {
            var heapOffsetBasedIndexSizes = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x07);
            Assert.AreEqual(4, heapOffsetBasedIndexSizes.StringIndexSize);
            Assert.AreEqual(4, heapOffsetBasedIndexSizes.GuidIndexSize);
            Assert.AreEqual(4, heapOffsetBasedIndexSizes.BlobSize);
        }

        [TestMethod]
        public void AllIndexesShouldBeTwo_Test()
        {
            var heapOffsetBasedIndexSizes = new PeNet.Utilities.HeapOffsetBasedIndexSizes(0x00);
            Assert.AreEqual(2, heapOffsetBasedIndexSizes.StringIndexSize);
            Assert.AreEqual(2, heapOffsetBasedIndexSizes.GuidIndexSize);
            Assert.AreEqual(2, heapOffsetBasedIndexSizes.BlobSize);
        }
    }
}