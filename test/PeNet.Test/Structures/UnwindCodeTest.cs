using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class UnwindCodeTest
    {
        [Fact]
        public void UnwindCodeConstructorWorks_Test()
        {
            var unwindCode = new UnwindCode(new BufferFile(RawStructures.RawUnwindCode), 2);

            Assert.Equal((byte) 0x11, unwindCode.CodeOffset);
            Assert.Equal(UnwindOpType.AllocSmall, unwindCode.UnwindOp);
            Assert.Equal((byte) 0x3, unwindCode.Opinfo);
            Assert.Equal((ushort) 0x5544, unwindCode.FrameOffset);
        }
    }
}