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
using PeNet.Utilities;

namespace PeNet.UnitTest
{
    [TestClass]
    public class FlagResolver_Test
    {
        [TestMethod]
        public void ResolveCOMImageFlagsSingleFlags_Test()
        {
            Assert.AreEqual("COMIMAGE_FLAGS_ILONLY", FlagResolver.ResolveCOMImageFlags(0x00000001).First());
            Assert.AreEqual("COMIMAGE_FLAGS_32BITREQUIRED", FlagResolver.ResolveCOMImageFlags(0x00000002).First());
            Assert.AreEqual("COMIMAGE_FLAGS_IL_LIBRARY", FlagResolver.ResolveCOMImageFlags(0x000000004).First());
            Assert.AreEqual("COMIMAGE_FLAGS_STRONGNAMESIGNED", FlagResolver.ResolveCOMImageFlags(0x00000008).First());
            Assert.AreEqual("COMIMAGE_FLAGS_NATIVE_ENTRYPOINT", FlagResolver.ResolveCOMImageFlags(0x00000010).First());
            Assert.AreEqual("COMIMAGE_FLAGS_TRACKDEBUGDATA", FlagResolver.ResolveCOMImageFlags(0x00010000).First());
        }

        [TestMethod]
        public void ResolveCOMIMagesFlagsMultipleFlags_Test()
        {
            uint flags = 0x00010005;
            var resolved = FlagResolver.ResolveCOMImageFlags(flags);

            Assert.AreEqual(3, resolved.Count);
            Assert.AreEqual("COMIMAGE_FLAGS_ILONLY", resolved[0]);
            Assert.AreEqual("COMIMAGE_FLAGS_IL_LIBRARY", resolved[1]);
            Assert.AreEqual("COMIMAGE_FLAGS_TRACKDEBUGDATA", resolved[2]);
        }

        [TestMethod]
        public void ResolveMaskValidSingleFlags_Test()
        {
            Assert.AreEqual("Module", FlagResolver.ResolveMaskValidFlags(1).First());
            Assert.AreEqual("TypeRef", FlagResolver.ResolveMaskValidFlags(2).First());
            Assert.AreEqual("TypeDef", FlagResolver.ResolveMaskValidFlags(4).First());
            Assert.AreEqual("Field", FlagResolver.ResolveMaskValidFlags(16).First());
            Assert.AreEqual("MethodDef", FlagResolver.ResolveMaskValidFlags(64).First());
            Assert.AreEqual("Param", FlagResolver.ResolveMaskValidFlags(256).First());
            Assert.AreEqual("InterfaceImpl", FlagResolver.ResolveMaskValidFlags(512).First());
            Assert.AreEqual("MemberRef", FlagResolver.ResolveMaskValidFlags(1024).First());
            Assert.AreEqual("Constant", FlagResolver.ResolveMaskValidFlags(2048).First());
            Assert.AreEqual("CustomAttribute", FlagResolver.ResolveMaskValidFlags(4096).First());
            Assert.AreEqual("FieldMarshal", FlagResolver.ResolveMaskValidFlags(8192).First());
            Assert.AreEqual("DeclSecurity", FlagResolver.ResolveMaskValidFlags(16384).First());
            Assert.AreEqual("ClassLayout", FlagResolver.ResolveMaskValidFlags(32768).First());
            Assert.AreEqual("FieldLayout", FlagResolver.ResolveMaskValidFlags(65536).First());
            Assert.AreEqual("StandAloneSig", FlagResolver.ResolveMaskValidFlags(131072).First());
            Assert.AreEqual("EventMap", FlagResolver.ResolveMaskValidFlags(262144).First());
            Assert.AreEqual("Event", FlagResolver.ResolveMaskValidFlags(1048576).First());
            Assert.AreEqual("PropertyMap", FlagResolver.ResolveMaskValidFlags(2097152).First());
            Assert.AreEqual("Property", FlagResolver.ResolveMaskValidFlags(8388608).First());
            Assert.AreEqual("MethodSemantics", FlagResolver.ResolveMaskValidFlags(16777216).First());
            Assert.AreEqual("MethodImpl", FlagResolver.ResolveMaskValidFlags(33554432).First());
            Assert.AreEqual("ModuleRef", FlagResolver.ResolveMaskValidFlags(67108864).First());
            Assert.AreEqual("TypeSpec", FlagResolver.ResolveMaskValidFlags(134217728).First());
            Assert.AreEqual("ImplMap", FlagResolver.ResolveMaskValidFlags(268435456).First());
            Assert.AreEqual("FieldRVA", FlagResolver.ResolveMaskValidFlags(536870912).First());
            Assert.AreEqual("Assembly", FlagResolver.ResolveMaskValidFlags(4294967296).First());
            Assert.AreEqual("AssemblyProcessor", FlagResolver.ResolveMaskValidFlags(8589934592).First());
            Assert.AreEqual("AssemblyOS", FlagResolver.ResolveMaskValidFlags(17179869184).First());
            Assert.AreEqual("AssemblyRef", FlagResolver.ResolveMaskValidFlags(34359738368).First());
            Assert.AreEqual("AssemblyRefProcessor", FlagResolver.ResolveMaskValidFlags(68719476736).First());
            Assert.AreEqual("AssemblyRefOS", FlagResolver.ResolveMaskValidFlags(137438953472).First());
            Assert.AreEqual("File", FlagResolver.ResolveMaskValidFlags(274877906944).First());
            Assert.AreEqual("ExportedType", FlagResolver.ResolveMaskValidFlags(549755813888).First());
            Assert.AreEqual("ManifestResource", FlagResolver.ResolveMaskValidFlags(1099511627776).First());
            Assert.AreEqual("NestedClass", FlagResolver.ResolveMaskValidFlags(2199023255552).First());
            Assert.AreEqual("GenericParam", FlagResolver.ResolveMaskValidFlags(4398046511104).First());
            Assert.AreEqual("GenericParamConstraint", FlagResolver.ResolveMaskValidFlags(17592186044416).First());
        }

        [TestMethod]
        public void ResolveMaskValidMutlipleFlags_Test()
        {
            ulong multipleFlags = 0x00000A0909A21F57;
            var tables = FlagResolver.ResolveMaskValidFlags(multipleFlags);

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