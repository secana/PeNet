using System;
using System.Collections.Generic;
using System.Text;
using PeNet.Test.Structures;

namespace PeNet.Structures.MetaDataTables
{
    public class Method : AbstractTable
    {
        public Method(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            RVA = ReadSize(4, ref CurrentOffset);
            ImplFlags = (ushort) ReadSize(2, ref CurrentOffset);
            Flags = (ushort) ReadSize(2, ref CurrentOffset);
            Name = ReadSize(HeapSizes.String, ref CurrentOffset);
            Signature = ReadSize(HeapSizes.Blob, ref CurrentOffset);
            ParamList = ReadSize(IndexSizes[Index.Param], ref CurrentOffset);
        }

        public uint RVA {get;}
        public ushort ImplFlags {get;}
        public ushort Flags {get;}
        public uint Name {get;}
        public uint Signature {get;}
        public uint ParamList {get;}
    }
}
