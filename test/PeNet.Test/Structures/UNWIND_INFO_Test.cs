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

            Assert.Equal(1, unwindInfo.UnwindCode.Length);
            Assert.Equal((byte) 0x77, unwindInfo.UnwindCode[0].CodeOffset);
            Assert.Equal((byte) 0x8, unwindInfo.UnwindCode[0].UnwindOp);
            Assert.Equal((byte) 0x9, unwindInfo.UnwindCode[0].Opinfo);

            Assert.Equal(0xffeeddcc, unwindInfo.ExceptionHandler);
        }
    }
}