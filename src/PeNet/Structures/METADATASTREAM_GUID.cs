using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    public interface IMETADATASTREAM_GUID
    {
        /// <summary>
        /// List with all GUIDs from the Meta Data stream "GUID".
        /// </summary>
        List<Guid> Guids { get; }

        /// <summary>
        /// List with all GUIDs and their index from the 
        /// Meta Data stream "GUID".
        /// </summary>
        List<Tuple<Guid, uint>> GuidsAndIndices { get; }

        /// <summary>
        /// Return the GUID at the index from the stream.
        /// </summary>
        /// <param name="index">Index of the GUID to return.</param>
        /// <returns>GUID at the position index. Null if not available.</returns>
        Guid? GetGuidAtIndex(uint index);

        /// <summary>
        ///     Creates a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>Optional header properties as a string.</returns>
        string ToString();
    }

    /// <summary>
    /// Represents the "GUID" meta data stream from the .Net header which 
    /// contains all application GUIDs to idenfitfy different assembly versions.
    /// </summary>
    /// <inheritdoc cref="IMETADATASTREAM_GUID" />
    public class METADATASTREAM_GUID : AbstractStructure, IMETADATASTREAM_GUID
    {
        private readonly uint _size;
        public List<Guid> Guids { get; }
        public List<Tuple<Guid, uint>> GuidsAndIndices { get; }

        public METADATASTREAM_GUID(byte[] buff, uint offset, uint size) 
            : base(buff, offset)
        {
            _size = size;
            GuidsAndIndices = ParseGuidsAndIndices();
            Guids = GuidsAndIndices.Select(x => x.Item1).ToList();
        }

        public Guid? GetGuidAtIndex(uint index)
        {
            return GuidsAndIndices.FirstOrDefault(x => x.Item2 == index)?.Item1;
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

                guidsAndIndicies.Add(new Tuple<Guid, uint>(new Guid(bytes), (uint) guidsAndIndicies.Count + 1));
            }

            return guidsAndIndicies;
        }
    }
}