using System;
using System.Collections.Generic;
using System.Linq;
using PeNet.FileParser;

namespace PeNet.Header.Net
{
    /// <summary>
    /// Represents the "GUID" meta data stream from the .Net header which 
    /// contains all application GUIDs to idenfitfy different assembly versions.
    /// </summary>
    public class MetaDataStreamGuid : AbstractStructure
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

        public MetaDataStreamGuid(IRawFile peFile, long offset, uint size) 
            : base(peFile, offset)
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

        private List<Tuple<Guid, uint>> ParseGuidsAndIndices()
        {
            // A GUID is an 128 bit (16 bytes) long identifier
            var numOfGuiDs = _size / 16;
            var guidsAndIndicies = new List<Tuple<Guid, uint>>((int)numOfGuiDs);

            for (var i = Offset; i < Offset + _size; i += 16)
            {
                guidsAndIndicies.Add(new Tuple<Guid, uint>(new Guid(PeFile.AsSpan(i, 16)), (uint) guidsAndIndicies.Count + 1));
            }

            return guidsAndIndicies;
        }
    }
}