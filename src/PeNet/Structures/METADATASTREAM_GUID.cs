using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// Represents the "GUID" meta data stream from the .Net header which 
    /// contains all application GUIDs to idenfitfy different assembly versions.
    /// </summary>
    /// <inheritdoc />
    public class METADATASTREAM_GUID : AbstractStructure
    {
        private readonly uint _size;

        /// <summary>
        /// List with all GUIDs from the Meta Data stream "GUID".
        /// </summary>
        public List<Guid> Guids { get; }

        /// <summary>
        /// List with all GUIDs and their index from the 
        /// Meta Data stream "GUID".
        /// </summary>
        public List<Tuple<Guid, uint>> GuidsAndIndices { get; }

        /// <summary>
        /// Create a new METADATASTREAM_GUID that represents the Meta Data stream
        /// "GUID" from the .Net header.
        /// </summary>
        /// <param name="buff">PE file as a byte buffer.</param>
        /// <param name="offset">Offset of the "GUID" stream in the PE header.</param>
        /// <param name="size">Size of the "GUID" stream in bytes.</param>
        public METADATASTREAM_GUID(byte[] buff, uint offset, uint size) 
            : base(buff, offset)
        {
            _size = size;
            GuidsAndIndices = ParseGuidsAndIndices();
            Guids = GuidsAndIndices.Select(x => x.Item1).ToList();
        }

        /// <summary>
        /// Return the GUID at the index from the stream.
        /// </summary>
        /// <param name="index">Index of the GUID to return.</param>
        /// <returns>GUID at the position index. Null if not available.</returns>
        public Guid? GetGuidAtIndex(uint index)
        {
            return GuidsAndIndices.FirstOrDefault(x => x.Item2 == index)?.Item1;
        }

        /// <summary>
        ///     Creates a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>Optional header properties as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("METADATASTREAM_GUID\n");
            sb.Append(this.PropertiesToString("{0,-15}:\t{1,10:X}\n"));
            foreach (var strings in GuidsAndIndices)
                sb.Append($"{strings.Item2}\t{strings.Item1}");
            return sb.ToString();
        }

        private List<Tuple<Guid, uint>> ParseGuidsAndIndices()
        {
            // A GUID is an 128 bit (16 bytes) long identifier
            var numOfGUIDs = _size / 16;
            var guidsAndIndicies = new List<Tuple<Guid, uint>>((int)numOfGUIDs);

            for (var i = Offset; i < Offset + _size; i += 16)
            {
                var bytes = new byte[16];
                Array.Copy(Buff, i, bytes, 0, 16);

                guidsAndIndicies.Add(new Tuple<Guid, uint>(new Guid(bytes), i - Offset));
            }

            return guidsAndIndicies;
        }
    }
}