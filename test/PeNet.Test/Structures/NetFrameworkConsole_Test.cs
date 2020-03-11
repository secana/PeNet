using System;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    public class NetFrameworkConsole_Test
    {
        private readonly PeFile _peFile = new PeFile(@"./Binaries/NetFrameworkConsole.exe");

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

            Assert.Equal(0x00000048u, netDirectory.Cb);
            Assert.Equal(0x0002u, netDirectory.MajorRuntimeVersion);
            Assert.Equal(0x0005u, netDirectory.MinorRuntimeVersion);
            Assert.Equal(0x000020ACu, netDirectory.MetaData.VirtualAddress);
            Assert.Equal(0x00000728u, netDirectory.MetaData.Size);
            Assert.Equal(ComFlagsType.IlOnly, netDirectory.Flags & ComFlagsType.IlOnly);
            Assert.Equal(ComFlagsType.BitRequired32, netDirectory.Flags & ComFlagsType.BitRequired32);
            Assert.Equal(0x06000001u, netDirectory.EntryPointToken);
            Assert.Equal(0x06000001u, netDirectory.EntryPointRva);
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
        public void MetaDataStreamTables_ParseCorrectValues()
        {
            var tablesHeader = _peFile.MetaDataStreamTablesHeader;

            Assert.Equal(0x00000000u, tablesHeader.Reserved1);
            Assert.Equal(0x02u, tablesHeader.MajorVersion);
            Assert.Equal(0x00u, tablesHeader.MinorVersion);
            Assert.Equal(0x01u, tablesHeader.Reserved2);
            Assert.Equal(0x0000000908021547u, (ulong) tablesHeader.MaskValid);
            Assert.Equal(0x000016003301FA00u, tablesHeader.MaskSorted);
        }

      [Fact]
        public void MetaDataStreamUS_ParseCorrectValues()
        {
            var us = _peFile.MetaDataStreamUS;

            Assert.Single(us.UserStrings);
            Assert.Equal(@"C:\", us.UserStrings[0]);
        }

        [Fact]
        public void MetaDataStreamGUID_ParseCorrectValues()
        {
            var guid = _peFile.MetaDataStreamGUID;
            Assert.Single(guid.GuidsAndIndices);
            Assert.Equal(new Guid("5250e853-c17a-4e76-adb3-0a716ec8af5d"), guid.Guids[0]);
        }

        [Fact]
        public void MetaDataStreamBlob_ParseCorrectValues()
        {
            var blob = _peFile.MetaDataStreamBlob;

            // Just test a few values instead of the whole blob
            Assert.Equal(0x150, blob.Length);
            Assert.Equal(0x00, blob[0]);
            Assert.Equal(0x4E, blob[0x97]);
            Assert.Equal(0x00, blob[0x14F]);
        }

        [Fact]
        public void MetaDataTablesHdr_ParseCorrectTables()
        {
            var MetaDataTablesHdr = _peFile.MetaDataStreamTablesHeader;

            Assert.Equal("Module", MetaDataTablesHdr.TableDefinitions[0].Name);
            Assert.Equal(1U, MetaDataTablesHdr.TableDefinitions[0].RowCount);
            Assert.Equal(0U, MetaDataTablesHdr.TableDefinitions[0].Offset);
            Assert.Equal(10U, MetaDataTablesHdr.TableDefinitions[0].BytesPerRow);

            // ... More tables ...
          
            Assert.Equal("AssemblyRef", MetaDataTablesHdr.TableDefinitions[35].Name);
            Assert.Equal(1U, MetaDataTablesHdr.TableDefinitions[35].RowCount);
            Assert.Equal(454U, MetaDataTablesHdr.TableDefinitions[35].Offset);
            Assert.Equal(20U, MetaDataTablesHdr.TableDefinitions[35].BytesPerRow);
        }

        [Fact]
        public void MetaDataTable_Module()
        {
            var module = _peFile.MetaDataStreamTablesHeader.Tables.Module;

            Assert.Single(module);
            Assert.Equal(0x0000, module[0].Generation);
            Assert.Equal(0x01EBu, module[0].Name);
            Assert.Equal(0x0001u, module[0].Mvid);
            Assert.Equal(0x0000u, module[0].EncId);
            Assert.Equal(0x0000u, module[0].EncBaseId);
        }

        
        [Fact]
        public void MetaDataTable_TypeRef()
        {
            var typeRef = _peFile.MetaDataStreamTablesHeader.Tables.TypeRef;

            Assert.Equal(23, typeRef.Count);

            Assert.Equal(0x0006u, typeRef[0].ResolutionScope);
            Assert.Equal(0x0160u, typeRef[0].TypeName);
            Assert.Equal(0x02A4u, typeRef[0].TypeNamespace);

            // ... More rows ...

            Assert.Equal(0x0006u, typeRef[22].ResolutionScope);
            Assert.Equal(0x0243u, typeRef[22].TypeName);
            Assert.Equal(0x0225u, typeRef[22].TypeNamespace);
        }

        [Fact]
        public void MetaDataTable_TypeDef()
        {
            var typeDef = _peFile.MetaDataStreamTablesHeader.Tables.TypeDef;

            Assert.Equal(2, typeDef.Count);

            Assert.Equal(0x00000000u, typeDef[0].Flags);
            Assert.Equal(0x001Du, typeDef[0].Name);
            Assert.Equal(0x0000u, typeDef[0].Namespace);
            Assert.Equal(0x0000u, typeDef[0].Extends);
            Assert.Equal(0x0001u, typeDef[0].FieldList);
            Assert.Equal(0x0001u, typeDef[0].MethodList);

            Assert.Equal(0x00100000u, typeDef[1].Flags);
            Assert.Equal(0x021Du, typeDef[1].Name);
            Assert.Equal(0x0060u, typeDef[1].Namespace);
            Assert.Equal(0x0041u, typeDef[1].Extends);
            Assert.Equal(0x0001u, typeDef[1].FieldList);
            Assert.Equal(0x0001u, typeDef[1].MethodList);
        }

        [Fact]
        public void MetaDataTable_Field()
        {
            var field = _peFile.MetaDataStreamTablesHeader.Tables.Field;

            Assert.Empty(field);
        }

        [Fact]
        public void MetaDataTable_MethodDef()
        {
            var methodDef = _peFile.MetaDataStreamTablesHeader.Tables.MethodDef;

            Assert.Equal(2, methodDef.Count);

            Assert.Equal(0x00002050u, methodDef[0].RVA);
            Assert.Equal(0x0000u, methodDef[0].ImplFlags);
            Assert.Equal(0x0091u, methodDef[0].Flags);
            Assert.Equal(0x022Cu, methodDef[0].Name);
            Assert.Equal(0x005Cu, methodDef[0].Signature);
            Assert.Equal(0x0001u, methodDef[0].ParamList);

            Assert.Equal(0x000020A4u, methodDef[1].RVA);
            Assert.Equal(0x0000u, methodDef[1].ImplFlags);
            Assert.Equal(0x1886u, methodDef[1].Flags);
            Assert.Equal(0x026Cu, methodDef[1].Name);
            Assert.Equal(0x0006u, methodDef[1].Signature);
            Assert.Equal(0x0002u, methodDef[1].ParamList);
        }

        [Fact]
        public void MetaDataTable_Param()
        {
            var param = _peFile.MetaDataStreamTablesHeader.Tables.Param;

            Assert.Single(param);

            Assert.Equal(0x0000u, param[0].Flags);
            Assert.Equal(0x0001u, param[0].Sequence);
            Assert.Equal(0x02E8u, param[0].Name);
        }

        [Fact]
        public void MetaDataTable_InterfaceImpl()
        {
            var interfaceImpl = _peFile.MetaDataStreamTablesHeader.Tables.InterfaceImpl;

            Assert.Empty(interfaceImpl);
        }

        [Fact]
        public void MetaDataTable_MemberRef()
        {
            var memberRef = _peFile.MetaDataStreamTablesHeader.Tables.MemberRef;

            Assert.Equal(22, memberRef.Count);

            Assert.Equal(0x0009u, memberRef[0].Class);
            Assert.Equal(0x026Cu, memberRef[0].Name);
            Assert.Equal(0x0001u, memberRef[0].Signature);

            // ... More rows ...

            Assert.Equal(0x0081u, memberRef[21].Class);
            Assert.Equal(0x026Cu, memberRef[21].Name);
            Assert.Equal(0x0006u, memberRef[21].Signature);
        }

        [Fact]
        public void MetaDataTable_Constant()
        {
            var constant = _peFile.MetaDataStreamTablesHeader.Tables.Constant;

            Assert.Empty(constant);
        }

        [Fact]
        public void MetaDataTable_CustomAttribute()
        {
            var customAttribute = _peFile.MetaDataStreamTablesHeader.Tables.CustomAttribute;

            Assert.Equal(14, customAttribute.Count);

            Assert.Equal(0x002Eu, customAttribute[0].Parent);
            Assert.Equal(0x000Bu, customAttribute[0].Type);
            Assert.Equal(0x0062u, customAttribute[0].Value);

            // ... More rows ...

            Assert.Equal(0x002Eu, customAttribute[13].Parent);
            Assert.Equal(0x0073u, customAttribute[13].Type);
            Assert.Equal(0x00101u, customAttribute[13].Value);
        }

        [Fact]
        public void MetaDataTable_FieldMarshall()
        {
            var fieldMarshall = _peFile.MetaDataStreamTablesHeader.Tables.FieldMarshal;

            Assert.Empty(fieldMarshall);
        }

        [Fact]
        public void MetaDataTable_DeclSecurity()
        {
            var declSecurity = _peFile.MetaDataStreamTablesHeader.Tables.DeclSecurity;

            Assert.Empty(declSecurity);
        }

        [Fact]
        public void MetaDataTable_ClassLayout()
        {
            var classLayout = _peFile.MetaDataStreamTablesHeader.Tables.ClassLayout;

            Assert.Empty(classLayout);
        }

        [Fact]
        public void MetaDataTable_FieldLayout()
        {
            var fieldLayout = _peFile.MetaDataStreamTablesHeader.Tables.FieldLayout;

            Assert.Empty(fieldLayout);
        }

        [Fact]
        public void MetaDataTable_StandAloneSig()
        {
            var standAloneSig = _peFile.MetaDataStreamTablesHeader.Tables.StandAloneSig;

            Assert.Single(standAloneSig);

            Assert.Equal(0x001Au, standAloneSig[0].Signature);
        }

        [Fact]
        public void MetaDataTable_EventMap()
        {
            var eventMap = _peFile.MetaDataStreamTablesHeader.Tables.EventMap;

            Assert.Empty(eventMap);
        }

        [Fact]
        public void MetaDataTable_Event()
        {
            var ev = _peFile.MetaDataStreamTablesHeader.Tables.Event;

            Assert.Empty(ev);
        }

        [Fact]
        public void MetaDataTable_PropertyMap()
        {
            var popertyMap = _peFile.MetaDataStreamTablesHeader.Tables.PropertyMap;

            Assert.Empty(popertyMap);
        }

        [Fact]
        public void MetaDataTable_Property()
        {
            var poperty = _peFile.MetaDataStreamTablesHeader.Tables.Property;

            Assert.Empty(poperty);
        }

        [Fact]
        public void MetaDataTable_MethodSemantics()
        {
            var methodSemantics = _peFile.MetaDataStreamTablesHeader.Tables.MethodSemantic;

            Assert.Empty(methodSemantics);
        }

        [Fact]
        public void MetaDataTable_MethodImpl()
        {
            var methodImpl = _peFile.MetaDataStreamTablesHeader.Tables.MethodImpl;

            Assert.Empty(methodImpl);
        }

        [Fact]
        public void MetaDataTable_ModuleRef()
        {
            var moduleRef = _peFile.MetaDataStreamTablesHeader.Tables.ModuleRef;

            Assert.Empty(moduleRef);
        }

        [Fact]
        public void MetaDataTable_TypeSpec()
        {
            var typeSpec = _peFile.MetaDataStreamTablesHeader.Tables.TypeSpec;

            Assert.Equal(2, typeSpec.Count);

            Assert.Equal(0x002Bu, typeSpec[0].Signature);
            Assert.Equal(0x003Au, typeSpec[1].Signature);
        }

        [Fact]
        public void MetaDataTable_FieldRVA()
        {
            var fieldRVA = _peFile.MetaDataStreamTablesHeader.Tables.FieldRVA;

            Assert.Empty(fieldRVA);
        }

        [Fact]
        public void MetaDataTable_Assembly()
        {
            var assembly = _peFile.MetaDataStreamTablesHeader.Tables.Assembly;

            Assert.Single(assembly);

            Assert.Equal(0x00008004u, assembly[0].HashAlgId);
            Assert.Equal(0x0001u, assembly[0].MajorVersion);
            Assert.Equal(0x0000u, assembly[0].MinorVersion);
            Assert.Equal(0x0000u, assembly[0].BuildNumber);
            Assert.Equal(0x0000u, assembly[0].RevisionNumber);
            Assert.Equal(0x00000000u, assembly[0].Flags);
            Assert.Equal(0x0000u, assembly[0].PublicKey);
            Assert.Equal(0x0060u, assembly[0].Name);
            Assert.Equal(0x0000u, assembly[0].Culture);
        }

        [Fact]
        public void MetaDataTable_AssemblyProcessor()
        {
            var assemblyProcessor = _peFile.MetaDataStreamTablesHeader.Tables.AssemblyProcessor;

            Assert.Empty(assemblyProcessor);
        }

        [Fact]
        public void MetaDataTable_AssemblyOS()
        {
            var assemblyOS = _peFile.MetaDataStreamTablesHeader.Tables.AssemblyOS;

            Assert.Empty(assemblyOS);
        }

        [Fact]
        public void MetaDataTable_AssemblyRef()
        {
            var assemblyRef = _peFile.MetaDataStreamTablesHeader.Tables.AssemblyRef;

            Assert.Single(assemblyRef);

            Assert.Equal(0x0004u, assemblyRef[0].MajorVersion);
            Assert.Equal(0x0000u, assemblyRef[0].MinorVersion);
            Assert.Equal(0x0000u, assemblyRef[0].BuildNumber);
            Assert.Equal(0x0000u, assemblyRef[0].RevisionNumber);
            Assert.Equal(0x00000000u, assemblyRef[0].Flags);
            Assert.Equal(0x0053u, assemblyRef[0].PublicKeyOrToken);
            Assert.Equal(0x0030u, assemblyRef[0].Name);
            Assert.Equal(0x0000u, assemblyRef[0].Culture);
            Assert.Equal(0x0000u, assemblyRef[0].HashValue);
        }

        [Fact]
        public void MetaDataTable_AssemblyRefProcessor()
        {
            var assemblyRefProcessor = _peFile.MetaDataStreamTablesHeader.Tables.AssemblyRefProcessor;

            Assert.Empty(assemblyRefProcessor);
        }

        [Fact]
        public void MetaDataTable_AssemblyRefOS()
        {
            var assemblyRefOS = _peFile.MetaDataStreamTablesHeader.Tables.AssemblyRefOS;

            Assert.Empty(assemblyRefOS);
        }

        [Fact]
        public void MetaDataTable_File()
        {
            var file = _peFile.MetaDataStreamTablesHeader.Tables.File;

            Assert.Empty(file);
        }

        [Fact]
        public void MetaDataTable_ExportedType()
        {
            var exportedType = _peFile.MetaDataStreamTablesHeader.Tables.ExportedType;

            Assert.Empty(exportedType);
        }

        [Fact]
        public void MetaDataTable_ManifestResource()
        {
            var manifestResource = _peFile.MetaDataStreamTablesHeader.Tables.ManifestResource;

            Assert.Empty(manifestResource);
        }

        [Fact]
        public void MetaDataTable_NestedClass()
        {
            var nestedClass = _peFile.MetaDataStreamTablesHeader.Tables.NestedClass;

            Assert.Empty(nestedClass);
        }

        [Fact]
        public void MetaDataTable_GenericParam()
        {
            var genericParam = _peFile.MetaDataStreamTablesHeader.Tables.GenericParam;

            Assert.Empty(genericParam);
        }

        [Fact]
        public void MetaDataTable_GenericParamConstraint()
        {
            var genericParamConstraint = _peFile.MetaDataStreamTablesHeader.Tables.GenericParamConstraints;

            Assert.Empty(genericParamConstraint);
        }

        [Fact]
        public void NetGuid_ModuleVersionId()
        {
            var mvids = _peFile.ClrModuleVersionIds;

            Assert.Single(mvids);
            Assert.Equal(new Guid("5250e853-c17a-4e76-adb3-0a716ec8af5d"), mvids[0]);
        }

        [Fact]
        public void NetGuid_ComTypeLibId()
        {
            Assert.Equal("a782d109-aa8f-427b-8dcf-1c786054c7e0", _peFile.ClrComTypeLibId);
        }
    }
}