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
    
    public class IMAGE_RESOURCE_DIRECTORY_Test
    {
        [Fact]
        public void ImageResourceDirectoryConstructorWorks_Test()
        {
            var resourceDirectory = new IMAGE_RESOURCE_DIRECTORY(RawStructures.RawResourceDirectory, 2, 2);
            Assert.Equal((uint) 0x33221100, resourceDirectory.Characteristics);
            Assert.Equal((uint) 0x77665544, resourceDirectory.TimeDateStamp);
            Assert.Equal((ushort) 0x9988, resourceDirectory.MajorVersion);
            Assert.Equal((ushort) 0xbbaa, resourceDirectory.MinorVersion);
            Assert.Equal((ushort) 0x0001, resourceDirectory.NumberOfNameEntries);
            Assert.Equal((ushort) 0x0001, resourceDirectory.NumberOfIdEntries);
            Assert.Equal((uint) 0x44332211, resourceDirectory.DirectoryEntries[0].Name);
            Assert.Equal(0x88776655, resourceDirectory.DirectoryEntries[0].OffsetToData);
            Assert.Equal((uint) 0x44332222 & 0xFFFF, resourceDirectory.DirectoryEntries[1].ID);
            Assert.Equal(0x88776622, resourceDirectory.DirectoryEntries[1].OffsetToData);
        }
    }
}