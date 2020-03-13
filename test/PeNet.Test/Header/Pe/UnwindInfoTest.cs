using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Header.Pe
{
    
    public class UnwindInfoTest
    {
        [Fact]
        public void UnwindInfoConstructorWorks_Test()
        {
            var unwindInfo = new UnwindInfo(new BufferFile(RawStructures.RawUnwindInfo), 2);
            Assert.Equal((byte) 0x1, unwindInfo.Version);
            Assert.Equal((byte) 0x12, unwindInfo.Flags);
            Assert.Equal((byte) 0x33, unwindInfo.SizeOfProlog);
            Assert.Equal((byte) 0x5, unwindInfo.FrameRegister);
            Assert.Equal((byte) 0x6, unwindInfo.FrameOffset);

            Assert.Single(unwindInfo.UnwindCode);
            Assert.Equal((byte) 0x77, unwindInfo.UnwindCode[0].CodeOffset);
            Assert.Equal(UnwindOpType.SaveXmm128, unwindInfo.UnwindCode[0].UnwindOp);
            Assert.Equal((byte) 0x9, unwindInfo.UnwindCode[0].Opinfo);

            Assert.Equal(0xffeeddcc, unwindInfo.ExceptionHandler);
        }
    }
}