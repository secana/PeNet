using System;
using System.Security.Cryptography;
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
        public void ConstructorWorks_Test()
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