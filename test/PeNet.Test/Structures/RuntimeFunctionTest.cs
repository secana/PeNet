using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class RuntimeFunctionTest
    {
        [Fact]
        public void RuntimeFunctionConstructorWorks_Test()
        {
            var runtimeFunction = new RuntimeFunction(new BufferFile(RawStructures.RawRuntimeFunction), 2, null);

            Assert.Equal((uint) 0x33221100, runtimeFunction.FunctionStart);
            Assert.Equal((uint) 0x77665544, runtimeFunction.FunctionEnd);
            Assert.Equal((uint) 0xbbaa9988, runtimeFunction.UnwindInfo);
        }
    }
}