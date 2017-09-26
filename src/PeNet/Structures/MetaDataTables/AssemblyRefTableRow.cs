using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    /// <summary>
    /// A row in the AssemblyRef Table of the Meta Data Tables Header
    /// in the .Net header.
    /// </summary>
    /// <inheritdoc />
    public class AssemblyRefTableRow : AbstractMetaDataTableRow
    {
        public override uint Length { get; }

        public AssemblyRefTableRow(
            byte[] buff, 
            uint offset,
            IHeapOffsetBasedIndexSizes heapOffsetSizes
            ) 
            : base(buff, offset, heapOffsetSizes)
        {
        }

        
    }
}