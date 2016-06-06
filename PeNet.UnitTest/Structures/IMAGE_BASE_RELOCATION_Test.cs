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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeNet.Structures;

namespace PeNet.UnitTest.Structures
{
    [TestClass]
    public class IMAGE_BASE_RELOCATION_Test
    {
        byte[] _rawImageBaseRelocCorrect = { 0xff, 0xff, 0x00, 0x00, 0x01, 0x00, 0x0c, 0x00, 0x00, 0x00, 0x11, 0x22, 0x33, 0x44 };

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SizeOfBlockIsBiggerThanRelocDirSize_Test()
        {
            var rawImageBaseRelocBroken = new byte[] {0x00, 0x00, 0x01, 0x00, 0x60, 0x00, 0x00, 0x00, 0x60, 0x30, 0xC4, 0x30};
            var ibr = new IMAGE_BASE_RELOCATION(rawImageBaseRelocBroken, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void OffsetIsBiggerThanBuffer_Test()
        {
            var ibr = new IMAGE_BASE_RELOCATION(_rawImageBaseRelocCorrect, 1234, 12);
        }

        [TestMethod]
        public void ImageBaseRelocationConstructorWorks_Test()
        {
            var ibr = new IMAGE_BASE_RELOCATION(_rawImageBaseRelocCorrect, 2, 12);

            Assert.AreEqual((uint) 0x10000, ibr.VirtualAddress);
            Assert.AreEqual((uint) 0xc, ibr.SizeOfBlock);
            Assert.AreEqual(2, ibr.TypeOffsets.Length);
            Assert.AreEqual(0x2211 & 0xF, ibr.TypeOffsets[0].Type);
            Assert.AreEqual(0x2211 >> 4, ibr.TypeOffsets[0].Offset);
            Assert.AreEqual(0x4433 & 0xF, ibr.TypeOffsets[1].Type);
            Assert.AreEqual(0x4433 >> 4, ibr.TypeOffsets[1].Offset);
        }
    }
}