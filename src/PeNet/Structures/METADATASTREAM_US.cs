using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Represents the "US" (user string) meta data stream from the .Net header which 
    /// contains all application interal strings.
    /// </summary>
    /// <inheritdoc />
    public class METADATASTREAM_US : AbstractStructure
    {
        private uint _size;

        /// <summary>
        /// List with strings in the Meta Data stream "US".
        /// </summary>
        public List<string> UserStrings { get; }

        /// <summary>
        /// List with strings and their index in the Meta Data stream "US".
        /// </summary>
        public List<Tuple<string, uint>> UserStringsAndIndices { get; }

        /// <summary>
        /// Create a new METADATASTREAM_US that represents the Meta Data stream
        /// "String" from the .Net header.
        /// </summary>
        /// <param name="buff">PE file as a byte buffer.</param>
        /// <param name="offset">Offset of the "US" stream in the PE header.</param>
        /// <param name="size">Size of the "US" stream in bytes.</param>
        public METADATASTREAM_US(byte[] buff, uint offset, uint size) 
            : base(buff, offset)
        {
            _size = size;
            UserStringsAndIndices = ParseUserStringsAndIndices();
            UserStrings = UserStringsAndIndices.Select(x => x.Item1).ToList();

        }

        /// <summary>
        /// Return the user string at the index from the stream.
        /// </summary>
        /// <param name="index">Index of the user string to return.</param>
        /// <returns>User string at the position index.</returns>
        public string GetUserStringAtIndex(uint index)
        {
            return UserStringsAndIndices.FirstOrDefault(x => x.Item2 == index)?.Item1;
        }

        /// <summary>
        ///     Creates a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>Optional header properties as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("METADATASTREAM_US\n");
            sb.Append(this.PropertiesToString("{0,-15}:\t{1,10:X}\n"));
            foreach (var strings in UserStringsAndIndices)
                sb.Append($"{strings.Item2}\t{strings.Item1}");
            return sb.ToString();
        }

        private List<Tuple<string, uint>> ParseUserStringsAndIndices()
        {
            var stringsAndIncides = new List<Tuple<string, uint>>();

            // The #US stream starts with a "0x00" byte. That's why 
            // we skip the first byte in the buffer
            for (var i = Offset + 1; i < Offset + _size; i++)
            {
                if (Buff[i] >= 0x80) // Not sure why this works but it does.
                    i++;

                int length = Buff[i];

                if (length == 0)                                        // Stop if a string has the length 0 since the end 
                    break;                                              // of the list is reached.

                i += 1;                                                 // Add "length byte" to current offset.
                var tmpString = Buff.GetUnicodeString(i, length - 1);  // Read the UTF-16 string
                i += (uint)length - 1;                                 // Add the string length to the current offset.

                stringsAndIncides.Add(new Tuple<string, uint>(tmpString, i - (uint)length - Offset));
            }

            return stringsAndIncides;
        }
    }
}