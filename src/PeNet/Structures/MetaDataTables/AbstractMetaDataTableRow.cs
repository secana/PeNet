using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    /// <summary>
    /// Abstract Meta Data Table Row.
    /// </summary>
    public abstract class AbstractMetaDataTableRow : AbstractStructure
    {
        /// <summary>
        /// The index sizes of the .Net heaps (streams). 
        /// </summary>
        public IHeapOffsetBasedIndexSizes HeapIndexSizes { get; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="buff">Buffer containing the row.</param>
        /// <param name="offset">Offset in the buffer where the row starts.</param>
        /// <param name="heapIndexSizes">The index sizes of the .Net heaps (streams).</param>
        protected AbstractMetaDataTableRow(byte[] buff, uint offset, IHeapOffsetBasedIndexSizes heapIndexSizes) 
            : base(buff, offset)
        {
            HeapIndexSizes = heapIndexSizes;
        }

        /// <summary>
        /// Length of the row in bytes.
        /// </summary>
        public abstract uint Length { get; }
    }
}