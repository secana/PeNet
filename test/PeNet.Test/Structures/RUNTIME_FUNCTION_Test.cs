using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class RUNTIME_FUNCTION_Test
    {
        [Fact]
        public void RuntimeFunctionConstructorWorks_Test()
        {
            var runtimeFunction = new RUNTIME_FUNCTION(RawStructures.RawRuntimeFunction, 2, null);

            Assert.Equal((uint) 0x33221100, runtimeFunction.FunctionStart);
            Assert.Equal((uint) 0x77665544, runtimeFunction.FunctionEnd);
            Assert.Equal((uint) 0xbbaa9988, runtimeFunction.UnwindInfo);
        }
    }
}