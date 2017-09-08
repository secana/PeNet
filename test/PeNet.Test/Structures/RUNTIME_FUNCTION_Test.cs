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