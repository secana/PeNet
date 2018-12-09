using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class UNWIND_INFO_Test
    {
        [Fact]
        public void UnwindInfoConstructorWorks_Test()
        {
            var unwindInfo = new UNWIND_INFO(RawStructures.RawUnwindInfo, 2);
            Assert.Equal((byte) 0x1, unwindInfo.Version);
            Assert.Equal((byte) 0x12, unwindInfo.Flags);
            Assert.Equal((byte) 0x33, unwindInfo.SizeOfProlog);
            Assert.Equal((byte) 0x5, unwindInfo.FrameRegister);
            Assert.Equal((byte) 0x6, unwindInfo.FrameOffset);

            Assert.Single(unwindInfo.UnwindCode);
            Assert.Equal((byte) 0x77, unwindInfo.UnwindCode[0].CodeOffset);
            Assert.Equal((byte) 0x8, unwindInfo.UnwindCode[0].UnwindOp);
            Assert.Equal((byte) 0x9, unwindInfo.UnwindCode[0].Opinfo);

            Assert.Equal(0xffeeddcc, unwindInfo.ExceptionHandler);
        }
    }
}