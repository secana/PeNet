namespace PeNet.Structures
{
    /// <summary>
    /// Size of the meta data heaps.
    /// </summary>
    public class HeapSizes
    {
        /// <summary>
        /// Size of the offsets into the "String" heap.
        /// </summary>
        public uint String {get;}

        /// <summary>
        /// Size of the offset into the "Guid" heap.
        /// </summary>
        public uint Guid {get;}

        /// <summary>
        /// Size of the offset into the "Blob" heap.
        /// </summary>
        public uint Blob {get;}

        /// <summary>
        /// Create a new HeapSizes instances.
        /// </summary>
        /// <param name="heapSizes">HeapSizes value from the METADATATABLESHDR.</param>
        public HeapSizes(byte heapSizes)
        {
            String = (heapSizes & 0x1) == 0 ? 2U: 4U;
            Guid = (heapSizes & 0x2) == 0 ? 2U: 4U;
            Blob = (heapSizes & 0x4) == 0 ? 2U: 4U;
        }
    }
}
