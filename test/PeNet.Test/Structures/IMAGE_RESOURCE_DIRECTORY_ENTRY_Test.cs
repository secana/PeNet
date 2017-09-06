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
    
    public class IMAGE_RESOURCE_DIRECTORY_ENTRY_Test
    {
        [Fact]
        public void ResourceDirectroyEntryByNameConstructorWorks_Test()
        {
            var resourceDirectroyEntry =
                new IMAGE_RESOURCE_DIRECTORY_ENTRY(RawStructures.RawResourceDirectoryEntryByName, 2, 2);

            Assert.True(resourceDirectroyEntry.IsNamedEntry);
            Assert.False(resourceDirectroyEntry.IsIdEntry);
            Assert.Equal(0x80332211, resourceDirectroyEntry.Name);
            Assert.Equal((uint) 0x55443322, resourceDirectroyEntry.OffsetToData);
        }

        [Fact]
        public void ResourceDirectroyEntryByIdConstructorWorks_Test()
        {
            var resourceDirectroyEntry =
                new IMAGE_RESOURCE_DIRECTORY_ENTRY(RawStructures.RawResourceDirectoryEntryById, 2, 2);

            Assert.True(resourceDirectroyEntry.IsIdEntry);
            Assert.False(resourceDirectroyEntry.IsNamedEntry);
            Assert.Equal((uint) 0x00332211 & 0xFFFF, resourceDirectroyEntry.ID);
            Assert.Equal((uint) 0x55443322, resourceDirectroyEntry.OffsetToData);
        }
    }
}