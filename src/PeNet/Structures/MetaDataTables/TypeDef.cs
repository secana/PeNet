using System;
using System.Collections.Generic;
using System.Text;

namespace PeNet.Structures.MetaDataTables
{
    public class TypeDef : AbstractTable
    {
        public TypeDef(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Flags = ReadSize(4);
            Name = ReadSize(HeapSizes.String);
            Namespace = ReadSize(HeapSizes.String);
            Extends = ReadSize(IndexSizes[Index.TypeDefOrRef]);
            FieldList = ReadSize(IndexSizes[Index.Field]);
            MethodList = ReadSize(IndexSizes[Index.MethodDef]);
        }

        public uint Flags {get;}

        public uint Name {get;}

        public uint Namespace {get;}

        public uint Extends {get;}

        public uint FieldList {get;}

        public uint MethodList {get;}
    }
}
