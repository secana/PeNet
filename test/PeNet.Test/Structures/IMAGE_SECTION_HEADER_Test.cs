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
    
    public class IMAGE_SECTION_HEADER_Test
    {
        [Fact]
        public void ImageSectionHeaderConstructorWorks_Test()
        {
            var sectionHeader = new IMAGE_SECTION_HEADER(RawStructures.RawSectionHeader, 2);

            Assert.Equal(".data", PeNet.Utilities.FlagResolver.ResolveSectionName(sectionHeader.Name));
            Assert.Equal((uint) 0x33221100, sectionHeader.VirtualSize);
            Assert.Equal((uint) 0x77665544, sectionHeader.VirtualAddress);
            Assert.Equal(0xbbaa9988, sectionHeader.SizeOfRawData);
            Assert.Equal(0xffeeddcc, sectionHeader.PointerToRawData);
            Assert.Equal((uint) 0x44332211, sectionHeader.PointerToRelocations);
            Assert.Equal(0x88776655, sectionHeader.PointerToLinenumbers);
            Assert.Equal((ushort) 0xaa99, sectionHeader.NumberOfRelocations);
            Assert.Equal((ushort) 0xccbb, sectionHeader.NumberOfLinenumbers);
            Assert.Equal((uint) 0x00ffeedd, sectionHeader.Characteristics);
        }
    }
}