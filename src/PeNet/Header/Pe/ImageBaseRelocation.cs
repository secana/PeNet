using System;
using System.Collections.Generic;
using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The ImageBaseRelocation structure holds information needed to relocate
    ///     the image to another virtual address.
    /// </summary>
    public class ImageBaseRelocation : AbstractStructure
    {
        /// <summary>
        ///     Create a new ImageBaseRelocation object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset to the relocation struct in the PE file.</param>
        /// <param name="relocSize">Size of the complete relocation directory.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     If the SizeOfBlock is bigger than the size
        ///     of the Relocation Directory.
        /// </exception>
        public ImageBaseRelocation(IRawFile peFile, long offset, uint relocSize)
            : base(peFile, offset)
        {
            if (SizeOfBlock > relocSize)
                throw new ArgumentOutOfRangeException(nameof(relocSize),
                    "SizeOfBlock cannot be bigger than size of the Relocation Directory.");

            if(SizeOfBlock < 8)
                throw new Exception("SizeOfBlock cannot be smaller than 8.");

            ParseTypeOffsets();
        }

        /// <summary>
        ///     RVA of the relocation block.>	PeNet.dll!PeNet.Structures.ImageBaseRelocation.ImageBaseRelocation(PeNet.IRawFile peFile, long offset, uint relocSize) Line 26	C#

        /// </summary>
        public uint VirtualAddress
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     SizeOfBlock-8 indicates how many TypeOffsets follow the SizeOfBlock.
        /// </summary>
        public uint SizeOfBlock
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Array with the TypeOffsets for the relocation block.
        /// </summary>
        public TypeOffset[]? TypeOffsets { get; private set; }

        private void ParseTypeOffsets()
        {
            var list = new List<TypeOffset>();
            for (uint i = 0; i < (SizeOfBlock - 8)/2; i++)
            {
                list.Add(new TypeOffset(PeFile, Offset + 8 + i*2));
            }
            TypeOffsets = list.ToArray();
        }

        /// <summary>
        ///     Represents the type and offset in an
        ///     ImageBaseRelocation structure.
        /// </summary>
        public class TypeOffset
        {
            private readonly IRawFile _peFile;
            private readonly long _offset;

            /// <summary>
            ///     Create a new TypeOffset object.
            /// </summary>
            /// <param name="peFile">A PE file.</param>
            /// <param name="offset">Offset of the TypeOffset in the PE file.</param>
            public TypeOffset(IRawFile peFile, long offset)
            {
                _peFile = peFile;
                _offset = offset;
            }

            /// <summary>
            ///     The type is described in the 4 lower bits of the
            ///     TypeOffset word.
            /// </summary>
            public byte Type
            {
                get
                {
                    var to = _peFile.ReadUShort(_offset);
                    return (byte) (to >> 12);
                }
            }

            /// <summary>
            ///     The offset is described in the 12 higher bits of the
            ///     TypeOffset word.
            /// </summary>
            public ushort Offset
            {
                get
                {
                    var to = _peFile.ReadUShort(_offset);
                    return (ushort) (to & 0xFFF);
                }
            }
        }
    }
}