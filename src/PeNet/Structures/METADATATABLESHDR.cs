using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeNet.Test.Structures;
using PeNet.Utilities;

namespace PeNet.Structures
{
    public interface IMETADATATABLESHDR
    {
        /// <summary>
        /// The size the indexes into the streams have. 
        /// Bit 0 (0x01) set: Indexes into #String are 4 bytes wide.
        /// Bit 1 (0x02) set: Indexes into #GUID heap are 4 bytes wide.
        /// Bit 2 (0x04) set: Indexes into #Blob heap are 4 bytes wide.
        /// If bit not set: indexes into heap is 2 bytes wide.
        /// </summary>
        byte HeapSizes { get; set; }

        /// <summary>
        /// Access a list of defined tables in the Meta Data Tables Header
        /// with the name and number of rows of the table.
        /// </summary>
        List<METADATATABLESHDR.MetaDataTableInfo> TableDefinitions { get; }
    }

    /// <summary>
    /// The Meta Data Tables Header contains information about all present
    /// data tables in the .Net assembly.
    /// </summary>
    public class METADATATABLESHDR : AbstractStructure, IMETADATATABLESHDR
    {
        private List<MetaDataTableInfo> _tableDefinitions;

        /// <summary>
        /// Represents an table definition entry from the list
        /// of available tables in the Meta Data Tables Header 
        /// in the .Net header of an assembly.
        /// </summary>
        public struct MetaDataTableInfo
        {
            /// <summary>
            /// Number of rows of the table.
            /// </summary>
            public uint RowCount { get; set; }

            /// <summary>
            /// Name of the table.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Offset where the table starts.
            /// </summary>
            public uint Offset { get; set; }

            /// <summary>
            /// Byter per row in the table.
            /// </summary>
            public uint BytesPerRow { get; set;}

            /// <summary>
            ///     Create a string representation of the objects
            ///     properties.
            /// </summary>
            /// <returns>The TableDefinition properties as a string.</returns>
            public override string ToString()
            {
                var sb = new StringBuilder("Table Definition\n");
                sb.Append(this.PropertiesToString("{0,-10}:\t{1,10:X}\n"));

                return sb.ToString();
            }
        }

