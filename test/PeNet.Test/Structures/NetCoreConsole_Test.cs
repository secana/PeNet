using System;
using Xunit;

namespace PeNet.Test.Structures
{
    public class NetCoreConsole_Test
    {
        private readonly PeFile _peFile = new PeFile(@"./Binaries/NetCoreConsole.dll");

        [Fact]
        public void DataDirectory_COMDescripterSet()
        {
            var dataDirectory = _peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int) Constants.DataDirectoryIndex.COM_Descriptor];

            Assert.Equal(0x2008u, dataDirectory.VirtualAddress);
            Assert.Equal(0x48u, dataDirectory.Size);
        }

        [Fact]
        public void IsSignatureValid()
        {
            Assert.False(_peFile.IsSignatureValid);
        }

        [Fact]
        public void NetDirectory_ParseCorrectValues()
        {
            var netDirectory = _peFile.ImageComDescriptor;

            Assert.Equal(0x00000048u, netDirectory.cb);
            Assert.Equal(0x0002u, netDirectory.MajorRuntimeVersion);
            Assert.Equal(0x0005u, netDirectory.MinorRuntimeVersion);
            Assert.Equal(0x00003A38u, netDirectory.MetaData.VirtualAddress);
            Assert.Equal(0x000056F8u, netDirectory.MetaData.Size);
            Assert.Equal(0x00000001u, netDirectory.Flags);
            Assert.Equal(0x0600000Cu, netDirectory.EntryPointToken);
            Assert.Equal(0x0600000Cu, netDirectory.EntryPointRVA);
            Assert.Equal(0x00000000u, netDirectory.Resources.VirtualAddress);
            Assert.Equal(0x00000000u, netDirectory.Resources.Size);
            Assert.Equal(0x00000000u, netDirectory.StrongNameSignature.VirtualAddress);
            Assert.Equal(0x00000000u, netDirectory.StrongNameSignature.Size);
            Assert.Equal(0x00000000u, netDirectory.CodeManagerTable.VirtualAddress);
            Assert.Equal(0x00000000u, netDirectory.CodeManagerTable.Size);
            Assert.Equal(0x00000000u, netDirectory.VTableFixups.VirtualAddress);
            Assert.Equal(0x00000000u, netDirectory.VTableFixups.Size);
            Assert.Equal(0x00000000u, netDirectory.ExportAddressTableJumps.VirtualAddress);
            Assert.Equal(0x00000000u, netDirectory.ExportAddressTableJumps.Size);
            Assert.Equal(0x00000000u, netDirectory.ManagedNativeHeader.VirtualAddress);
            Assert.Equal(0x00000000u, netDirectory.ManagedNativeHeader.Size);
        }

        [Fact]
        public void MetaDataHeader_ParseCorrectValues()
        {
            var metaDataHeader = _peFile.MetaDataHdr;

            Assert.Equal(0x424A5342u, metaDataHeader.Signature);
            Assert.Equal(0x001u, metaDataHeader.MajorVersion);
            Assert.Equal(0x001u, metaDataHeader.MinorVersion);
            Assert.Equal(0x00000000u, metaDataHeader.Reserved);
            Assert.Equal(0x0000000Cu, metaDataHeader.VersionLength);
            Assert.Equal("v4.0.30319", metaDataHeader.Version);
            Assert.Equal(0x0000u, metaDataHeader.Flags);
            Assert.Equal(0x0005u, metaDataHeader.Streams);
        }

        [Fact]
        public void MetaDataStreamTablesHeader_ParseCorrectValues()
        {
            var tablesHeader = _peFile.MetaDataStreamTablesHeader;

            Assert.Equal(0x00000000u, tablesHeader.Reserved1);
            Assert.Equal(0x02u, tablesHeader.MajorVersion);
            Assert.Equal(0x00u, tablesHeader.MinorVersion);
            Assert.Equal(0x01u, tablesHeader.Reserved2);
            Assert.Equal(0x00001E0909A21F57u, tablesHeader.Valid);
            Assert.Equal(0x000016003301FA00u, tablesHeader.MaskSorted);
        }

        [Fact]
        public void MetaDataStreamStrings_ParseCorrectValues()
        {
            var strings = _peFile.MetaDataStreamString.Strings;

            Assert.Equal(465, strings.Count);
            Assert.Equal("<>9__0_0", strings[0]);
            // ... More strings ...
            Assert.Equal("get_GdcLoudCheck", strings[228]);
            // ... More strings ...
            Assert.Equal("IsNullOrEmpty", strings[464]);
        }

        [Fact]
        public void MetaDataStreamUS_ParseCorrectValues()
        {
            var us = _peFile.MetaDataStreamUS;

            Assert.Equal(51, us.UserStrings.Count);
            Assert.Equal("CandCMetaInformationService", us.UserStrings[0]);
            Assert.Equal("Metainformation Proto3 Request for {0}.", us.UserStrings[44]);
            Assert.Equal("ServiceEndpoints:Redis:CacheExpire:Minutes", us.UserStrings[50]);
        }

        [Fact]
        public void MetaDataStreamGUID_ParseCorrectValues()
        {
            var guid = _peFile.MetaDataStreamGUID;
            Assert.Single(guid.GuidsAndIndices);
            Assert.Equal(new Guid("42f102e3-93ed-453b-bb95-d932c33520d4"), guid.Guids[0]);
        }

        [Fact]
        public void MetaDataStreamBlob_ParseCorrectValues()
        {
            var blob = _peFile.MetaDataStreamBlob;

            // Just test a few values instead of the whole blob
            Assert.Equal(0xC0C, blob.Length);
            Assert.Equal(0x00, blob[0]);
            Assert.Equal(0x81, blob[0x400]);
            Assert.Equal(0x70, blob[0xB70]);
        }

        [Fact]
        public void MetaDataTablesHdr_ParseCorrectTables()
        {
            var metaDataTablesHdr = _peFile.MetaDataStreamTablesHeader;

            Assert.Equal("Module", metaDataTablesHdr.TableDefinitions[0].Name);
            Assert.Equal(0x1U, metaDataTablesHdr.TableDefinitions[0].RowCount);
            Assert.Equal(0x00U, metaDataTablesHdr.TableDefinitions[0].Offset);
            Assert.Equal(0x0AU, metaDataTablesHdr.TableDefinitions[0].BytesPerRow);

            // ... More tables ...
          
            Assert.Equal("AssemblyRef", metaDataTablesHdr.TableDefinitions[35].Name);
            Assert.Equal(30U, metaDataTablesHdr.TableDefinitions[35].RowCount);
            Assert.Equal(0x1B50U, metaDataTablesHdr.TableDefinitions[35].Offset);
            Assert.Equal(20U, metaDataTablesHdr.TableDefinitions[35].BytesPerRow);

            // ... More tables ...
        }

        [Fact]
        public void MetaDataTable_Module()
        {
            var module = _peFile.MetaDataStreamTablesHeader.Tables.Module;

            Assert.Single(module);
            Assert.Equal(0x0000, module[0].Generation);
            Assert.Equal(0x0F92u, module[0].Name);
            Assert.Equal(0x0001u, module[0].Mvid);
            Assert.Equal(0x0000u, module[0].EncId);
            Assert.Equal(0x0000u, module[0].EncBaseId);
        }

        
        [Fact]
        public void MetaDataTable_TypeRef()
        {
            var typeRef = _peFile.MetaDataStreamTablesHeader.Tables.TypeRef;

            Assert.Equal(141, typeRef.Count);

            Assert.Equal(0x0006u, typeRef[0].ResolutionScope);
            Assert.Equal(0x0CA3u, typeRef[0].TypeName);
            Assert.Equal(0x182Fu, typeRef[0].TypeNamespace);

            // ... More rows ...

            Assert.Equal(0x000Eu, typeRef[22].ResolutionScope);
            Assert.Equal(0x17D1u, typeRef[22].TypeName);
            Assert.Equal(0x0E57u, typeRef[22].TypeNamespace);

            // ... More rows ...

            Assert.Equal(0x0036u, typeRef[140].ResolutionScope);
            Assert.Equal(0x0172u, typeRef[140].TypeName);
            Assert.Equal(0x01E3u, typeRef[140].TypeNamespace);
        }

        [Fact]
        public void MetaDataTable_TypeDef()
        {
            var typeDef = _peFile.MetaDataStreamTablesHeader.Tables.TypeDef;

            Assert.Equal(45, typeDef.Count);

            Assert.Equal(0x00000000u, typeDef[0].Flags);
            Assert.Equal(0x0229u, typeDef[0].Name);
            Assert.Equal(0x0000u, typeDef[0].Namespace);
            Assert.Equal(0x0000u, typeDef[0].Extends);
            Assert.Equal(0x0001u, typeDef[0].FieldList);
            Assert.Equal(0x0001u, typeDef[0].MethodList);

            // ... More rows ...

            Assert.Equal(0x00100182u, typeDef[44].Flags);
            Assert.Equal(0x0AA6u, typeDef[44].Name);
            Assert.Equal(0x0000u, typeDef[44].Namespace);
            Assert.Equal(0x0035u, typeDef[44].Extends);
            Assert.Equal(0x005Au, typeDef[44].FieldList);
            Assert.Equal(0x006Eu, typeDef[44].MethodList);
        }

        [Fact]
        public void MetaDataTable_Field()
        {
            var field = _peFile.MetaDataStreamTablesHeader.Tables.Field;

            Assert.Equal(92, field.Count);

            Assert.Equal(0x8056u, field[0].Flags);
            Assert.Equal(0x049Du, field[0].Name);
            Assert.Equal(0x088Cu, field[0].Signature);

            // ... More rows ...

            Assert.Equal(0x0031u, field[91].Flags);
            Assert.Equal(0x0727u, field[91].Name);
            Assert.Equal(0x088Cu, field[91].Signature);
        }

        [Fact]
        public void MetaDataTable_MethodDef()
        {
            var methodDef = _peFile.MetaDataStreamTablesHeader.Tables.MethodDef;

            Assert.Equal(113, methodDef.Count);

            Assert.Equal(0x0000u, methodDef[0].RVA);
            Assert.Equal(0x0000u, methodDef[0].ImplFlags);
            Assert.Equal(0x05C6u, methodDef[0].Flags);
            Assert.Equal(0x0F24u, methodDef[0].Name);
            Assert.Equal(0x09D3u, methodDef[0].Signature);
            Assert.Equal(0x0001u, methodDef[0].ParamList);

            // ... More rows ...

            Assert.Equal(0x39E0u, methodDef[112].RVA);
            Assert.Equal(0x0000u, methodDef[112].ImplFlags);
            Assert.Equal(0x1891u, methodDef[112].Flags);
            Assert.Equal(0x1806u, methodDef[112].Name);
            Assert.Equal(0x0A15u, methodDef[112].Signature);
            Assert.Equal(0x007Eu, methodDef[112].ParamList);
        }

        [Fact]
        public void MetaDataTable_Param()
        {
            var param = _peFile.MetaDataStreamTablesHeader.Tables.Param;

            Assert.Equal(125, param.Count);

            Assert.Equal(0x0000u, param[0].Flags);
            Assert.Equal(0x0001u, param[0].Sequence);
            Assert.Equal(0x1085u, param[0].Name);

            // ... More rows ...

            Assert.Equal(0x0000u, param[124].Flags);
            Assert.Equal(0x0001u, param[124].Sequence);
            Assert.Equal(0x0D4Cu, param[124].Name);
        }

        [Fact]
        public void MetaDataTable_InterfaceImpl()
        {
            var interfaceImpl = _peFile.MetaDataStreamTablesHeader.Tables.InterfaceImpl;

            Assert.Equal(3, interfaceImpl.Count);

            Assert.Equal(0x0005u, interfaceImpl[0].Class);
            Assert.Equal(0x004Du, interfaceImpl[0].Interface);

            Assert.Equal(0x0006u, interfaceImpl[1].Class);
            Assert.Equal(0x000Cu, interfaceImpl[1].Interface);

            Assert.Equal(0x0019u, interfaceImpl[2].Class);
            Assert.Equal(0x0068u, interfaceImpl[2].Interface);
        }

        [Fact]
        public void MetaDataTable_MemberRef()
        {
            var memberRef = _peFile.MetaDataStreamTablesHeader.Tables.MemberRef;

            Assert.Equal(216, memberRef.Count);

            Assert.Equal(0x0009u, memberRef[0].Class);
            Assert.Equal(0x1800u, memberRef[0].Name);
            Assert.Equal(0x0001u, memberRef[0].Signature);

            // ... More rows ...

            Assert.Equal(0x0409u, memberRef[215].Class);
            Assert.Equal(0x01A8u, memberRef[215].Name);
            Assert.Equal(0x07D7u, memberRef[215].Signature);
        }

        [Fact]
        public void MetaDataTable_Constant()
        {
            var constant = _peFile.MetaDataStreamTablesHeader.Tables.Constant;

            Assert.Equal(38, constant.Count);

            Assert.Equal(0x08, constant[0].Type);
            Assert.Equal(0x0004u, constant[0].Parent);
            Assert.Equal(0x0800u, constant[0].Value);
            
            // ... More rows ...

            Assert.Equal(0x12, constant[37].Type);
            Assert.Equal(0x0195u, constant[37].Parent);
            Assert.Equal(0x0887u, constant[37].Value);
        }

        [Fact]
        public void MetaDataTable_CustomAttribute()
        {
            var customAttribute = _peFile.MetaDataStreamTablesHeader.Tables.CustomAttribute;

            Assert.Equal(97, customAttribute.Count);

            Assert.Equal(0x002Eu, customAttribute[0].Parent);
            Assert.Equal(0x000Bu, customAttribute[0].Type);
            Assert.Equal(0x0B31u, customAttribute[0].Value);

            // ... More rows ...

            Assert.Equal(0x0F44u, customAttribute[96].Parent);
            Assert.Equal(0x0073u, customAttribute[96].Type);
            Assert.Equal(0x0BFDu, customAttribute[96].Value);
        }

        [Fact]
        public void MetaDataTable_FieldMarshall()
        {
            var fieldMarshall = _peFile.MetaDataStreamTablesHeader.Tables.FieldMarshal;

            Assert.Null(fieldMarshall);
        }

        [Fact]
        public void MetaDataTable_DeclSecurity()
        {
            var declSecurity = _peFile.MetaDataStreamTablesHeader.Tables.DeclSecurity;

            Assert.Null(declSecurity);
        }

        [Fact]
        public void MetaDataTable_ClassLayout()
        {
            var classLayout = _peFile.MetaDataStreamTablesHeader.Tables.ClassLayout;

            Assert.Null(classLayout);
        }

        [Fact]
        public void MetaDataTable_FieldLayout()
        {
            var fieldLayout = _peFile.MetaDataStreamTablesHeader.Tables.FieldLayout;

            Assert.Null(fieldLayout);
        }

        [Fact]
        public void MetaDataTable_StandAloneSig()
        {
            var standAloneSig = _peFile.MetaDataStreamTablesHeader.Tables.StandAloneSig;

            Assert.Equal(30, standAloneSig.Count);

            Assert.Equal(0x0027u, standAloneSig[0].Signature);

            // ... More rows ...

            Assert.Equal(0x07C4u, standAloneSig[29].Signature);
        }

        [Fact]
        public void MetaDataTable_EventMap()
        {
            var eventMap = _peFile.MetaDataStreamTablesHeader.Tables.EventMap;

            Assert.Null(eventMap);
        }

        [Fact]
        public void MetaDataTable_Event()
        {
            var ev = _peFile.MetaDataStreamTablesHeader.Tables.Event;

            Assert.Null(ev);
        }

        [Fact]
        public void MetaDataTable_PropertyMap()
        {
            var propertyMap = _peFile.MetaDataStreamTablesHeader.Tables.PropertyMap;

            Assert.Equal(6, propertyMap.Count);

            Assert.Equal(0x0005u, propertyMap[0].Parent);
            Assert.Equal(0x0001u, propertyMap[0].PropertyList);

            // ... More rows ...

            Assert.Equal(0x002Du, propertyMap[5].Parent);
            Assert.Equal(0x0017u, propertyMap[5].PropertyList);

        }

        [Fact]
        public void MetaDataTable_Property()
        {
            var property = _peFile.MetaDataStreamTablesHeader.Tables.Property;

            Assert.Equal(25, property.Count);

            Assert.Equal(0x0000u, property[0].Flags);
            Assert.Equal(0x0521u, property[0].Name);
            Assert.Equal(0x0B1Bu, property[0].Type);

            // ... More rows ...

            Assert.Equal(0x0000u, property[24].Flags);
            Assert.Equal(0x18BBu, property[24].Name);
            Assert.Equal(0x0B2Du, property[24].Type);
        }

        [Fact]
        public void MetaDataTable_MethodSemantics()
        {
            var methodSemantics = _peFile.MetaDataStreamTablesHeader.Tables.MethodSemantic;

            Assert.Equal(27, methodSemantics.Count);

            Assert.Equal(0x0002u, methodSemantics[0].Semantics);
            Assert.Equal(0x0002u, methodSemantics[0].Method);
            Assert.Equal(0x0003u, methodSemantics[0].Association);

            // ... More rows ...

            Assert.Equal(0x0002u, methodSemantics[26].Semantics);
            Assert.Equal(0x0070u, methodSemantics[26].Method);
            Assert.Equal(0x00033u, methodSemantics[26].Association);
        }

        [Fact]
        public void MetaDataTable_MethodImpl()
        {
            var methodImpl = _peFile.MetaDataStreamTablesHeader.Tables.MethodImpl;

            Assert.Null(methodImpl);
        }

        [Fact]
        public void MetaDataTable_ModuleRef()
        {
            var moduleRef = _peFile.MetaDataStreamTablesHeader.Tables.ModuleRef;

            Assert.Null(moduleRef);
        }

        [Fact]
        public void MetaDataTable_TypeSpec()
        {
            var typeSpec = _peFile.MetaDataStreamTablesHeader.Tables.TypeSpec;

            Assert.Equal(43, typeSpec.Count);

            Assert.Equal(0x001Bu, typeSpec[0].Signature);

            // ... More rows ...

            Assert.Equal(0x07B5u, typeSpec[42].Signature);
        }

        [Fact]
        public void MetaDataTable_FieldRVA()
        {
            var fieldRVA = _peFile.MetaDataStreamTablesHeader.Tables.FieldRVA;

            Assert.Null(fieldRVA);
        }

        [Fact]
        public void MetaDataTable_Assembly()
        {
            var assembly = _peFile.MetaDataStreamTablesHeader.Tables.Assembly;

            Assert.Single(assembly);

            Assert.Equal(0x00008004u, assembly[0].HashAlgId);
            Assert.Equal(0x0001u, assembly[0].MajorVersion);
            Assert.Equal(0x0009u, assembly[0].MinorVersion);
            Assert.Equal(0x0007u, assembly[0].BuildNumber);
            Assert.Equal(0x0000u, assembly[0].RevisionNumber);
            Assert.Equal(0x00000000u, assembly[0].Flags);
            Assert.Equal(0x0000u, assembly[0].PublicKey);
            Assert.Equal(0x08CFu, assembly[0].Name);
            Assert.Equal(0x0000u, assembly[0].Culture);
        }

        [Fact]
        public void MetaDataTable_AssemblyProcessor()
        {
            var assemblyProcessor = _peFile.MetaDataStreamTablesHeader.Tables.AssemblyProcessor;

            Assert.Null(assemblyProcessor);
        }

        [Fact]
        public void MetaDataTable_AssemblyOS()
        {
            var assemblyOS = _peFile.MetaDataStreamTablesHeader.Tables.AssemblyOS;

            Assert.Null(assemblyOS);
        }

        [Fact]
        public void MetaDataTable_AssemblyRef()
        {
            var assemblyRef = _peFile.MetaDataStreamTablesHeader.Tables.AssemblyRef;

            Assert.Equal(30, assemblyRef.Count);

            Assert.Equal(0x0004u, assemblyRef[0].MajorVersion);
            Assert.Equal(0x0002u, assemblyRef[0].MinorVersion);
            Assert.Equal(0x0000u, assemblyRef[0].BuildNumber);
            Assert.Equal(0x0000u, assemblyRef[0].RevisionNumber);
            Assert.Equal(0x00000000u, assemblyRef[0].Flags);
            Assert.Equal(0x07DCu, assemblyRef[0].PublicKeyOrToken);
            Assert.Equal(0x0A33u, assemblyRef[0].Name);
            Assert.Equal(0x0000u, assemblyRef[0].Culture);
            Assert.Equal(0x0000u, assemblyRef[0].HashValue);

            // ... More rows ...

            Assert.Equal(0x0001u, assemblyRef[29].MajorVersion);
            Assert.Equal(0x0001u, assemblyRef[29].MinorVersion);
            Assert.Equal(0x0002u, assemblyRef[29].BuildNumber);
            Assert.Equal(0x0000u, assemblyRef[29].RevisionNumber);
            Assert.Equal(0x00000000u, assemblyRef[29].Flags);
            Assert.Equal(0x07E5u, assemblyRef[29].PublicKeyOrToken);
            Assert.Equal(0x1A98u, assemblyRef[29].Name);
            Assert.Equal(0x0000u, assemblyRef[29].Culture);
            Assert.Equal(0x0000u, assemblyRef[29].HashValue);
        }

        [Fact]
        public void MetaDataTable_AssemblyRefProcessor()
        {
            var assemblyRefProcessor = _peFile.MetaDataStreamTablesHeader.Tables.AssemblyRefProcessor;

            Assert.Null(assemblyRefProcessor);
        }

        [Fact]
        public void MetaDataTable_AssemblyRefOS()
        {
            var assemblyRefOS = _peFile.MetaDataStreamTablesHeader.Tables.AssemblyRefOS;

            Assert.Null(assemblyRefOS);
        }

        [Fact]
        public void MetaDataTable_File()
        {
            var file = _peFile.MetaDataStreamTablesHeader.Tables.File;

            Assert.Null(file);
        }

        [Fact]
        public void MetaDataTable_ExportedType()
        {
            var exportedType = _peFile.MetaDataStreamTablesHeader.Tables.ExportedType;

            Assert.Null(exportedType);
        }

        [Fact]
        public void MetaDataTable_ManifestResource()
        {
            var manifestResource = _peFile.MetaDataStreamTablesHeader.Tables.ManifestResource;

            Assert.Null(manifestResource);
        }

        [Fact]
        public void MetaDataTable_NestedClass()
        {
            var nestedClass = _peFile.MetaDataStreamTablesHeader.Tables.NestedClass;

            Assert.Equal(19, nestedClass.Count);

            Assert.Equal(0x001Bu, nestedClass[0].NestedClassType);
            Assert.Equal(0x0002u, nestedClass[0].EnclosingClassType);

            // ... More rows ...

            Assert.Equal(0x002Du, nestedClass[18].NestedClassType);
            Assert.Equal(0x002Cu, nestedClass[18].EnclosingClassType);
        }

        [Fact]
        public void MetaDataTable_GenericParam()
        {
            var genericParam = _peFile.MetaDataStreamTablesHeader.Tables.GenericParam;

            Assert.Single(genericParam);

            Assert.Equal(0x0000u, genericParam[0].Number);
            Assert.Equal(0x0010u, genericParam[0].Flags);
            Assert.Equal(0x0047u, genericParam[0].Owner);
            Assert.Equal(0x0835u, genericParam[0].Name);
        }

        [Fact]
        public void MetaDataTable_GenericParamConstraint()
        {
            var genericParamConstraint = _peFile.MetaDataStreamTablesHeader.Tables.GenericParamConstraints;

            Assert.Single(genericParamConstraint);
            Assert.Equal(0x0001u, genericParamConstraint[0].Owner);
            Assert.Equal(0x0000u, genericParamConstraint[0].Constraint);
        }
    }
}