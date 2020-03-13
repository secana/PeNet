using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class MethodDef : AbstractTable
    {
        public MethodDef(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            RVA = ReadSize(4);
            ImplFlags = (ushort) ReadSize(2);
            Flags = (ushort) ReadSize(2);
            Name = ReadSize(HeapSizes.String);
            Signature = ReadSize(HeapSizes.Blob);
            ParamList = ReadSize(IndexSizes[Index.Param]);
        }

        public uint RVA {get;}
        public ushort ImplFlags {get;}
        public ushort Flags {get;}
        public uint Name {get;}
        public uint Signature {get;}
        public uint ParamList {get;}
    }
}
