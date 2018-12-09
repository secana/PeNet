using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class UNWIND_CODE_Test
    {
        [Fact]
        public void UnwindCodeConstructorWorks_Test()
        {
            var unwindCode = new UNWIND_CODE(RawStructures.RawUnwindCode, 2);

            Assert.Equal((byte) 0x11, unwindCode.CodeOffset);
            Assert.Equal((byte) 0x2, unwindCode.UnwindOp);
            Assert.Equal((byte) 0x3, unwindCode.Opinfo);
            Assert.Equal((ushort) 0x5544, unwindCode.FrameOffset);
        }
    }
}