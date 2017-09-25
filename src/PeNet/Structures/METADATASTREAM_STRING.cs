using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Represents the "String" meta data stream from the .Net header which 
    /// contains all application interal strings.
    /// </summary>
    /// <inheritdoc />
    public class METADATASTREAM_STRING : AbstractStructure
    {
        private readonly uint _size;
        
        /// <summary>
        /// List with strings in the Meta Data stream "String".
        /// </summary>
        public List<string> Strings { get; }

        /// <summary>
        /// List with strings and their index in the Meta Data stream "String".
        /// </summary>
        public List<Tuple<string, uint>> StringsAndIndices { get; }

        /// <summary>
        /// Create a new METADATASTREAM_STRING that represents the Meta Data stream
        /// "String" from the .Net header.
        /// </summary>
        /// <param name="buff">PE file as a byte buffer.</param>
        /// <param name="offset">Offset of the "String" stream in the PE header.</param>
        /// <param name="size">Size of the "String" stream in bytes.</param>
        public METADATASTREAM_STRING(byte[] buff, uint offset, uint size) 
            : base(buff, offset)
        {
            _size = size;

            StringsAndIndices = ParseStringsAndIndices();
            Strings = StringsAndIndices.Select(x => x.Item1).ToList();
        }

        /// <summary>
        /// Return the string at the index from the stream.
        /// </summary>
        /// <param name="index">Index of the string to return.</param>
        /// <returns>String at the position index.</returns>
        public string GetStringAtIndex(uint index)
        {
            return StringsAndIndices.FirstOrDefault(x => x.Item2 == index)?.Item1;
        }


        /// <summary>
        ///     Creates a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>Optional header properties as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("METADATASTREAM_STRING\n");
            sb.Append(this.PropertiesToString("{0,-15}:\t{1,10:X}\n"));
            foreach (var strings in StringsAndIndices)
                sb.Append($"{strings.Item2}\t{strings.Item1}");
            return sb.ToString();
        }

        private List<Tuple<string, uint>> ParseStringsAndIndices()
        {
            var stringsAndIndices = new List<Tuple<string, uint>>();

            for (var i = Offset; i < Offset + _size; i++)
            {
                var tmpString = Buff.GetCString(i);
                i += (uint)tmpString.Length;

                if (String.IsNullOrWhiteSpace(tmpString))
                    continue;

                stringsAndIndices.Add(new Tuple<string, uint>(tmpString, i - Offset));
            }

            return stringsAndIndices;
        }
    }
}