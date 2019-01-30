using System;
using Xunit;

namespace PeNet.Test.Structures
{
    public class NetFrameworkConsole_Test
    {
        private readonly PeFile _peFile = new PeFile(@"../../../Binaries/NetFrameworkConsole.exe");

        [Fact]
        public void NetFameworkConsole_DataDirectory_COMDescripterSet()
        {
            var dataDirectory = _peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int) Constants.DataDirectoryIndex.COM_Descriptor];

            Assert.Equal(0x2008u, dataDirectory.VirtualAddress);
            Assert.Equal(0x48u, dataDirectory.Size);
        }

        [Fact]
        public void NetFameworkConsole_IsSignatureValid()
        {
            Assert.False(_peFile.IsSignatureValid);
        }

        [Fact]
        public void NetFameworkConsole_NetDirectory_ParseCorrectValues()
        {
            var netDirectory = _peFile.ImageComDescriptor;

            Assert.Equal(0x00000048u, netDirectory.cb);
            Assert.Equal(0x0002u, netDirectory.MajorRuntimeVersion);
            Assert.Equal(0x0005u, netDirectory.MinorRuntimeVersion);
            Assert.Equal(0x000020ACu, netDirectory.MetaData.VirtualAddress);
            Assert.Equal(0x00000728u, netDirectory.MetaData.Size);
            Assert.Equal(0x00020003u, netDirectory.Flags);
            Assert.Equal(0x06000001u, netDirectory.EntryPointToken);
            Assert.Equal(0x06000001u, netDirectory.EntryPointRVA);
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
        public void NetFrameworkConsole_MetaDataHeader_ParseCorrectValues()
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
        public void NetFameworkConsole_MetaDataStreamTables_ParseCorrectValues()
        {
            var tablesHeader = _peFile.MetaDataStreamTablesHeader;

            Assert.Equal(0x00000000u, tablesHeader.Reserved1);
            Assert.Equal(0x02u, tablesHeader.MajorVersion);
            Assert.Equal(0x00u, tablesHeader.MinorVersion);
            Assert.Equal(0x01u, tablesHeader.Reserved2);
            Assert.Equal(0x0000000908021547u, tablesHeader.Valid);
            Assert.Equal(0x000016003301FA00u, tablesHeader.MaskSorted);
        }

        [Fact]
        public void NetFrameworkConsole_MetaDataStreamStrings_ParseCorrectValues()
        {
            var strings = _peFile.MetaDataStreamString.Strings;

            Assert.Equal(46, strings.Count);
            Assert.Equal("IEnumerable`1", strings[0]);
            Assert.Equal("IEnumerator`1", strings[1]);
            Assert.Equal("<Module>", strings[2]);
            Assert.Equal("System.IO", strings[3]);
            Assert.Equal("mscorlib", strings[4]);
            Assert.Equal("System.Collections.Generic", strings[5]);
            Assert.Equal("IDisposable", strings[6]);
            Assert.Equal("NetFrameworkConsole", strings[7]);
            Assert.Equal("WriteLine", strings[8]);
            Assert.Equal("Dispose", strings[9]);
            Assert.Equal("GuidAttribute", strings[10]);
            Assert.Equal("DebuggableAttribute", strings[11]);
            Assert.Equal("ComVisibleAttribute", strings[12]);
            Assert.Equal("AssemblyTitleAttribute", strings[13]);
            Assert.Equal("AssemblyTrademarkAttribute", strings[14]);
            Assert.Equal("TargetFrameworkAttribute", strings[15]);
            Assert.Equal("AssemblyFileVersionAttribute", strings[16]);
            Assert.Equal("AssemblyConfigurationAttribute", strings[17]);
            Assert.Equal("AssemblyDescriptionAttribute", strings[18]);
            Assert.Equal("CompilationRelaxationsAttribute", strings[19]);
            Assert.Equal("AssemblyProductAttribute", strings[20]);
            Assert.Equal("AssemblyCopyrightAttribute", strings[21]);
            Assert.Equal("AssemblyCompanyAttribute", strings[22]);
            Assert.Equal("RuntimeCompatibilityAttribute", strings[23]);
            Assert.Equal("NetFrameworkConsole.exe", strings[24]);
            Assert.Equal("System.Runtime.Versioning", strings[25]);
            Assert.Equal("Program", strings[26]);
            Assert.Equal("System", strings[27]);
            Assert.Equal("Main", strings[28]);
            Assert.Equal("System.Reflection", strings[29]);
            Assert.Equal("ConsoleKeyInfo", strings[30]);
            Assert.Equal("IEnumerator", strings[31]);
            Assert.Equal("GetEnumerator", strings[32]);
            Assert.Equal(".ctor", strings[33]);
            Assert.Equal("System.Diagnostics", strings[34]);
            Assert.Equal("System.Runtime.InteropServices", strings[35]);
            Assert.Equal("System.Runtime.CompilerServices", strings[36]);
            Assert.Equal("DebuggingModes", strings[37]);
            Assert.Equal("EnumerateDirectories", strings[38]);
            Assert.Equal("args", strings[39]);
            Assert.Equal("System.Collections", strings[40]);
            Assert.Equal("Object", strings[41]);
            Assert.Equal("get_Current", strings[42]);
            Assert.Equal("MoveNext", strings[43]);
            Assert.Equal("ReadKey", strings[44]);
            Assert.Equal("Directory", strings[45]);
        }

        [Fact]
        public void NetFrameworkConsole_MetaDataStreamUS_ParseCorrectValues()
        {
            var us = _peFile.MetaDataStreamUS;

            Assert.Single(us.UserStrings);
            Assert.Equal(@"C:\", us.UserStrings[0]);
        }

        [Fact]
        public void NetFrameworkConsole_MetaDataStreamGUID_ParseCorrectValues()
        {
            var guid = _peFile.MetaDataStreamGUID;
            Assert.Single(guid.GuidsAndIndices);
            Assert.Equal(new Guid("5250e853-c17a-4e76-adb3-0a716ec8af5d"), guid.Guids[0]);
        }

        [Fact]
        public void NetFrameworkConsole_MetaDataStreamBlob_ParseCorrectValues()
        {
            var blob = _peFile.MetaDataStreamBlob;

            // Just test a few values instead of the whole blob
            Assert.Equal(0x150, blob.Length);
            Assert.Equal(0x00, blob[0]);
            Assert.Equal(0x4E, blob[0x97]);
            Assert.Equal(0x00, blob[0x14F]);
        }

        [Fact]
        public void NetFrameworkConsole_MetaDataTablesHdr_ParseCorrectTables()
        {
            var metaDataTablesHdr = _peFile.MetaDataStreamTablesHeader;

            Assert.Equal("Module", metaDataTablesHdr.TableDefinitions[0].Name);
            Assert.Equal(1U, metaDataTablesHdr.TableDefinitions[0].RowCount);
            Assert.Equal(0U, metaDataTablesHdr.TableDefinitions[0].Offset);
            Assert.Equal(10U, metaDataTablesHdr.TableDefinitions[0].BytesPerRow);

            // ... More tables ...
          
            Assert.Equal("AssemblyRef", metaDataTablesHdr.TableDefinitions[35].Name);
            Assert.Equal(1U, metaDataTablesHdr.TableDefinitions[35].RowCount);
            Assert.Equal(454U, metaDataTablesHdr.TableDefinitions[35].Offset);
            Assert.Equal(20U, metaDataTablesHdr.TableDefinitions[35].BytesPerRow);
        }

        [Fact]
        public void NetFrameworkConsole_MetaDataTable_Module()
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
        public void NetFrameworkConsole_MetaDataTable_TypeRef()
        {
            var typeRef = _peFile.MetaDataStreamTablesHeader.Tables.TypeRef;

            Assert.Equal(23, typeRef.Count);

            Assert.Equal(0x0006u, typeRef[0].ResolutionScope);
            Assert.Equal(0x0160u, typeRef[0].TypeName);
            Assert.Equal(0x02A4u, typeRef[0].TypeNamespace);

            Assert.Equal(0x0006u, typeRef[22].ResolutionScope);
            Assert.Equal(0x0243u, typeRef[22].TypeName);
            Assert.Equal(0x0225u, typeRef[22].TypeNamespace);
        }
    }
}