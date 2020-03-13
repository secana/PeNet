using System;
using System.Collections.Generic;
using System.Linq;
using PeNet.FileParser;

namespace PeNet.Header.Net
{
    /// <summary>
    /// Represents the "US" (user string) meta data stream from the .Net header which 
    /// contains all application internal strings.
    /// </summary>
    public class MetaDataStreamUs : AbstractStructure
    {
        private readonly uint _size;

        /// <summary>
        /// List with strings in the Meta Data stream "US".
        /// </summary>
        public List<string> UserStrings { get; }

        /// <summary>
        /// List with strings and their index in the Meta Data stream "US".
        /// </summary>
        public List<Tuple<string, uint>> UserStringsAndIndices { get; }

        public MetaDataStreamUs(IRawFile peFile, long offset, uint size) 
            : base(peFile, offset)
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
        public string? GetUserStringAtIndex(uint index)
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
                if (PeFile.ReadByte(i) >= 0x80) // Not sure why this works but it does.
                    i++;

                int length = PeFile.ReadByte(i);

                if (length == 0)                                        // Stop if a string has the length 0 since the end 
                    break;                                              // of the list is reached.

                i += 1;                                                 // Add "length byte" to current offset.
                var tmpString = PeFile.ReadUnicodeString(i);               // Read the UTF-16 string
                i += (uint)length - 1;                                  // Add the string length to the current offset.

                stringsAndIncides.Add(new Tuple<string, uint>(tmpString, (uint) i - (uint) length - (uint) Offset));
            }

            return stringsAndIncides;
        }
    }
}