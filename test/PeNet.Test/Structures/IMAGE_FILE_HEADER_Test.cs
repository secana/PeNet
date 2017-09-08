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
    
    public class IMAGE_FILE_HEADER_Test
    {
        [Fact]
        public void ImageFileHeaderConstructorWorks_Test()
        {
            var fileHeader = new IMAGE_FILE_HEADER(RawStructures.RawFileHeader, 2);
            Assert.Equal((ushort) 0x1100, fileHeader.Machine);
            Assert.Equal((ushort) 0x3322, fileHeader.NumberOfSections);
            Assert.Equal((uint) 0x77665544, fileHeader.TimeDateStamp);
            Assert.Equal(0xbbaa9988, fileHeader.PointerToSymbolTable);
            Assert.Equal(0xffeeddcc, fileHeader.NumberOfSymbols);
            Assert.Equal((ushort) 0x2211, fileHeader.SizeOfOptionalHeader);
            Assert.Equal((ushort) 0x4433, fileHeader.Characteristics);
        }
    }
}