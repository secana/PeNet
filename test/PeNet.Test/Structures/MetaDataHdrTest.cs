using System.Linq;
using PeNet.FileParser;
using PeNet.Header.Net;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class MetaDataHdrTest
    {
        [Fact]
        public void MetaDataHdrConstructorWorks_Test()
        {
            var metaDataHdr = new MetaDataHdr(new BufferFile(RawDotNetStructures.RawMetaDataHeader), 2);
            Assert.Equal((uint) 0x55443322, metaDataHdr.Signature);
            Assert.Equal((ushort) 0x7766, metaDataHdr.MajorVersion);
            Assert.Equal((ushort) 0x9988, metaDataHdr.MinorVersion);
            Assert.Equal((uint) 0xddccbbaa, metaDataHdr.Reserved);
            Assert.Equal((uint) 0x0000000C, metaDataHdr.VersionLength);
            Assert.Equal("v4.0.30319", metaDataHdr.Version);
            Assert.Equal((ushort) 0x2211, metaDataHdr.Flags);
            Assert.Equal((ushort) 0x0002, metaDataHdr.Streams);

            Assert.Equal(2, metaDataHdr.MetaDataStreamsHdrs.Length);
            Assert.Equal((uint) 0x6C, metaDataHdr.MetaDataStreamsHdrs[0].RelOffset);
            Assert.Equal((uint) 0x1804, metaDataHdr.MetaDataStreamsHdrs[0].Size);
            Assert.Equal("#~", metaDataHdr.MetaDataStreamsHdrs[0].StreamName);
            Assert.Equal((uint) 0x1870, metaDataHdr.MetaDataStreamsHdrs[1].RelOffset);
            Assert.Equal((uint) 0x1468, metaDataHdr.MetaDataStreamsHdrs[1].Size);
            Assert.Equal("#Strings", metaDataHdr.MetaDataStreamsHdrs[1].StreamName);
        }

        [Fact]
        public void ResolveMaskValid_Single_Test()
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
        public void ResolveMaskValid_Multiple_Test()
        {
            var multipleFlags = (MaskValidType)0x00000A0909A21F57;
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