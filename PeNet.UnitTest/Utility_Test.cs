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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PeNet.UnitTest
{
    [TestClass]
    public class Utility_Test
    {
        [TestMethod]
        public void ResolveCOMImageFlagsSingleFlags_Test()
        {
            Assert.AreEqual("COMIMAGE_FLAGS_ILONLY", Utility.ResolveCOMImageFlags(0x00000001).First());
            Assert.AreEqual("COMIMAGE_FLAGS_32BITREQUIRED", Utility.ResolveCOMImageFlags(0x00000002).First());
            Assert.AreEqual("COMIMAGE_FLAGS_IL_LIBRARY", Utility.ResolveCOMImageFlags(0x000000004).First());
            Assert.AreEqual("COMIMAGE_FLAGS_STRONGNAMESIGNED", Utility.ResolveCOMImageFlags(0x00000008).First());
            Assert.AreEqual("COMIMAGE_FLAGS_NATIVE_ENTRYPOINT", Utility.ResolveCOMImageFlags(0x00000010).First());
            Assert.AreEqual("COMIMAGE_FLAGS_TRACKDEBUGDATA", Utility.ResolveCOMImageFlags(0x00010000).First());
        }

        [TestMethod]
        public void ResolveCOMIMagesFlagsMultipleFlags_Test()
        {
            uint flags = 0x00010005;
            var resolved = Utility.ResolveCOMImageFlags(flags);

            Assert.AreEqual(3, resolved.Count);
            Assert.AreEqual("COMIMAGE_FLAGS_ILONLY", resolved[0]);
            Assert.AreEqual("COMIMAGE_FLAGS_IL_LIBRARY", resolved[1]);
            Assert.AreEqual("COMIMAGE_FLAGS_TRACKDEBUGDATA", resolved[2]);
        }

        [TestMethod]
        public void ResolveMaskValidSingleFlags_Test()
        {
            Assert.AreEqual("Module", Utility.ResolveMaskValidFlags(1).First());
            Assert.AreEqual("TypeRef", Utility.ResolveMaskValidFlags(2).First());
            Assert.AreEqual("TypeDef", Utility.ResolveMaskValidFlags(4).First());
            Assert.AreEqual("Field", Utility.ResolveMaskValidFlags(16).First());
            Assert.AreEqual("MethodDef", Utility.ResolveMaskValidFlags(64).First());
            Assert.AreEqual("Param", Utility.ResolveMaskValidFlags(256).First());
            Assert.AreEqual("InterfaceImpl", Utility.ResolveMaskValidFlags(512).First());
            Assert.AreEqual("MemberRef", Utility.ResolveMaskValidFlags(1024).First());
            Assert.AreEqual("Constant", Utility.ResolveMaskValidFlags(2048).First());
            Assert.AreEqual("CustomAttribute", Utility.ResolveMaskValidFlags(4096).First());
            Assert.AreEqual("FieldMarshal", Utility.ResolveMaskValidFlags(8192).First());
            Assert.AreEqual("DeclSecurity", Utility.ResolveMaskValidFlags(16384).First());
            Assert.AreEqual("ClassLayout", Utility.ResolveMaskValidFlags(32768).First());
            Assert.AreEqual("FieldLayout", Utility.ResolveMaskValidFlags(65536).First());
            Assert.AreEqual("StandAloneSig", Utility.ResolveMaskValidFlags(131072).First());
            Assert.AreEqual("EventMap", Utility.ResolveMaskValidFlags(262144).First());
            Assert.AreEqual("Event", Utility.ResolveMaskValidFlags(1048576).First());
            Assert.AreEqual("PropertyMap", Utility.ResolveMaskValidFlags(2097152).First());
            Assert.AreEqual("Property", Utility.ResolveMaskValidFlags(8388608).First());
            Assert.AreEqual("MethodSemantics", Utility.ResolveMaskValidFlags(16777216).First());
            Assert.AreEqual("MethodImpl", Utility.ResolveMaskValidFlags(33554432).First());
            Assert.AreEqual("ModuleRef", Utility.ResolveMaskValidFlags(67108864).First());
            Assert.AreEqual("TypeSpec", Utility.ResolveMaskValidFlags(134217728).First());
            Assert.AreEqual("ImplMap", Utility.ResolveMaskValidFlags(268435456).First());
            Assert.AreEqual("FieldRVA", Utility.ResolveMaskValidFlags(536870912).First());
            Assert.AreEqual("Assembly", Utility.ResolveMaskValidFlags(4294967296).First());
            Assert.AreEqual("AssemblyProcessor", Utility.ResolveMaskValidFlags(8589934592).First());
            Assert.AreEqual("AssemblyOS", Utility.ResolveMaskValidFlags(17179869184).First());
            Assert.AreEqual("AssemblyRef", Utility.ResolveMaskValidFlags(34359738368).First());
            Assert.AreEqual("AssemblyRefProcessor", Utility.ResolveMaskValidFlags(68719476736).First());
            Assert.AreEqual("AssemblyRefOS", Utility.ResolveMaskValidFlags(137438953472).First());
            Assert.AreEqual("File", Utility.ResolveMaskValidFlags(274877906944).First());
            Assert.AreEqual("ExportedType", Utility.ResolveMaskValidFlags(549755813888).First());
            Assert.AreEqual("ManifestResource", Utility.ResolveMaskValidFlags(1099511627776).First());
            Assert.AreEqual("NestedClass", Utility.ResolveMaskValidFlags(2199023255552).First());
            Assert.AreEqual("GenericParam", Utility.ResolveMaskValidFlags(4398046511104).First());
            Assert.AreEqual("GenericParamConstraint", Utility.ResolveMaskValidFlags(17592186044416).First());
        }

        [TestMethod]
        public void ResolveMaskValidMutlipleFlags_Test()
        {
            ulong multipleFlags = 0x00000A0909A21F57;
            var tables = Utility.ResolveMaskValidFlags(multipleFlags);

            Assert.AreEqual(19, tables.Count);
            Assert.IsTrue(tables.Contains("Module"));
            Assert.IsTrue(tables.Contains("TypeRef"));
            Assert.IsTrue(tables.Contains("TypeDef"));
            Assert.IsTrue(tables.Contains("Field"));
            Assert.IsTrue(tables.Contains("MethodDef"));
            Assert.IsTrue(tables.Contains("Param"));
            Assert.IsTrue(tables.Contains("InterfaceImpl"));
            Assert.IsTrue(tables.Contains("MemberRef"));
            Assert.IsTrue(tables.Contains("Constant"));
            Assert.IsTrue(tables.Contains("CustomAttribute"));
            Assert.IsTrue(tables.Contains("StandAloneSig"));
            Assert.IsTrue(tables.Contains("PropertyMap"));
            Assert.IsTrue(tables.Contains("Property"));
            Assert.IsTrue(tables.Contains("MethodSemantics"));
            Assert.IsTrue(tables.Contains("TypeSpec"));
            Assert.IsTrue(tables.Contains("Assembly"));
            Assert.IsTrue(tables.Contains("AssemblyRef"));
            Assert.IsTrue(tables.Contains("NestedClass"));
            Assert.IsTrue(tables.Contains("MethodSpec"));
        }
    }
}