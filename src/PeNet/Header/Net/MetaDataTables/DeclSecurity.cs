using PeNet.FileParser;

namespace PeNet.Header.Net.MetaDataTables
{
    public class DeclSecurity : AbstractTable
    {
        public DeclSecurity(IRawFile peFile, long offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(peFile, offset, heapSizes, indexSizes)
        {
            Action = (ushort) ReadSize(2);
            Parent = ReadSize(IndexSizes[Index.HasDeclSecurity]);
            PermissionSet = ReadSize(HeapSizes.Blob);
        }

        public ushort Action {get;}
        public uint Parent {get;}
        public uint PermissionSet {get;}
    }
}