        /// <summary>
        /// Create a new Meta Data Tables Header instance from a byte array.
        /// </summary>
        /// <param name="buff">Buffer which contains a METADATATABLESHDR structure.</param>
        /// <param name="offset">Offset in the buffer, where the header starts.</param>
        public METADATATABLESHDR(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        /// <summary>
        /// Reserved1, always 0.
        /// </summary>
        public uint Reserved1
        {
            get { return Buff.BytesToUInt32(Offset); }
            set { Buff.SetUInt32(Offset, value); }
        }

        /// <summary>
        /// Major Version.
        /// </summary>
        public byte MajorVersion
        {
            get { return Buff[Offset + 0x4]; }
            set { Buff[Offset + 0x4] = value; }
        }

        /// <summary>
        /// Minor Version.
        /// </summary>
        public byte MinorVersion
        {
            get { return Buff[Offset + 0x5]; }
            set { Buff[Offset + 0x5] = value; }
        }

        /// <summary>
        /// The size the indexes into the streams have. 
        /// Bit 0 (0x01) set: Indexes into #String are 4 bytes wide.
        /// Bit 1 (0x02) set: Indexes into #GUID heap are 4 bytes wide.
        /// Bit 2 (0x04) set: Indexes into #Blob heap are 4 bytes wide.
        /// If bit not set: indexes into heap is 2 bytes wide.
        /// </summary>
        public byte HeapSizes
        {
            get { return Buff[Offset + 0x6]; }
            set { Buff[Offset + 0x6] = value; }
        }

        /// <summary>
        /// Reserved2, always 1.
        /// </summary>
        public byte Reserved2
        {
            get { return Buff[Offset + 0x7]; }
            set { Buff[Offset + 0x7] = value; }
        }

        /// <summary>
        /// Bit mask which shows, which tables are present in the .Net assembly. 
        /// Maximal 64 tables can be present, but most tables are not defined such that
        /// the high bits of the mask are always 0.
        /// </summary>
        public ulong Valid
        {
            get { return Buff.BytesToUInt64(Offset + 0x8); }
            set { Buff.SetUInt64(Offset + 0x8, value); }
        }

        /// <summary>
        /// Bit mask which shows, which tables are sorted. 
        /// </summary>
        public ulong MaskSorted
        {
            get { return Buff.BytesToUInt64(Offset + 0x10); }
            set { Buff.SetUInt64(Offset + 0x10, value); }
        }

        /// <summary>
        /// Access a list of defined tables in the Meta Data Tables Header
        /// with the name and number of rows of the table.
        /// </summary>
        public List<MetaDataTableInfo> TableDefinitions
        {
            get
            {
                if (_tableDefinitions != null)
                    return _tableDefinitions;

                _tableDefinitions = ParseTableDefinitions();
                return _tableDefinitions;
            }
        }

        private List<MetaDataTableInfo> ParseTableDefinitions()
        {
            var heapSizes = new HeapSizes(HeapSizes);
            var tables = new MetaDataTableInfo[64];

            var startOfTableDefinitions = Offset + 24;
            var names = FlagResolver.ResolveMaskValidFlags(Valid);

            var cnt = 0;
            for (var i = 0; i < tables.Length; ++i)
            {
                if((Valid & (1UL << i)) != 0)
                {
                    tables[i].RowCount = Buff.BytesToUInt32(startOfTableDefinitions + (uint) cnt*4);
                    tables[i].Name = names[cnt];
                    cnt++;
                }
            }

            tables[(int)MetadataToken.Module].BytesPerRow = 2 + heapSizes.String + heapSizes.Guid * 3;
			tables[(int)MetadataToken.TypeReference].BytesPerRow = GetIndexSize(CodedIndex.ResolutionScope, tables) + heapSizes.String * 2;
			tables[(int)MetadataToken.Type].BytesPerRow = 4 + heapSizes.String * 2 + GetIndexSize(CodedIndex.TypeDefOrRef, tables)
				+ GetIndexSize(MetadataToken.Field, tables) + GetIndexSize(MetadataToken.Method, tables);
			tables[(int)MetadataToken.Field].BytesPerRow = 2 + heapSizes.String + heapSizes.Blob;
			tables[(int)MetadataToken.Method].BytesPerRow = 8 + heapSizes.String + heapSizes.Blob + GetIndexSize(MetadataToken.Parameter, tables);
			tables[(int)MetadataToken.Parameter].BytesPerRow = 4 + heapSizes.String;
			tables[(int)MetadataToken.InterfaceImplementation].BytesPerRow = GetIndexSize(MetadataToken.Type, tables) + GetIndexSize(CodedIndex.TypeDefOrRef, tables);
			tables[(int)MetadataToken.MemberReference].BytesPerRow = GetIndexSize(CodedIndex.MemberRefParent, tables) + heapSizes.String + heapSizes.Blob;
			tables[(int)MetadataToken.Constant].BytesPerRow = 2 + GetIndexSize(CodedIndex.HasConstant, tables) + heapSizes.Blob;
			tables[(int)MetadataToken.CustomAttribute].BytesPerRow = GetIndexSize(CodedIndex.HasCustomAttribute, tables)
				+ GetIndexSize(CodedIndex.CustomAttributeType, tables) + heapSizes.Blob;
			tables[(int)MetadataToken.FieldMarshal].BytesPerRow = GetIndexSize(CodedIndex.HasFieldMarshal, tables) + heapSizes.Blob;
			tables[(int)MetadataToken.DeclarativeSecurity].BytesPerRow = 2 + GetIndexSize(CodedIndex.HasDeclSecurity, tables) + heapSizes.Blob;
			tables[(int)MetadataToken.ClassLayout].BytesPerRow = 6 + GetIndexSize(MetadataToken.Type, tables);
			tables[(int)MetadataToken.FieldLayout].BytesPerRow = 4 + GetIndexSize(MetadataToken.Field, tables);
			tables[(int)MetadataToken.Signature].BytesPerRow = heapSizes.Blob;
			tables[(int)MetadataToken.EventMap].BytesPerRow = GetIndexSize(MetadataToken.Type, tables) + GetIndexSize(MetadataToken.Event, tables);
			tables[(int)MetadataToken.Event].BytesPerRow = 2 + heapSizes.String + GetIndexSize(CodedIndex.TypeDefOrRef, tables);
			tables[(int)MetadataToken.PropertyMap].BytesPerRow = GetIndexSize(MetadataToken.Type, tables) + GetIndexSize(MetadataToken.Property, tables);
			tables[(int)MetadataToken.Property].BytesPerRow = 2 + heapSizes.String + heapSizes.Blob;
			tables[(int)MetadataToken.MethodSemantics].BytesPerRow = 2 + GetIndexSize(MetadataToken.Method, tables) + GetIndexSize(CodedIndex.HasSemantics, tables);
			tables[(int)MetadataToken.MethodImplementation].BytesPerRow = GetIndexSize(MetadataToken.Type, tables) + GetIndexSize(CodedIndex.MethodDefOrRef, tables) * 2;
			tables[(int)MetadataToken.ModuleReference].BytesPerRow = heapSizes.String;
			tables[(int)MetadataToken.TypeSpecification].BytesPerRow = heapSizes.Blob;
			tables[(int)MetadataToken.ImplementationMap].BytesPerRow = 2 + GetIndexSize(CodedIndex.MemberForwarded, tables)
				+ heapSizes.String + GetIndexSize(MetadataToken.ModuleReference, tables);
			tables[(int)MetadataToken.FieldRva].BytesPerRow = 4 + GetIndexSize(MetadataToken.Field, tables);
			tables[(int)MetadataToken.Assembly].BytesPerRow = 16 + heapSizes.Blob + heapSizes.String * 2;
			tables[(int)MetadataToken.AssemblyProcessor].BytesPerRow = 4;
			tables[(int)MetadataToken.AssemblyOS].BytesPerRow = 12;
			tables[(int)MetadataToken.AssemblyReference].BytesPerRow = 12 + heapSizes.Blob * 2 + heapSizes.String * 2;
			tables[(int)MetadataToken.AssemblyReferenceProcessor].BytesPerRow = 4 + GetIndexSize(MetadataToken.AssemblyReference, tables);
			tables[(int)MetadataToken.AssemblyReferenceOS].BytesPerRow = 12 + GetIndexSize(MetadataToken.AssemblyReference, tables);
			tables[(int)MetadataToken.File].BytesPerRow = 4 + heapSizes.String + heapSizes.Blob;
			tables[(int)MetadataToken.ExportedType].BytesPerRow = 8 + heapSizes.String * 2 + GetIndexSize(CodedIndex.Implementation, tables);
			tables[(int)MetadataToken.ManifestResource].BytesPerRow = 8 + heapSizes.String + GetIndexSize(CodedIndex.Implementation, tables);
			tables[(int)MetadataToken.NestedClass].BytesPerRow = GetIndexSize(MetadataToken.NestedClass, tables) * 2;
			tables[(int)MetadataToken.GenericParameter].BytesPerRow = 4 + GetIndexSize(CodedIndex.TypeOrMethodDef, tables) + heapSizes.String;
			tables[(int)MetadataToken.MethodSpecification].BytesPerRow = GetIndexSize(CodedIndex.MethodDefOrRef, tables) + heapSizes.Blob;
			tables[(int)MetadataToken.GenericParameterConstraint].BytesPerRow = GetIndexSize(MetadataToken.GenericParameter, tables) + GetIndexSize(CodedIndex.TypeDefOrRef, tables);


            uint offset = 0;
			for (int i = 0; i < tables.Length; ++i)
			{
				tables[i].Offset = offset;
				offset += tables[i].BytesPerRow * tables[i].RowCount;
			}

      
            return tables.ToList();
        }

        private uint GetIndexSize(CodedIndex codedIndex, MetaDataTableInfo[] tables)
		{
			uint maxRowCount = 0;
			for (int i = 0; i < codedIndex.TableCount; ++i)
			{
				var table = codedIndex.GetTable(i);
				if (table.HasValue)
				{
					var rowCount = tables[(int)table.Value].RowCount;
					if (rowCount > maxRowCount) maxRowCount = rowCount;
				}
			}

			int valueBitCount = 16 - codedIndex.TagBitCount;
			return maxRowCount < (1U << valueBitCount) ? 2U : 4U;
		}

        private uint GetIndexSize(MetadataToken table, MetaDataTableInfo[] tables)
		{
			return tables[(int)table].RowCount <= ushort.MaxValue ? 2U : 4U;
		}

        /// <summary>
        ///     Create a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>The METADATATABLESHDR properties as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("METADATATABLESHDR\n");
            sb.Append(this.PropertiesToString("{0,-10}:\t{1,10:X}\n"));

            return sb.ToString();
        }
    }
}