/***********************************************************************
Copyright 2016 Stefan Hausotte

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*************************************************************************/

using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    /// <summary>
    /// TypeDef Table row in the Meta Data Tables Header of
    /// the .Net header.
    /// </summary>
    public class TypeRefTableRow : AbstractMetaDataTableRow
    {
        private readonly HeapOffsetBasedIndexSizes _heapOffsetBasedIndexSizes;

        /// <summary>
        /// Create a new TypeRef Table Row instance.
        /// </summary>
        /// <param name="buff">Buffer containing the row.</param>
        /// <param name="offset">Offset in the buffer where the row starts.</param>
        /// <param name="heapOffsetBasedIndexSizes">Computes sizes of the heap bases indexes.</param>
        public TypeRefTableRow(byte[] buff, uint offset, HeapOffsetBasedIndexSizes heapOffsetBasedIndexSizes) 
            : base(buff, offset)
        {
            _heapOffsetBasedIndexSizes = heapOffsetBasedIndexSizes;
        }

        //public uint ResolutionScope => Buff.BytesToUInt32(DYNAMICALLY COMPUTED INDEX);

        //public uint 

        /// <summary>
        /// Length of the row in bytes.
        /// </summary>
        public override uint Length { get; }
    }
}