namespace PeNet.Structures.MetaDataTables
{
    /// <summary>
    /// Abstract Meta Data Table Row.
    /// </summary>
    public abstract class AbstractMetaDataTableRow : AbstractStructure
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="buff">Buffer containing the row.</param>
        /// <param name="offset">Offset in the buffer where the row starts.</param>
        protected AbstractMetaDataTableRow(byte[] buff, uint offset) 
            : base(buff, offset)
        {
        }

        /// <summary>
        /// Length of the row in bytes.
        /// </summary>
        public abstract uint Length { get; }
    }
}