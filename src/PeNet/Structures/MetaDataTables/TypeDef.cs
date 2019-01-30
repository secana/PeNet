using System;
using System.Collections.Generic;
using System.Text;
using PeNet.Test.Structures;

namespace PeNet.Structures.MetaDataTables
{
    public class TypeDef : AbstractTable
    {
        public TypeDef(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Flags = ReadSize(4);
            TypeName = ReadSize(HeapSizes.String);
            TypeNamespace = ReadSize(HeapSizes.String);
            Extends = ReadSize(IndexSizes[Index.TypeDefOrRef]);
            FieldList = ReadSize(IndexSizes[Index.Field]);
            MethodList = ReadSize(IndexSizes[Index.MethodDef]);
        }

        public uint Flags {get;}

        public uint TypeName {get;}

        public uint TypeNamespace {get;}

        public uint Extends {get;}

        public uint FieldList {get;}

        public uint MethodList {get;}
    }
}
