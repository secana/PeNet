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
    
    public class IMAGE_EXPORT_DIRECTORY_Test
    {
        [Fact]
        public void ImageExportDirectoryConstructorWorks_Test()
        {
            var exportDirectory = new IMAGE_EXPORT_DIRECTORY(RawStructures.RawExportDirectory, 2);

            Assert.Equal((uint) 0x33221100, exportDirectory.Characteristics);
            Assert.Equal((uint) 0x77665544, exportDirectory.TimeDateStamp);
            Assert.Equal((ushort) 0x9988, exportDirectory.MajorVersion);
            Assert.Equal((ushort) 0xbbaa, exportDirectory.MinorVersion);
            Assert.Equal(0xffeeddcc, exportDirectory.Name);
            Assert.Equal((uint) 0x55443322, exportDirectory.Base);
            Assert.Equal((uint) 0x44332211, exportDirectory.NumberOfFunctions);
            Assert.Equal(0x88776655, exportDirectory.NumberOfNames);
            Assert.Equal(0xccbbaa99, exportDirectory.AddressOfFunctions);
            Assert.Equal((uint) 0x00ffeedd, exportDirectory.AddressOfNames);
            Assert.Equal((uint) 0x55443322, exportDirectory.AddressOfNameOrdinals);
        }
    }
}