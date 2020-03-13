using System;
using System.Collections.Generic;
using static PeNet.Header.Net.MetaDataTablesHdr;

namespace PeNet.Header.Net
{
	internal enum MetadataToken
	{
		Module = 0x00,
		TypeReference = 0x01,
		TypeDef = 0x02,
		Field = 0x04,
		MethodDef = 0x06,
		Parameter = 0x08,
		InterfaceImplementation = 0x09,
		MemberReference = 0x0A,
		Constant = 0x0B,
		CustomAttribute = 0x0C,
		FieldMarshal = 0x0D,
		DeclarativeSecurity = 0x0E,
		ClassLayout = 0x0F,
		FieldLayout = 0x10,
		StandAloneSignature = 0x11,
		EventMap = 0x12,
		Event = 0x14,
		PropertyMap = 0x15,
		Property = 0x17,
		MethodSemantics = 0x18,
		MethodImplementation = 0x19,
		ModuleReference = 0x1A,
		TypeSpecification = 0x1B,
		ImplementationMap = 0x1C,
		FieldRVA = 0x1D,
		Assembly = 0x20,
		AssemblyProcessor = 0x21,
		AssemblyOS = 0x22,
		AssemblyReference = 0x23,
		AssemblyReferenceProcessor = 0x24,
		AssemblyReferenceOS = 0x25,
		File = 0x26,
		ExportedType = 0x27,
		ManifestResource = 0x28,
		NestedClass = 0x29,
		GenericParameter = 0x2A,
		MethodSpecification = 0x2B,
		GenericParameterConstraint = 0x2C,
		String = 0x70,
		Name = 0x71,
		BaseType = 0x72,
	}

    interface IMetaDataIndex
    {
        uint Size {get;}
    }


    public enum Index
    {
        String,
        Guid,
        MethodDef,
        Field,
        Param,
        Event,
        TypeDef,
        Property,
        ModuleRef,
        AssemblyRef,
        GenericParam,
        TypeDefOrRef,
        HasConstant,
        HasCustomAttribute,
        HasFieldMarshal,
        HasDeclSecurity,
        MemberRefParent,
        HasSemantics,
        MethodDefOrRef,
        MemberForwarded,
        Implementation,
        CustomAttributeType,
        ResolutionScope,
        TypeOrMethodDef
    };

