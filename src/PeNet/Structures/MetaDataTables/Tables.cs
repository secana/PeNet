using System.Collections.Generic;

namespace PeNet.Structures.MetaDataTables
{
    public class Tables
    {
        public List<Module> Module {get; set;}
        public List<TypeRef> TypeRef {get; set;}
        public List<TypeDef> TypeDef {get; set;}
        public List<Field> Field {get; set;}
    }
}
