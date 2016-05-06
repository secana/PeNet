using System.Text;

namespace PeNet.Structures
{
    /// <summary>
    /// The IMAGE_RESOURCE_DATA_ENTRY points to the data of
    /// the resources in the PE file like version info, strings etc.
    /// </summary>
    public class IMAGE_RESOURCE_DATA_ENTRY
    {
        private readonly byte[] _buff;
        private readonly uint _offset;

        /// <summary>
        /// Construct a IMAGE_RESOURCE_DATA_ENTRY at a given offset.
        /// </summary>
        /// <param name="buff">PE file as a byte array.</param>
        /// <param name="offset">Offset to the structure in the file.</param>
        public IMAGE_RESOURCE_DATA_ENTRY(byte[] buff, uint offset)
        {
            _buff = buff;
            _offset = offset;
        }

        /// <summary>
        /// Offset to the data of the resource.
        /// </summary>
        public uint OffsetToData
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        /// <summary>
        /// Size of the resource data.
        /// </summary>
        public uint Size1
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value, _offset + 0x4, _buff); }
        }

        /// <summary>
        /// Code Page
        /// </summary>
        public uint CodePage
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x8); }
            set { Utility.SetUInt32(value, _offset + 0x8, _buff); }
        }

        /// <summary>
        /// Reserved
        /// </summary>
        public uint Reserved
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0xC); }
            set { Utility.SetUInt32(value, _offset + 0xC, _buff); }
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_RESOURCE_DATA_ENTRY\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
