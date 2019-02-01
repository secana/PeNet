using System.Collections.Generic;

namespace PeNet.Structures.MetaDataTables
{
    public class Tables
    {
        public List<Module> Module {get; set;}
        public List<TypeRef> TypeRef {get; set;}
        public List<TypeDef> TypeDef {get; set;}
        public List<Field> Field {get; set;}
        public List<MethodDef> MethodDef {get; set;}
        public List<Param> Param {get; set;}
        public List<InterfaceImpl> InterfaceImpl {get; set;}
        public List<MemberRef> MemberRef {get; set;}
        public List<Constant> Constant {get; set;}
        public List<CustomAttribute> CustomAttribute {get; set;}
        public List<FieldMarshal> FieldMarshal  {get; set;}
        public List<DeclSecurity> DeclSecurity {get; set;}
        public List<ClassLayout> ClassLayout {get; set;}
        public List<FieldLayout> FieldLayout {get; set;}
        public List<StandAloneSig> StandAloneSig {get; set;}
        public List<EventMap> EventMap {get; set;}
        public List<Event> Event {get; set;}
        public List<PropertyMap> PropertyMap {get; set;}
        public List<Property> Property {get; set;}
        public List<MethodSemantics> MethodSemantic {get; set;}
        public List<MethodImpl> MethodImpl {get; set;}
        public List<ModuleRef> ModuleRef {get; set;}
        public List<TypeSpec> TypeSpec {get; set;}
        public List<ImplMap> ImplMap {get; set;}
        public List<FieldRVA> FieldRVA {get; set;}
        public List<Assembly> Assembly {get; set;}
        public List<AssemblyProcessor> AssemblyProcessor {get; set;}
        public List<AssemblyOS> AssemblyOS {get; set;}
        public List<AssemblyRef> AssemblyRef {get; set;}
        public List<AssemblyRefProcessor> AssemblyRefProcessor {get; set;}
        public List<AssemblyRefOS> AssemblyRefOS {get; set;}
        public List<File> File {get; set;}
        public List<ExportedType> ExportedType {get; set;}
        public List<ManifestResource> ManifestResource {get; set;}
        public List<NestedClass> NestedClass {get; set;}
        public List<GenericParam> GenericParam {get; set;}
        public List<GenericParamConstraint> GenericParamConstraints {get; set;}
    }
}
