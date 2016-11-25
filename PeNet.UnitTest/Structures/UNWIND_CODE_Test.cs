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

using Xunit;
using PeNet.Structures;

namespace PeNet.UnitTest.Structures
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