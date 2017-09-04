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
    
    public class IMAGE_DEBUG_DIRECTORY_Test
    {
        [Fact]
        public void ImageDebugDirectoryConstructorWorks_Test()
        {
            var idd = new IMAGE_DEBUG_DIRECTORY(RawStructures.RawDebugDirectory, 2);

            Assert.Equal((uint) 0x44332211, idd.Characteristics);
            Assert.Equal(0x88776655, idd.TimeDateStamp);
            Assert.Equal((ushort) 0xaa99, idd.MajorVersion);
            Assert.Equal((ushort) 0xccbb, idd.MinorVersion);
            Assert.Equal((uint) 0x11ffeedd, idd.Type);
            Assert.Equal((uint) 0x55443322, idd.SizeOfData);
            Assert.Equal(0x99887766, idd.AddressOfRawData);
            Assert.Equal(0xddccbbaa, idd.PointerToRawData);
        }
    }
}