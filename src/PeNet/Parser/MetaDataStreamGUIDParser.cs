using System;
using System.Collections.Generic;
using PeNet.Utilities;

namespace PeNet.Parser
{
    internal class MetaDataStreamGUIDParser : SafeParser<List<string>>
    {
        private readonly uint _size;

        public MetaDataStreamGUIDParser(
            byte[] buff,
            uint offset,
            uint size
        )
            : base(buff, offset)

        {
            _size = size;
        }

        protected override List<string> ParseTarget()
        {
            // A GUID is an 128 bit (16 bytes) long identifier
            var numOfGUIDs = _size / 16;
            var GUIDs = new List<string>((int) numOfGUIDs);

            for (var i = _offset; i < _offset + _size; i += 16)
            {
                var bytes = new byte[16];
                Array.Copy(_buff, i, bytes, 0, 16);

                GUIDs.Add(bytes.ToHexString());
            }

            return GUIDs;
        }
    }
}