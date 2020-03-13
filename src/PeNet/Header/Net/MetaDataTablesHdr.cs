using System;
using System.Collections.Generic;
using System.Linq;
using PeNet.FileParser;
using PeNet.Header.Net.MetaDataTables;

namespace PeNet.Header.Net
{
    public interface IMetaDataTablesHdr
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
        List<MetaDataTablesHdr.MetaDataTableInfo> TableDefinitions { get; }
    }

    /// <summary>
    /// The Meta Data Tables Header contains information about all present
    /// data tables in the .Net assembly.
    /// </summary>
    public class MetaDataTablesHdr : AbstractStructure, IMetaDataTablesHdr
    {
        private List<MetaDataTableInfo>? _tableDefinitions;
        private Tables? _tables = null;

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
        }

        /// <summary>
        /// Create a new Meta Data Tables Header instance from a byte array.
        /// </summary>
        /// <param name="peFile">PE file which contains a MetaDataTablesHdr structure.</param>
        /// <param name="offset">Offset in the PE file, where the header starts.</param>
        public MetaDataTablesHdr(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        /// <summary>
        /// Reserved1, always 0.
        /// </summary>
        public uint Reserved1
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        /// Major Version.
        /// </summary>
        public byte MajorVersion
        {
            get => PeFile.ReadByte(Offset + 0x4);
            set => PeFile.WriteByte(Offset + 0x4, value);
        }

        /// <summary>
        /// Minor Version.
        /// </summary>
        public byte MinorVersion
        {
            get => PeFile.ReadByte(Offset + 0x5);
            set => PeFile.WriteByte(Offset + 0x5, value);
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
            get => PeFile.ReadByte(Offset + 0x6);
            set => PeFile.WriteByte(Offset + 0x6, value);
        }

        /// <summary>
        /// Reserved2, always 1.
        /// </summary>
        public byte Reserved2
        {
            get => PeFile.ReadByte(Offset + 0x7);
            set => PeFile.WriteByte(Offset + 0x7, value);
        }

        /// <summary>
        /// Bit mask which shows, which tables are present in the .Net assembly. 
        /// Maximal 64 tables can be present, but most tables are not defined such that
        /// the high bits of the mask are always 0.
        /// </summary>
        public MaskValidType MaskValid
        {
            get => (MaskValidType) PeFile.ReadULong(Offset + 0x8);
            set => PeFile.WriteULong(Offset + 0x8, (ulong) value);
        }

        /// <summary>
        /// MaskValid flags resolved to readable strings.
        /// </summary>
        public List<string> MaskValidResolved
            => ResolveMaskValid(MaskValid);

        /// <summary>
        /// Bit mask which shows, which tables are sorted. 
        /// </summary>
        public ulong MaskSorted
        {
            get => PeFile.ReadULong(Offset + 0x10);
            set => PeFile.WriteULong(Offset + 0x10, value);
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

        /// <summary>
        /// Access all parsed meta data tables.
        /// </summary>
        public Tables Tables 
        {
            get
            {
                if(_tables is null)
                {
                    
                    _tables = ParseMetaDataTables();
                }

                return _tables;
            }
        }

        private List<MetaDataTableInfo> ParseTableDefinitions()
        {
            var heapSizes = new HeapSizes(HeapSizes);


            var tables = new MetaDataTableInfo[64];

            var startOfTableDefinitions = Offset + 24;
            var names = ResolveMaskValid(MaskValid);

            // Set number of rows per table
            var cnt = 0;
            for (var i = 0; i < tables.Length; ++i)
            {
                if (((ulong) MaskValid & (1UL << i)) != 0)
                {
                    tables[i].RowCount = PeFile.ReadUInt(startOfTableDefinitions + (uint)cnt * 4);
                    tables[i].Name = names[cnt];
                    cnt++;
                }
            }

            var indexSizes = new IndexSize(tables);

            // Set row size of tables
            tables[(int)MetadataToken.Module].BytesPerRow = 2 + heapSizes.String + heapSizes.Guid * 3;
            tables[(int)MetadataToken.TypeReference].BytesPerRow = indexSizes[Index.ResolutionScope] + heapSizes.String * 2;
            tables[(int)MetadataToken.TypeDef].BytesPerRow = 4 + heapSizes.String * 2 + indexSizes[Index.TypeDefOrRef] + indexSizes[Index.Field] + indexSizes[Index.MethodDef];
            tables[(int)MetadataToken.Field].BytesPerRow = 2 + heapSizes.String + heapSizes.Blob;
            tables[(int)MetadataToken.MethodDef].BytesPerRow = 8 + heapSizes.String + heapSizes.Blob + GetIndexSize(MetadataToken.Parameter, tables);
            tables[(int)MetadataToken.Parameter].BytesPerRow = 4 + heapSizes.String;
            tables[(int)MetadataToken.InterfaceImplementation].BytesPerRow = GetIndexSize(MetadataToken.TypeDef, tables) + indexSizes[Index.TypeDefOrRef];
            tables[(int)MetadataToken.MemberReference].BytesPerRow = indexSizes[Index.MemberRefParent] + heapSizes.String + heapSizes.Blob;
            tables[(int)MetadataToken.Constant].BytesPerRow = 2 + indexSizes[Index.HasConstant] + heapSizes.Blob;
            tables[(int)MetadataToken.CustomAttribute].BytesPerRow = indexSizes[Index.HasCustomAttribute] + indexSizes[Index.CustomAttributeType] + heapSizes.Blob;
            tables[(int)MetadataToken.FieldMarshal].BytesPerRow = indexSizes[Index.HasFieldMarshal] + heapSizes.Blob;
            tables[(int)MetadataToken.DeclarativeSecurity].BytesPerRow = 2 + indexSizes[Index.HasDeclSecurity] + heapSizes.Blob;
            tables[(int)MetadataToken.ClassLayout].BytesPerRow = 6 + GetIndexSize(MetadataToken.TypeDef, tables);
            tables[(int)MetadataToken.FieldLayout].BytesPerRow = 4 + GetIndexSize(MetadataToken.Field, tables);
            tables[(int)MetadataToken.StandAloneSignature].BytesPerRow = heapSizes.Blob;
            tables[(int)MetadataToken.EventMap].BytesPerRow = GetIndexSize(MetadataToken.TypeDef, tables) + GetIndexSize(MetadataToken.Event, tables);
            tables[(int)MetadataToken.Event].BytesPerRow = 2 + heapSizes.String + indexSizes[Index.TypeDefOrRef];
            tables[(int)MetadataToken.PropertyMap].BytesPerRow = GetIndexSize(MetadataToken.TypeDef, tables) + GetIndexSize(MetadataToken.Property, tables);
            tables[(int)MetadataToken.Property].BytesPerRow = 2 + heapSizes.String + heapSizes.Blob;
            tables[(int)MetadataToken.MethodSemantics].BytesPerRow = 2 + GetIndexSize(MetadataToken.MethodDef, tables) + indexSizes[Index.HasSemantics];
            tables[(int)MetadataToken.MethodImplementation].BytesPerRow = GetIndexSize(MetadataToken.TypeDef, tables) + indexSizes[Index.MethodDefOrRef] * 2;
            tables[(int)MetadataToken.ModuleReference].BytesPerRow = heapSizes.String;
            tables[(int)MetadataToken.TypeSpecification].BytesPerRow = heapSizes.Blob;
            tables[(int)MetadataToken.ImplementationMap].BytesPerRow = 2 + indexSizes[Index.MemberForwarded] + heapSizes.String + GetIndexSize(MetadataToken.ModuleReference, tables);
            tables[(int)MetadataToken.FieldRVA].BytesPerRow = 4 + GetIndexSize(MetadataToken.Field, tables);
            tables[(int)MetadataToken.Assembly].BytesPerRow = 16 + heapSizes.Blob + heapSizes.String * 2;
            tables[(int)MetadataToken.AssemblyProcessor].BytesPerRow = 4;
            tables[(int)MetadataToken.AssemblyOS].BytesPerRow = 12;
            tables[(int)MetadataToken.AssemblyReference].BytesPerRow = 12 + heapSizes.Blob * 2 + heapSizes.String * 2;
            tables[(int)MetadataToken.AssemblyReferenceProcessor].BytesPerRow = 4 + GetIndexSize(MetadataToken.AssemblyReference, tables);
            tables[(int)MetadataToken.AssemblyReferenceOS].BytesPerRow = 12 + GetIndexSize(MetadataToken.AssemblyReference, tables);
            tables[(int)MetadataToken.File].BytesPerRow = 4 + heapSizes.String + heapSizes.Blob;
            tables[(int)MetadataToken.ExportedType].BytesPerRow = 8 + heapSizes.String * 2 + indexSizes[Index.Implementation];
            tables[(int)MetadataToken.ManifestResource].BytesPerRow = 8 + heapSizes.String + indexSizes[Index.Implementation];
            tables[(int)MetadataToken.NestedClass].BytesPerRow = GetIndexSize(MetadataToken.NestedClass, tables) * 2;
            tables[(int)MetadataToken.GenericParameter].BytesPerRow = 4 + indexSizes[Index.TypeOrMethodDef] + heapSizes.String;
            tables[(int)MetadataToken.MethodSpecification].BytesPerRow = indexSizes[Index.MethodDefOrRef] + heapSizes.Blob;
            tables[(int)MetadataToken.GenericParameterConstraint].BytesPerRow = GetIndexSize(MetadataToken.GenericParameter, tables) + indexSizes[Index.TypeDefOrRef];


            // Set offset of tables
            uint offset = 0;
            for (int i = 0; i < tables.Length; ++i)
            {
                tables[i].Offset = offset;
                offset += tables[i].BytesPerRow * tables[i].RowCount;
            }

            return tables.ToList();
        }

        private Tables ParseMetaDataTables()
        {
            var tables = new Tables
            {
                Module                  = ParseTable<Module>(MetadataToken.Module),
                TypeRef                 = ParseTable<TypeRef>(MetadataToken.TypeReference),
                TypeDef                 = ParseTable<TypeDef>(MetadataToken.TypeDef),
                Field                   = ParseTable<Field>(MetadataToken.Field),
                MethodDef               = ParseTable<MethodDef>(MetadataToken.MethodDef),
                Param                   = ParseTable<Param>(MetadataToken.Parameter),
                InterfaceImpl           = ParseTable<InterfaceImpl>(MetadataToken.InterfaceImplementation),
                MemberRef               = ParseTable<MemberRef>(MetadataToken.MemberReference),
                Constant                = ParseTable<Constant>(MetadataToken.Constant),
                CustomAttribute         = ParseTable<CustomAttribute>(MetadataToken.CustomAttribute),
                FieldMarshal            = ParseTable<FieldMarshal>(MetadataToken.FieldMarshal),
                DeclSecurity            = ParseTable<DeclSecurity>(MetadataToken.DeclarativeSecurity),
                ClassLayout             = ParseTable<ClassLayout>(MetadataToken.ClassLayout),
                FieldLayout             = ParseTable<FieldLayout>(MetadataToken.FieldLayout),
                StandAloneSig           = ParseTable<StandAloneSig>(MetadataToken.StandAloneSignature),
                EventMap                = ParseTable<EventMap>(MetadataToken.EventMap),
                Event                   = ParseTable<Event>(MetadataToken.Event),
                PropertyMap             = ParseTable<PropertyMap>(MetadataToken.PropertyMap),
                Property                = ParseTable<Property>(MetadataToken.Property),
                MethodSemantic          = ParseTable<MethodSemantics>(MetadataToken.MethodSemantics),
                MethodImpl              = ParseTable<MethodImpl>(MetadataToken.MethodImplementation),
                ModuleRef               = ParseTable<ModuleRef>(MetadataToken.ModuleReference),
                TypeSpec                = ParseTable<TypeSpec>(MetadataToken.TypeSpecification),
                ImplMap                 = ParseTable<ImplMap>(MetadataToken.ImplementationMap),
                FieldRVA                = ParseTable<FieldRVA>(MetadataToken.FieldRVA),
                Assembly                = ParseTable<Assembly>(MetadataToken.Assembly),
                AssemblyProcessor       = ParseTable<AssemblyProcessor>(MetadataToken.AssemblyProcessor),
                AssemblyOS              = ParseTable<AssemblyOS>(MetadataToken.AssemblyOS),
                AssemblyRef             = ParseTable<AssemblyRef>(MetadataToken.AssemblyReference),
                AssemblyRefProcessor    = ParseTable<AssemblyRefProcessor>(MetadataToken.AssemblyReferenceProcessor),
                AssemblyRefOS           = ParseTable<AssemblyRefOS>(MetadataToken.AssemblyReferenceOS),
                File                    = ParseTable<File>(MetadataToken.File),
                ExportedType            = ParseTable<ExportedType>(MetadataToken.ExportedType),
                ManifestResource        = ParseTable<ManifestResource>(MetadataToken.ManifestResource),
                NestedClass             = ParseTable<NestedClass>(MetadataToken.NestedClass),
                GenericParam            = ParseTable<GenericParam>(MetadataToken.GenericParameter),
                GenericParamConstraints = ParseTable<GenericParamConstraint>(MetadataToken.GenericParameterConstraint)
            };

            return tables;
        }

        private List<T> ParseTable<T>(MetadataToken token)
            where T : AbstractTable
        {
            var heapSizes = new HeapSizes(HeapSizes);
            var indexSizes = new IndexSize(TableDefinitions.ToArray());
            var tablesOffset = (uint)(Offset + 0x18u + HammingWeight((ulong) MaskValid) * 4u);

            var tableInfo = TableDefinitions[(int)token];
            var rows = new List<T>();

            if(tableInfo.RowCount != 0)
            {
                for(var i = 0u; i < tableInfo.RowCount; i++)
                {
                    rows.Add((T) Activator.CreateInstance(typeof(T), new object[] 
                        {
                            PeFile, tablesOffset + tableInfo.Offset + tableInfo.BytesPerRow * i, heapSizes, indexSizes
                        }));
                }
            }

            return rows.Count == 0 ? new List<T>(0) : rows;
        }

        private int HammingWeight(ulong value)
        {
            var count = 0;
            while (value != 0)
            {
                ++count;
                value &= (value - 1);
            }
            return count;
        }

        private uint GetIndexSize(MetadataToken table, MetaDataTableInfo[] tables)
        {
            return tables[(int)table].RowCount <= UInt16.MaxValue ? 2U : 4U;
        }

        /// <summary>
        ///     Resolve which tables are present in the .Net header based
        ///     on the MaskValid flags from the MetaDataTablesHdr.
        /// </summary>
        /// <param name="maskValid">MaskValid value from the MetaDataTablesHdr</param>
        /// <returns>List with present table names.</returns>
        public static List<string> ResolveMaskValid(MaskValidType maskValid)
        {
            var st = new List<string>();
            foreach (var flag in (MaskValidType[])Enum.GetValues(typeof(MaskValidType)))
            {
                if ((maskValid & flag) == flag)
                {
                    st.Add(flag.ToString());
                }
            }
            return st;
        }
    }

    /// <summary>
    /// MaskValid flags from the MetaDataTablesHdr.
    /// The flags show, which tables are present.
    /// </summary>
    [Flags]
    public enum MaskValidType : ulong
    {
        /// <summary>
        /// Table Module is present.
        /// </summary>
        Module = 0x1,

        /// <summary>
        /// Table TypeRef is present.
        /// </summary>
        TypeRef = 0x2,

        /// <summary>
        /// Table TypeDef is present.
        /// </summary>
        TypeDef = 0x4,

        /// <summary>
        /// Table Field is present.
        /// </summary>
        Field = 0x10,

        /// <summary>
        /// Table MethodDef is present.
        /// </summary>
        MethodDef = 0x40,

        /// <summary>
        /// Table Param is present.
        /// </summary>
        Param = 0x100,

        /// <summary>
        /// Table InterfaceImpl is present.
        /// </summary>
        InterfaceImpl = 0x200,

        /// <summary>
        /// Table MemberRef is present.
        /// </summary>
        MemberRef = 0x400,

        /// <summary>
        /// Table Constant is present.
        /// </summary>
        Constant = 0x800,

        /// <summary>
        /// Table CustomAttribute is present.
        /// </summary>
        CustomAttribute = 0x1000,

        /// <summary>
        /// Table FieldMarshal is present.
        /// </summary>
        FieldMarshal = 0x2000,

        /// <summary>
        /// Table DeclSecurity is present.
        /// </summary>
        DeclSecurity = 0x4000,

        /// <summary>
        /// Table ClassLayout is present.
        /// </summary>
        ClassLayout = 0x8000,

        /// <summary>
        /// Table FieldLayout is present.
        /// </summary>
        FieldLayout = 0x10000,

        /// <summary>
        /// Table StandAloneSig is present.
        /// </summary>
        StandAloneSig = 0x20000,

        /// <summary>
        /// Table EventMap is present.
        /// </summary>
        EventMap = 0x40000,

        /// <summary>
        /// Table Event is present.
        /// </summary>
        Event = 0x100000,

        /// <summary>
        /// Table PropertyMap is present.
        /// </summary>
        PropertyMap = 0x200000,

        /// <summary>
        /// Table Property is present.
        /// </summary>
        Property = 0x800000,

        /// <summary>
        /// Table MethodSemantics is present.
        /// </summary>
        MethodSemantics = 0x1000000,

        /// <summary>
        /// Table MethodImpl is present.
        /// </summary>
        MethodImpl = 0x2000000,

        /// <summary>
        /// Table ModuleRef is present.
        /// </summary>
        ModuleRef = 0x4000000,

        /// <summary>
        /// Table TypeSpec is present.
        /// </summary>
        TypeSpec = 0x8000000,

        /// <summary>
        /// Table ImplMap is present.
        /// </summary>
        ImplMap = 0x10000000,

        /// <summary>
        /// Table FieldRVA is present.
        /// </summary>
        FieldRva = 0x20000000,

        /// <summary>
        /// Table Assembly is present.
        /// </summary>
        Assembly = 0x100000000,

        /// <summary>
        /// Table AssemblyProcessor is present.
        /// </summary>
        AssemblyProcessor = 0x200000000,

        /// <summary>
        /// Table AssemblyOS is present.
        /// </summary>
        AssemblyOS = 0x400000000,

        /// <summary>
        /// Table AssemblyRef is present.
        /// </summary>
        AssemblyRef = 0x800000000,

        /// <summary>
        /// Table AssemblyRefProcessor is present.
        /// </summary>
        AssemblyRefProcessor = 0x1000000000,

        /// <summary>
        /// Table AssemblyRefOS is present.
        /// </summary>
        AssemblyRefOS = 0x2000000000,

        /// <summary>
        /// Table File is present.
        /// </summary>
        File = 0x4000000000,

        /// <summary>
        /// Table ExportedType is present.
        /// </summary>
        ExportedType = 0x8000000000,

        /// <summary>
        /// Table ManifestResource is present.
        /// </summary>
        ManifestResource = 0x10000000000,

        /// <summary>
        /// Table NestedClass is present.
        /// </summary>
        NestedClass = 0x20000000000,

        /// <summary>
        /// Table GenericParam is present.
        /// </summary>
        GenericParam = 0x40000000000,

        /// <summary>
        /// Table MethodSpec is present
        /// </summary>
        MethodSpec = 0x80000000000,

        /// <summary>
        /// Table GenericParamConstraint is present.
        /// </summary>
        GenericParamConstraint = 0x100000000000
    }
}