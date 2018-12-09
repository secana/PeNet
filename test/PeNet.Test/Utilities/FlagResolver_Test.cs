using System.Linq;
using PeNet.Utilities;
using Xunit;

namespace PeNet.Test.Utilities
{
    public class FlagResolver_Test
    {
        [Fact]
        public void ResolveCOMImageFlagsSingleFlags_Test()
        {
            Assert.Equal("COMIMAGE_FLAGS_ILONLY", FlagResolver.ResolveCOMImageFlags(0x00000001).First());
            Assert.Equal("COMIMAGE_FLAGS_32BITREQUIRED", FlagResolver.ResolveCOMImageFlags(0x00000002).First());
            Assert.Equal("COMIMAGE_FLAGS_IL_LIBRARY", FlagResolver.ResolveCOMImageFlags(0x000000004).First());
            Assert.Equal("COMIMAGE_FLAGS_STRONGNAMESIGNED", FlagResolver.ResolveCOMImageFlags(0x00000008).First());
            Assert.Equal("COMIMAGE_FLAGS_NATIVE_ENTRYPOINT", FlagResolver.ResolveCOMImageFlags(0x00000010).First());
            Assert.Equal("COMIMAGE_FLAGS_TRACKDEBUGDATA", FlagResolver.ResolveCOMImageFlags(0x00010000).First());
        }

        [Fact]
        public void ResolveCOMIMagesFlagsMultipleFlags_Test()
        {
            uint flags = 0x00010005;
            var resolved = FlagResolver.ResolveCOMImageFlags(flags);

            Assert.Equal(3, resolved.Count);
            Assert.Equal("COMIMAGE_FLAGS_ILONLY", resolved[0]);
            Assert.Equal("COMIMAGE_FLAGS_IL_LIBRARY", resolved[1]);
            Assert.Equal("COMIMAGE_FLAGS_TRACKDEBUGDATA", resolved[2]);
        }

        [Fact]
        public void ResolveMaskValidSingleFlags_Test()
        {
            Assert.Equal("Module", FlagResolver.ResolveMaskValidFlags(1).First());
            Assert.Equal("TypeRef", FlagResolver.ResolveMaskValidFlags(2).First());
            Assert.Equal("TypeDef", FlagResolver.ResolveMaskValidFlags(4).First());
            Assert.Equal("Field", FlagResolver.ResolveMaskValidFlags(16).First());
            Assert.Equal("MethodDef", FlagResolver.ResolveMaskValidFlags(64).First());
            Assert.Equal("Param", FlagResolver.ResolveMaskValidFlags(256).First());
            Assert.Equal("InterfaceImpl", FlagResolver.ResolveMaskValidFlags(512).First());
            Assert.Equal("MemberRef", FlagResolver.ResolveMaskValidFlags(1024).First());
            Assert.Equal("Constant", FlagResolver.ResolveMaskValidFlags(2048).First());
            Assert.Equal("CustomAttribute", FlagResolver.ResolveMaskValidFlags(4096).First());
            Assert.Equal("FieldMarshal", FlagResolver.ResolveMaskValidFlags(8192).First());
            Assert.Equal("DeclSecurity", FlagResolver.ResolveMaskValidFlags(16384).First());
            Assert.Equal("ClassLayout", FlagResolver.ResolveMaskValidFlags(32768).First());
            Assert.Equal("FieldLayout", FlagResolver.ResolveMaskValidFlags(65536).First());
            Assert.Equal("StandAloneSig", FlagResolver.ResolveMaskValidFlags(131072).First());
            Assert.Equal("EventMap", FlagResolver.ResolveMaskValidFlags(262144).First());
            Assert.Equal("Event", FlagResolver.ResolveMaskValidFlags(1048576).First());
            Assert.Equal("PropertyMap", FlagResolver.ResolveMaskValidFlags(2097152).First());
            Assert.Equal("Property", FlagResolver.ResolveMaskValidFlags(8388608).First());
            Assert.Equal("MethodSemantics", FlagResolver.ResolveMaskValidFlags(16777216).First());
            Assert.Equal("MethodImpl", FlagResolver.ResolveMaskValidFlags(33554432).First());
            Assert.Equal("ModuleRef", FlagResolver.ResolveMaskValidFlags(67108864).First());
            Assert.Equal("TypeSpec", FlagResolver.ResolveMaskValidFlags(134217728).First());
            Assert.Equal("ImplMap", FlagResolver.ResolveMaskValidFlags(268435456).First());
            Assert.Equal("FieldRVA", FlagResolver.ResolveMaskValidFlags(536870912).First());
            Assert.Equal("Assembly", FlagResolver.ResolveMaskValidFlags(4294967296).First());
            Assert.Equal("AssemblyProcessor", FlagResolver.ResolveMaskValidFlags(8589934592).First());
            Assert.Equal("AssemblyOS", FlagResolver.ResolveMaskValidFlags(17179869184).First());
            Assert.Equal("AssemblyRef", FlagResolver.ResolveMaskValidFlags(34359738368).First());
            Assert.Equal("AssemblyRefProcessor", FlagResolver.ResolveMaskValidFlags(68719476736).First());
            Assert.Equal("AssemblyRefOS", FlagResolver.ResolveMaskValidFlags(137438953472).First());
            Assert.Equal("File", FlagResolver.ResolveMaskValidFlags(274877906944).First());
            Assert.Equal("ExportedType", FlagResolver.ResolveMaskValidFlags(549755813888).First());
            Assert.Equal("ManifestResource", FlagResolver.ResolveMaskValidFlags(1099511627776).First());
            Assert.Equal("NestedClass", FlagResolver.ResolveMaskValidFlags(2199023255552).First());
            Assert.Equal("GenericParam", FlagResolver.ResolveMaskValidFlags(4398046511104).First());
            Assert.Equal("GenericParamConstraint", FlagResolver.ResolveMaskValidFlags(17592186044416).First());
        }

        [Fact]
        public void ResolveMaskValidMutlipleFlags_Test()
        {
            ulong multipleFlags = 0x00000A0909A21F57;
            var tables = FlagResolver.ResolveMaskValidFlags(multipleFlags);

            Assert.Equal(19, tables.Count);
            Assert.Contains("Module", tables);
            Assert.Contains("TypeRef", tables);
            Assert.Contains("TypeDef", tables);
            Assert.Contains("Field", tables);
            Assert.Contains("MethodDef", tables);
            Assert.Contains("Param", tables);
            Assert.Contains("InterfaceImpl", tables);
            Assert.Contains("MemberRef", tables);
            Assert.Contains("Constant", tables);
            Assert.Contains("CustomAttribute", tables);
            Assert.Contains("StandAloneSig", tables);
            Assert.Contains("PropertyMap", tables);
            Assert.Contains("Property", tables);
            Assert.Contains("MethodSemantics", tables);
            Assert.Contains("TypeSpec", tables);
            Assert.Contains("Assembly", tables);
            Assert.Contains("AssemblyRef", tables);
            Assert.Contains("NestedClass", tables);
            Assert.Contains("MethodSpec", tables);
        }
    }
}