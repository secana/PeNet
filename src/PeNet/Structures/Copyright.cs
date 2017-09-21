using System.Text;
using ExtensionMethods = PeNet.Utilities.ExtensionMethods;

namespace PeNet.Structures
{
    /// <summary>
    ///     The copyright ASCII (not 0-terminated) string of the PE file
    ///     if any is given.
    /// </summary>
    public class Copyright : AbstractStructure
    {
        /// <summary>
        ///     Create a new copyright object.
        /// </summary>
        /// <param name="buff">PE binary as byte array.</param>
        /// <param name="offset">Offset to the copyright string in the binary.</param>
        /// <param name="size">Size of the copyright string.</param>
        public Copyright(byte[] buff, uint offset, uint size)
            : base(buff, offset)
        {
            CopyrightString = ParseCopyrightString(buff, offset, size);
        }

        /// <summary>
        ///     The copyright string.
        /// </summary>
        public string CopyrightString { get; private set; }

        private string ParseCopyrightString(byte[] buff, uint offset, uint size)
        {
            return Encoding.ASCII.GetString(buff, (int) offset, (int) size);
        }


        /// <summary>
        ///     Convert all object properties to strings.
        /// </summary>
        /// <returns>String representation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("Copyright\n");
            sb.Append(ExtensionMethods.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));

            return sb.ToString();
        }
    }
}