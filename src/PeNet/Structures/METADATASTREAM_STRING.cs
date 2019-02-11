using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    public interface IMETADATASTREAM_STRING
    {
        /// <summary>
        /// List with strings in the Meta Data stream "String".
        /// </summary>
        List<string> Strings { get; }

        /// <summary>
        /// List with strings and their index in the Meta Data stream "String".
        /// </summary>
        List<Tuple<string, uint>> StringsAndIndices { get; }

        /// <summary>
        /// Return the string at the index from the stream.
        /// </summary>
        /// <param name="index">Index of the string to return.</param>
        /// <returns>String at the position index.</returns>
        string GetStringAtIndex(uint index);

        /// <summary>
        ///     Creates a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>Optional header properties as a string.</returns>
        string ToString();
    }

    /// <inheritdoc cref="IMETADATASTREAM_STRING" />
    /// <summary>
    /// Represents the "String" meta data stream from the .Net header which 
    /// contains all application interal strings.
    /// </summary>
    public class METADATASTREAM_STRING : AbstractStructure, IMETADATASTREAM_STRING
    {
        private readonly uint _size;
        
        public List<string> Strings { get; }

        public List<Tuple<string, uint>> StringsAndIndices { get; }

        public METADATASTREAM_STRING(byte[] buff, uint offset, uint size) 
            : base(buff, offset)
        {
            _size = size;

            StringsAndIndices = ParseStringsAndIndices();
            Strings = StringsAndIndices.Select(x => x.Item1).ToList();
        }

        public string GetStringAtIndex(uint index)
        {
            return StringsAndIndices.FirstOrDefault(x => x.Item2 == index)?.Item1;
        }

        private List<Tuple<string, uint>> ParseStringsAndIndices()
        {
            var stringsAndIndices = new List<Tuple<string, uint>>();

            for (var i = Offset; i < Offset + _size; i++)
            {
                var index = i - Offset;
                var tmpString = Buff.GetCString(i);
                i += (uint)tmpString.Length;

                if (String.IsNullOrWhiteSpace(tmpString))
                    continue;

                stringsAndIndices.Add(new Tuple<string, uint>(tmpString, index));
            }

            return stringsAndIndices;
        }
    }
}