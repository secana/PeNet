using System;
using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class ImageBaseRelocationTest
    {
        [Fact]
        public void SizeOfBlockIsBiggerThanRelocDirSize_Test()
        {
            var rawImageBaseRelocBroken = new byte[]
            {0x00, 0x00, 0x01, 0x00, 0x60, 0x00, 0x00, 0x00, 0x60, 0x30, 0xC4, 0x30};
            Assert.Throws<ArgumentOutOfRangeException>(() => new ImageBaseRelocation(new BufferFile(rawImageBaseRelocBroken), 0, 0));
        }

        [Fact]
        public void OffsetIsBiggerThanBuffer_Test()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ImageBaseRelocation(new BufferFile(RawStructures.RawImageBaseRelocation), 1234, 12));
        }

        [Fact]
        public void ImageBaseRelocationConstructorWorks_Test()
        {
            var ibr = new ImageBaseRelocation(new BufferFile(RawStructures.RawImageBaseRelocation), 2, 12);

            Assert.Equal((uint) 0x10000, ibr.VirtualAddress);
            Assert.Equal((uint) 0xc, ibr.SizeOfBlock);
            Assert.Equal(2, ibr.TypeOffsets.Length);
            Assert.Equal(0x2211 >> 12, ibr.TypeOffsets[0].Type);
            Assert.Equal(0x2211 & 0xfff, ibr.TypeOffsets[0].Offset);
            Assert.Equal(0x4433 >> 12, ibr.TypeOffsets[1].Type);
            Assert.Equal(0x4433 & 0xfff, ibr.TypeOffsets[1].Offset);
        }
    }
}