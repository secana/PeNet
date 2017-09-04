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

using System.Linq;
using PeNet.Structures.MetaDataTables;
using Xunit;

namespace PeNet.UnitTest.Structures.MetaDataTables
{
    public class ModuleTable_Test
    {
        [Fact]
        public void ModuleTableConstructorWorksSmallIndexes_Test()
        {
            var moduleTable = new ModuleTable(RawDotNetStructures.RawModuleTableSmall, 0x02, 1, 0x00);

            Assert.Equal((uint) 1, moduleTable.NumberOfRows);
            Assert.Equal(1, moduleTable.Rows.Count);
            Assert.Equal((ushort) 0x2211, moduleTable.Rows.First().Generation);
            Assert.Equal((uint) 0x4433, moduleTable.Rows.First().Name);
            Assert.Equal((uint) 0x6655, moduleTable.Rows.First().Mvid);
            Assert.Equal((uint) 0x8877, moduleTable.Rows.First().EncId);
            Assert.Equal((uint) 0xaa99, moduleTable.Rows.First().EncBaseId);
            Assert.Equal((uint) 0x0a, moduleTable.Rows.First().Length);
            Assert.Equal((uint) 0x0a, moduleTable.Length);
        }

        [Fact]
        public void ModuleTableConstructorWorksBigIndexes_Test()
        {
            var moduleTable = new ModuleTable(RawDotNetStructures.RawModuleTableBig, 0x02, 1, 0x07);

            Assert.Equal((uint) 1, moduleTable.NumberOfRows);
            Assert.Equal(1, moduleTable.Rows.Count);
            Assert.Equal((ushort) 0x2211, moduleTable.Rows.First().Generation);
            Assert.Equal((uint) 0xbbaa4433, moduleTable.Rows.First().Name);
            Assert.Equal((uint) 0xbbaa6655, moduleTable.Rows.First().Mvid);
            Assert.Equal((uint) 0xbbaa8877, moduleTable.Rows.First().EncId);
            Assert.Equal((uint) 0xbbaaaa99, moduleTable.Rows.First().EncBaseId);
            Assert.Equal((uint) 0x12, moduleTable.Rows.First().Length);
            Assert.Equal((uint) 0x12, moduleTable.Length);
        }
    }
}