    public class IndexSize
    {
        private const byte Unused = 0xFF;
        public IndexSize(MetaDataTableInfo[] tables)
        {
            _index = new Dictionary<Index, IMetaDataIndex>
            {
                // Single Indices
                {Index.MethodDef, new SingleIndex(MetadataToken.MethodDef, tables)},
                {Index.Field, new SingleIndex(MetadataToken.Field, tables)},
                {Index.Param, new SingleIndex(MetadataToken.Parameter, tables)},
                {Index.TypeDef, new SingleIndex(MetadataToken.TypeDef, tables)},
                {Index.Event, new SingleIndex(MetadataToken.Event, tables)},
                {Index.Property, new SingleIndex(MetadataToken.Property, tables)},
                {Index.ModuleRef, new SingleIndex(MetadataToken.ModuleReference, tables)},
                {Index.AssemblyRef, new SingleIndex(MetadataToken.AssemblyReference, tables)},
                {Index.GenericParam, new SingleIndex(MetadataToken.GenericParameter, tables)},

                // Coded Indices
                {Index.TypeDefOrRef, new CodedIndex(tables, (byte)MetadataToken.TypeDef, (byte)MetadataToken.TypeReference, (byte)MetadataToken.TypeSpecification)},
		        {Index.HasConstant, new CodedIndex(tables, (byte)MetadataToken.Field, (byte)MetadataToken.Parameter, (byte)MetadataToken.Property)},
                {Index.HasCustomAttribute, new CodedIndex(tables,
                (byte)MetadataToken.MethodDef, (byte)MetadataToken.Field, (byte)MetadataToken.TypeReference, (byte)MetadataToken.TypeDef,
                (byte)MetadataToken.Parameter, (byte)MetadataToken.InterfaceImplementation, (byte)MetadataToken.MemberReference, (byte)MetadataToken.Module,
                (byte)MetadataToken.DeclarativeSecurity, (byte)MetadataToken.Property, (byte)MetadataToken.Event, (byte)MetadataToken.StandAloneSignature,
                (byte)MetadataToken.ModuleReference, (byte)MetadataToken.TypeSpecification, (byte)MetadataToken.Assembly, (byte)MetadataToken.AssemblyReference,
                (byte)MetadataToken.Field, (byte)MetadataToken.ExportedType, (byte)MetadataToken.ManifestResource) },
                {Index.HasFieldMarshal, new CodedIndex(tables, (byte)MetadataToken.Field, (byte)MetadataToken.Parameter)},
                {Index.HasDeclSecurity, new CodedIndex(tables, (byte)MetadataToken.TypeDef, (byte)MetadataToken.MethodDef, (byte)MetadataToken.Assembly)},
                {Index.MemberRefParent, new CodedIndex(tables, (byte)MetadataToken.TypeDef, (byte)MetadataToken.TypeReference, (byte)MetadataToken.ModuleReference, (byte)MetadataToken.MethodDef, (byte)MetadataToken.TypeSpecification)},
                {Index.HasSemantics, new CodedIndex(tables, (byte)MetadataToken.Event, (byte)MetadataToken.Property)},
                {Index.MethodDefOrRef, new CodedIndex(tables, (byte)MetadataToken.MethodDef, (byte)MetadataToken.MemberReference)},
                {Index.MemberForwarded, new CodedIndex(tables, (byte)MetadataToken.Field, (byte)MetadataToken.MethodDef)},
                {Index.Implementation, new CodedIndex(tables, (byte)MetadataToken.Field, (byte)MetadataToken.AssemblyReference, (byte)MetadataToken.ExportedType)},
                {Index.CustomAttributeType, new CodedIndex(tables, Unused, Unused, (byte)MetadataToken.MethodDef, (byte)MetadataToken.MemberReference, Unused)},
                {Index.ResolutionScope, new CodedIndex(tables, (byte)MetadataToken.Module, (byte)MetadataToken.ModuleReference, (byte)MetadataToken.AssemblyReference, (byte)MetadataToken.TypeReference)},
                {Index.TypeOrMethodDef, new CodedIndex(tables, (byte)MetadataToken.TypeDef, (byte)MetadataToken.MethodDef)}

            };
        }
    

        private readonly Dictionary<Index, IMetaDataIndex> _index;

        public uint this[Index index] => _index[index].Size;
    }



    class SingleIndex : IMetaDataIndex
    {
        private readonly MetadataToken _token;
        private readonly MetaDataTableInfo[] _tables;

        public SingleIndex(MetadataToken token, MetaDataTableInfo[] tables)
        {
            _token = token;
            _tables = tables;
        }

        public uint Size => _tables[(int)_token].RowCount <= ushort.MaxValue ? 2U : 4U;
    }

    class CodedIndex : IMetaDataIndex
    {
		private const byte Unused = 0xFF;
		private readonly byte[] _tokens;
        private readonly MetaDataTableInfo[] _tables;
        private readonly int _tagBitCount;

        public CodedIndex(MetaDataTableInfo[] tables, params byte[] tokens)
		{
			_tokens = tokens;
            _tables = tables;
			_tagBitCount = (int)Math.Ceiling(Math.Log(tokens.Length, 2));
		}

        public uint Size 
        {
            get
            {
                uint maxRowCount = 0;
		        for (var i = 0; i < _tokens.Length; ++i)
			    {
				    var table = GetTable(i);
				    if (table.HasValue)
				    {
					    var rowCount = _tables[(int)table.Value].RowCount;
					    if (rowCount > maxRowCount) maxRowCount = rowCount;
				    }
			    }

			    var valueBitCount = 16 - _tagBitCount;
			    return maxRowCount < (1U << valueBitCount) ? 2U : 4U;
                }
        }
		
		private MetadataToken? GetTable(int tag)
		{
			var table = _tokens[tag];
			return table == Unused ? null : (MetadataToken?)table;
		}
    }
}
