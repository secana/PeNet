using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    public interface IMETADATASTREAM_US
    {
        /// <summary>
        /// List with strings in the Meta Data stream "US".
        /// </summary>
        List<string> UserStrings { get; }

        /// <summary>
        /// List with strings and their index in the Meta Data stream "US".
        /// </summary>
        List<Tuple<string, uint>> UserStringsAndIndices { get; }

        /// <summary>
        /// Return the user string at the index from the stream.
        /// </summary>
        /// <param name="index">Index of the user string to return.</param>
        /// <returns>User string at the position index.</returns>
        string GetUserStringAtIndex(uint index);

        /// <summary>
        ///     Creates a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>Optional header properties as a string.</returns>
        string ToString();
    }

    /// <summary>
    /// Represents the "US" (user string) meta data stream from the .Net header which 
    /// contains all application interal strings.
    /// </summary>
    /// <inheritdoc cref="IMETADATASTREAM_US" />
    public class METADATASTREAM_US : AbstractStructure, IMETADATASTREAM_US
    {
        private uint _size;
        public List<string> UserStrings { get; }
        public List<Tuple<string, uint>> UserStringsAndIndices { get; }

        public METADATASTREAM_US(byte[] buff, uint offset, uint size) 
            : base(buff, offset)
        {
            _size = size;
            UserStringsAndIndices = ParseUserStringsAndIndices();
            UserStrings = UserStringsAndIndices.Select(x => x.Item1).ToList();

        }

        public string GetUserStringAtIndex(uint index)
        {
            return UserStringsAndIndices.FirstOrDefault(x => x.Item2 == index)?.Item1;
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