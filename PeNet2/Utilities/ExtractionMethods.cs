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

using System;
using System.Collections.Generic;
using System.Text;

namespace PeNet.Utilities
{
    /// <summary>
    /// Different methods to extract information from the PE binary.
    /// </summary>
    public static class ExtractionMethods
    {
        internal static ushort GetOrdinal(uint ordinal, byte[] buff)
        {
            return BitConverter.ToUInt16(new[] {buff[ordinal], buff[ordinal + 1]}, 0);
        }

        /// <summary>
        ///     Get a name (C string) at a specific position in a buffer.
        /// </summary>
        /// <param name="stringOffset">Offset of the string.</param>
        /// <param name="buff">Containing buffer.</param>
        /// <returns>The parsed C string.</returns>
        public static string GetCString(ulong stringOffset, byte[] buff)
        {
            var length = GetCStringLength(stringOffset, buff);
            var tmp = new char[length];
            for (ulong i = 0; i < length; i++)
            {
                tmp[i] = (char) buff[stringOffset + i];
            }

            return new string(tmp);
        }

        /// <summary>
        ///     For a given offset in an byte array, find the next
        ///     null value which terminates a C string.
        /// </summary>
        /// <param name="stringOffset">Offset of the string.</param>
        /// <param name="buff">Buffer which contains the string.</param>
        /// <returns>Length of the string in bytes.</returns>
        public static ulong GetCStringLength(ulong stringOffset, byte[] buff)
        {
            var offset = stringOffset;
            ulong length = 0;
            while (buff[offset] != 0x00)
            {
                length++;
                offset++;
            }
            return length;
        }

        // TODO: Improve parser. Currently it parses only ASCII characters.
        /// <summary>
        ///     Get a unicode string at a specific position in a buffer.
        /// </summary>
        /// <param name="stringOffset">Offset of the string.</param>
        /// <param name="buff">Containing buffer.</param>
        /// <returns>The parsed unicode string.</returns>
        public static string GetUnicodeString(ulong stringOffset, byte[] buff)
        {
            var charList = new List<byte>();

            for (var i = stringOffset; i < (ulong) buff.Length - 1; i++)
            {
                var highByte = buff[i +  1];
                var lowByte = buff[i];

                if(highByte != 0x00)
                    continue;

                if (highByte == 0x00 && lowByte == 0x00) // End of string.
                    break;

                charList.Add(lowByte);
            }

            return Encoding.ASCII.GetString(charList.ToArray());
        }
    }
}