using System.Linq;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Utilities
{
    public class FlagResolverTest
    {
        [Fact]
        public void ResolveCOMImageFlagsSingleFlags_Test()
        {
            Assert.Equal("IlOnly", ImageCor20Header.ResolveComFlags(ComFlagsType.IlOnly).First());
            Assert.Equal("BitRequired32", ImageCor20Header.ResolveComFlags(ComFlagsType.BitRequired32).First());
            Assert.Equal("IlLibrary", ImageCor20Header.ResolveComFlags(ComFlagsType.IlLibrary).First());
            Assert.Equal("StrongNameSigned", ImageCor20Header.ResolveComFlags(ComFlagsType.StrongNameSigned).First());
            Assert.Equal("NativeEntrypoint", ImageCor20Header.ResolveComFlags(ComFlagsType.NativeEntrypoint).First());
            Assert.Equal("TrackDebugData", ImageCor20Header.ResolveComFlags(ComFlagsType.TrackDebugData).First());
        }

        [Fact]
        public void ResolveCOMIMagesFlagsMultipleFlags_Test()
        {
            const uint flags = 0x00010005;
            var resolved = ImageCor20Header.ResolveComFlags((ComFlagsType) flags);

            Assert.Equal(3, resolved.Count);
            Assert.Equal("IlOnly", resolved[0]);
            Assert.Equal("IlLibrary", resolved[1]);
            Assert.Equal("TrackDebugData", resolved[2]);
        }

        [Fact]
        public void ResolveMaskValidSingleFlags_Test()
        {
            Assert.Equal("Module", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.Module).First());
            Assert.Equal("TypeRef", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.TypeRef).First());
            Assert.Equal("TypeDef", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.TypeDef).First());
            Assert.Equal("Field", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.Field).First());
            Assert.Equal("MethodDef", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.MethodDef).First());
            Assert.Equal("Param", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.Param).First());
            Assert.Equal("InterfaceImpl", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.InterfaceImpl).First());
            Assert.Equal("MemberRef", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.MemberRef).First());
            Assert.Equal("Constant", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.Constant).First());
            Assert.Equal("CustomAttribute", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.CustomAttribute).First());
            Assert.Equal("FieldMarshal", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.FieldMarshal).First());
            Assert.Equal("DeclSecurity", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.DeclSecurity).First());
            Assert.Equal("ClassLayout", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.ClassLayout).First());
            Assert.Equal("FieldLayout", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.FieldLayout).First());
            Assert.Equal("StandAloneSig", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.StandAloneSig).First());
            Assert.Equal("EventMap", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.EventMap).First());
            Assert.Equal("Event", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.Event).First());
            Assert.Equal("PropertyMap", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.PropertyMap).First());
            Assert.Equal("Property", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.Property).First());
            Assert.Equal("MethodSemantics", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.MethodSemantics).First());
            Assert.Equal("MethodImpl", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.MethodImpl).First());
            Assert.Equal("ModuleRef", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.ModuleRef).First());
            Assert.Equal("TypeSpec", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.TypeSpec).First());
            Assert.Equal("ImplMap", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.ImplMap).First());
            Assert.Equal("FieldRva", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.FieldRva).First());
            Assert.Equal("Assembly", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.Assembly).First());
            Assert.Equal("AssemblyProcessor", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.AssemblyProcessor).First());
            Assert.Equal("AssemblyOS", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.AssemblyOS).First());
            Assert.Equal("AssemblyRef", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.AssemblyRef).First());
            Assert.Equal("AssemblyRefProcessor", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.AssemblyRefProcessor).First());
            Assert.Equal("AssemblyRefOS", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.AssemblyRefOS).First());
            Assert.Equal("File", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.File).First());
            Assert.Equal("ExportedType", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.ExportedType).First());
            Assert.Equal("ManifestResource", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.ManifestResource).First());
            Assert.Equal("NestedClass", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.NestedClass).First());
            Assert.Equal("GenericParam", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.GenericParam).First());
            Assert.Equal("GenericParamConstraint", MetaDataTablesHdr.ResolveMaskValid(MaskValidType.GenericParamConstraint).First());
        }

        [Fact]
        public void ResolveMaskValidMutlipleFlags_Test()
        {
            var multipleFlags = (MaskValidType) 0x00000A0909A21F57;
            var tables = MetaDataTablesHdr.ResolveMaskValid(multipleFlags);

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