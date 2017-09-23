namespace PeNet.Utilities
{
    /// <summary>
    /// Computes the index sizes of #String, #GUID and #Blob
    /// based on the HeapOffsetSizes value in the Meta Data Tables Header.
    /// </summary>
    public class HeapOffsetBasedIndexSizes
    {
        private readonly byte _heapOffsetSizes;

        /// <summary>
        /// Size of the #String index (4 or 2 bytes).
        /// </summary>
        public uint StringIndexSize => (uint) ((_heapOffsetSizes & 0x1) == 0x1 ? 4 : 2);

        /// <summary>
        /// Size of the #GUID index (4 or 2 bytes).
        /// </summary>
        public uint GuidIndexSize => (uint) (((_heapOffsetSizes >> 1) & 0x1) == 0x1 ? 4 : 2);

        /// <summary>
        /// Size of the #Blob index (4 or 2 bytes).
        /// </summary>
        public uint BlobSize => (uint) (((_heapOffsetSizes >> 2) & 0x1) == 0x1 ? 4 : 2);

        /// <summary>
        /// Create a new HeapOffsetBasedIndexSizes instance based
        /// on the HeapOffsetSizes byte from the Meta Data Tables Header.
        /// </summary>
        /// <param name="heapOffsetSizes"></param>
        public HeapOffsetBasedIndexSizes(byte heapOffsetSizes)
        {
            _heapOffsetSizes = heapOffsetSizes;
        }
    }
}