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
    }
}
