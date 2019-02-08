namespace PeNet.Structures.MetaDataTables
{
    public class AssemblyRefProcessor : AbstractTable
    {
        public AssemblyRefProcessor(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
        {
            Processor = ReadSize(4);
            AssemblyRef = ReadSize(IndexSizes[Index.AssemblyRef]);
        }

        public uint Processor {get;}
        public uint AssemblyRef {get;}
    }
}
