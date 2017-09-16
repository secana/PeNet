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
    
    public class IMAGE_IMPORT_DESCRIPTOR_Test
    {
        [Fact]
        public void ImageImportDescriptorConstructorWorks_Test()
        {
            var importDescriptor = new IMAGE_IMPORT_DESCRIPTOR(RawStructures.RawImportDescriptor, 2);
            Assert.Equal((uint) 0x33221100, importDescriptor.OriginalFirstThunk);
            Assert.Equal((uint) 0x77665544, importDescriptor.TimeDateStamp);
            Assert.Equal(0xbbaa9988, importDescriptor.ForwarderChain);
            Assert.Equal(0xffeeddcc, importDescriptor.Name);
            Assert.Equal((uint) 0x44332211, importDescriptor.FirstThunk);
        }
    }
}