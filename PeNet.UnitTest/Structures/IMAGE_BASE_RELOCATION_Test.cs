using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeNet.Structures;

namespace PeNet.UnitTest.Structures
{
    [TestClass]
    public class IMAGE_BASE_RELOCATION_Test
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SizeOfBlockIsBiggerThanRelocDirSize_Test()
        {
            var rawImageBaseReloc = new byte[] {0x00, 0x00, 0x01, 0x00, 0x60, 0x00, 0x00, 0x00, 0x60, 0x30, 0xC4, 0x30};
            var ibr = new IMAGE_BASE_RELOCATION(rawImageBaseReloc, 0, 0);
        }

        [TestMethod]
        public void ConstructorWorks_Test()
        {
            var rawImageBaseReloc = new byte[] {0xff, 0xff, 0x00, 0x00, 0x01, 0x00, 0x0c, 0x00, 0x00, 0x00, 0x11, 0x22, 0x33, 0x44};
            var ibr = new IMAGE_BASE_RELOCATION(rawImageBaseReloc, 2, 12);

            Assert.AreEqual((uint) 0x10000, ibr.VirtualAddress);
            Assert.AreEqual((uint) 0xc, ibr.SizeOfBlock);
            Assert.AreEqual(2, ibr.TypeOffsets.Length);
        }
    }
}