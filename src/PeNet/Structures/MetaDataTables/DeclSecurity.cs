namespace PeNet.Structures.MetaDataTables
{
    public class DeclSecurity : AbstractTable
    {
        public DeclSecurity(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset, heapSizes, indexSizes)
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
