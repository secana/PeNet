using System.Collections.Generic;
using PeNet.Utilities;
using static System.String;

namespace PeNet.Parser
{
    internal class MetaDataStreamUSParser : SafeParser<List<string>>
    {
        private readonly uint _size;

        public MetaDataStreamUSParser(byte[] buff, uint offset, uint size) 
            : base(buff, offset)
        {
            _size = size;
        }

        protected override List<string> ParseTarget()
        {
            var stringList = new List<string>();

            // The #US stream starts with a "0x00" byte. That's why 
            // we skip the first byte in the buffer
            for (var i = _offset + 1; i < _offset + _size; i++)
            {
                if (_buff[i] >= 0x80) // Not sure why this works but it does.
                    i++;

                int length = _buff[i]; 
             
                if (length == 0)                                        // Stop if a string has the length 0 since the end 
                    break;                                              // of the list is reached.

                i += 1;                                                 // Add "length byte" to current offset.
                var tmpString = _buff.GetUnicodeString(i, length - 1);  // Read the UTF-16 string
                i += (uint) length - 1;                                 // Add the string length to the current offset.

                stringList.Add(tmpString);
            }

            return stringList;
        }
    }
}