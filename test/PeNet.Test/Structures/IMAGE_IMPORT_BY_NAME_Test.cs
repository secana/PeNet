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
    
    public class IMAGE_IMPORT_BY_NAME_Test
    {
        [Fact]
        public void ImageImportByNameConstructorWorks_Test()
        {
            var importByName = new IMAGE_IMPORT_BY_NAME(RawStructures.RawImportByName, 2);
            Assert.Equal((ushort) 0x1100, importByName.Hint);
            Assert.Equal("Hello World", importByName.Name);
        }
    }
}