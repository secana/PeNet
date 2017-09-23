using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The IMAGE_DATA_DIRECTORY struct represents the data directory,
    /// </summary>
    public class IMAGE_DATA_DIRECTORY : AbstractStructure
    {
        /// <summary>
        ///     Create a new IMAGE_DATA_DIRECTORY object.
        /// </summary>
        /// <param name="buff">PE binary as byte array.</param>
        /// <param name="offset">Raw offset to the data directory in the binary.</param>
        public IMAGE_DATA_DIRECTORY(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        /// <summary>
        ///     RVA of the table.
        /// </summary>
        public uint VirtualAddress
        {
            get { return Buff.BytesToUInt32(Offset); }
            set { Buff.SetUInt32(Offset, value); }
        }

        /// <summary>
        ///     Table size in bytes.
        /// </summary>
        public uint Size
        {
            get { return Buff.BytesToUInt32(Offset + 0x4); }
            set { Buff.SetUInt32(Offset + 0x4, value); }
        }

        /// <summary>
        ///     Convert all object properties to strings.
        /// </summary>
        /// <returns>String representation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_DATA_DIRECTORY\n");
            sb.Append(this.PropertiesToString("{0,-10}:\t{1,10:X}\n"));

            return sb.ToString();
        }
    }
}