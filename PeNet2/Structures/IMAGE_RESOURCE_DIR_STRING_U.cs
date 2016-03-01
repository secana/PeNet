using System;
using System.Text;

namespace PeNet.Structures
{
    /// <summary>
    ///     Represents a unicode string used for resource names
    ///     in the resource section.
    /// </summary>
    public class IMAGE_RESOURCE_DIR_STRING_U
    {
        private readonly byte[] _buff;
        private readonly uint _offset;

        /// <summary>
        ///     Create a new IMAGE_RESOURCE_DIR_STRING_U unicode string.
        /// </summary>
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset of the string.</param>
        public IMAGE_RESOURCE_DIR_STRING_U(byte[] buff, uint offset)
        {
            _buff = buff;
            _offset = offset;
        }

        /// <summary>
        ///     Length of the string in unicode characters, *not* in bytes.
        ///     1 unicode char = 2 bytes.
        /// </summary>
        public ushort Length
        {
            get { return Utility.BytesToUInt16(_buff, _offset); }
            set { Utility.SetUInt16(value, _offset, _buff); }
        }

        /// <summary>
        ///     The unicode string as a .Net string.
        /// </summary>
        public string NameString
        {
            get
            {
                var subarray = new byte[Length*2];
                Array.Copy(_buff, _offset + 2, subarray, 0, Length*2);

                return Encoding.Unicode.GetString(subarray);
            }
        }
    }
}