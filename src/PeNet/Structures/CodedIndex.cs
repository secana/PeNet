using System;

namespace PeNet.Test.Structures
{
    /// <summary>
	/// The type of the Meta Data Table index.
	/// </summary>
	public enum MetadataToken
	{
		Module = 0x00,
		TypeReference = 0x01,
		Type = 0x02,
		Field = 0x04,
		Method = 0x06,
		Parameter = 0x08,
		InterfaceImplementation = 0x09,
		MemberReference = 0x0A,
		Constant = 0x0B,
		CustomAttribute = 0x0C,
		FieldMarshal = 0x0D,
		DeclarativeSecurity = 0x0E,
		ClassLayout = 0x0F,
		FieldLayout = 0x10,
		Signature = 0x11,
		EventMap = 0x12,
		Event = 0x14,
		PropertyMap = 0x15,
		Property = 0x17,
		MethodSemantics = 0x18,
		MethodImplementation = 0x19,
		ModuleReference = 0x1A,
		TypeSpecification = 0x1B,
		ImplementationMap = 0x1C,
		FieldRva = 0x1D,
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

    class CodedIndex
    {
		private const byte unused = 0xFF;

		public static readonly CodedIndex TypeDefOrRef = new CodedIndex(
			(byte)MetadataToken.Type, (byte)MetadataToken.TypeReference, (byte)MetadataToken.TypeSpecification);
		public static readonly CodedIndex HasConstant = new CodedIndex(
			(byte)MetadataToken.Field, (byte)MetadataToken.Parameter, (byte)MetadataToken.Property);
		public static readonly CodedIndex HasCustomAttribute = new CodedIndex(
			(byte)MetadataToken.Method, (byte)MetadataToken.Field, (byte)MetadataToken.TypeReference, (byte)MetadataToken.Type,
			(byte)MetadataToken.Parameter, (byte)MetadataToken.InterfaceImplementation, (byte)MetadataToken.MemberReference, (byte)MetadataToken.Module,
			(byte)MetadataToken.DeclarativeSecurity, (byte)MetadataToken.Property, (byte)MetadataToken.Event, (byte)MetadataToken.Signature,
			(byte)MetadataToken.ModuleReference, (byte)MetadataToken.TypeSpecification, (byte)MetadataToken.Assembly, (byte)MetadataToken.AssemblyReference,
			(byte)MetadataToken.Field, (byte)MetadataToken.ExportedType, (byte)MetadataToken.ManifestResource);
		public static readonly CodedIndex HasFieldMarshal = new CodedIndex(
			(byte)MetadataToken.Field, (byte)MetadataToken.Parameter);
		public static readonly CodedIndex HasDeclSecurity = new CodedIndex(
			(byte)MetadataToken.Type, (byte)MetadataToken.Method, (byte)MetadataToken.Assembly);
		public static readonly CodedIndex MemberRefParent = new CodedIndex(
			(byte)MetadataToken.Type, (byte)MetadataToken.TypeReference, (byte)MetadataToken.ModuleReference, (byte)MetadataToken.Method, (byte)MetadataToken.TypeSpecification);
		public static readonly CodedIndex HasSemantics = new CodedIndex(
			(byte)MetadataToken.Event, (byte)MetadataToken.Property);
		public static readonly CodedIndex MethodDefOrRef = new CodedIndex(
			(byte)MetadataToken.Method, (byte)MetadataToken.MemberReference);
		public static readonly CodedIndex MemberForwarded = new CodedIndex(
			(byte)MetadataToken.Field, (byte)MetadataToken.Method);
		public static readonly CodedIndex Implementation = new CodedIndex
			((byte)MetadataToken.Field, (byte)MetadataToken.AssemblyReference, (byte)MetadataToken.ExportedType);
		public static readonly CodedIndex CustomAttributeType = new CodedIndex(
			unused, unused, (byte)MetadataToken.Method, (byte)MetadataToken.MemberReference, unused);
		public static readonly CodedIndex ResolutionScope = new CodedIndex(
			(byte)MetadataToken.Module, (byte)MetadataToken.ModuleReference, (byte)MetadataToken.AssemblyReference, (byte)MetadataToken.TypeReference);
		public static readonly CodedIndex TypeOrMethodDef = new CodedIndex(
			(byte)MetadataToken.Type, (byte)MetadataToken.Method);

		private readonly byte[] _tables;

		internal CodedIndex(params byte[] tables)
		{
			_tables = tables;
			TagBitCount = (int)Math.Ceiling(Math.Log(tables.Length, 2));
		}

		public int TableCount => _tables.Length;

		public int TagBitCount {get;}
		
		public MetadataToken? GetTable(int tag)
		{
			var table = _tables[tag];
			return table == unused ? null : (MetadataToken?)table;
		}
    }
}
