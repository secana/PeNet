using PeNet.Test.Structures;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    public class AbstractTable : AbstractStructure
    {
        protected HeapSizes HeapSizes {get;}
        protected IndexSize IndexSizes {get;}
        protected uint CurrentOffset;

        public AbstractTable(byte[] buff, uint offset, HeapSizes heapSizes, IndexSize indexSizes) 
            : base(buff, offset)
        {
            HeapSizes = heapSizes;
            IndexSizes = indexSizes;
            CurrentOffset = Offset;
        }

        protected uint ReadSize(uint size, ref uint offset)
        {
            if(size == 2)
            {
                offset += 2;
                return Buff.BytesToUInt16(offset - 2);
            }
            
            offset += 4;
            return Buff.BytesToUInt32(offset - 4);
        }
    }
}
