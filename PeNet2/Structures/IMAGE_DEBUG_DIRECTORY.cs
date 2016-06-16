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

using System.Text;

namespace PeNet.Structures
{
    /// <summary>
    ///     The IMAGE_DEBUG_DIRECTORY hold debug information
    ///     about the PE file.
    /// </summary>
    public class IMAGE_DEBUG_DIRECTORY : AbstractStructure
    {
        /// <summary>
        ///     Create a new IMAGE_DEBUG_DIRECTORY object.
        /// </summary>
        /// <param name="buff">PE binary as byte array.</param>
        /// <param name="offset">Offset to the debug struct in the binary.</param>
        public IMAGE_DEBUG_DIRECTORY(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        /// <summary>
        ///     Characteristics of the debug information.
        /// </summary>
        public uint Characteristics
        {
            get { return Utility.BytesToUInt32(Buff, Offset); }
            set { Utility.SetUInt32(value, Offset, Buff); }
        }

        /// <summary>
        ///     Time and date stamp
        /// </summary>
        public uint TimeDateStamp
        {
            get { return Utility.BytesToUInt32(Buff, Offset + 0x4); }
            set { Utility.SetUInt32(value, Offset + 0x4, Buff); }
        }

        /// <summary>
        ///     Major Version.
        /// </summary>
        public ushort MajorVersion
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x8); }
            set { Utility.SetUInt16(value, Offset + 0x8, Buff); }
        }

        /// <summary>
        ///     Minor Version.
        /// </summary>
        public ushort MinorVersion
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0xa); }
            set { Utility.SetUInt16(value, Offset + 0xa, Buff); }
        }

        /// <summary>
        ///     Type
        ///     1: Coff
        ///     2: CV-PDB
        ///     9: Borland
        /// </summary>
        public uint Type
        {
            get { return Utility.BytesToUInt32(Buff, Offset + 0xc); }
            set { Utility.SetUInt32(value, Offset + 0xc, Buff); }
        }

        /// <summary>
        ///     Size of data.
        /// </summary>
        public uint SizeOfData
        {
            get { return Utility.BytesToUInt32(Buff, Offset + 0x10); }
            set { Utility.SetUInt32(value, Offset + 0x10, Buff); }
        }

        /// <summary>
        ///     Address of raw data.
        /// </summary>
        public uint AddressOfRawData
        {
            get { return Utility.BytesToUInt32(Buff, Offset + 0x14); }
            set { Utility.SetUInt32(value, Offset + 0x14, Buff); }
        }

        /// <summary>
        ///     Pointer to raw data.
        /// </summary>
        public uint PointerToRawData
        {
            get { return Utility.BytesToUInt32(Buff, Offset + 0x18); }
            set { Utility.SetUInt32(value, Offset + 0x18, Buff); }
        }

        /// <summary>
        ///     Convert all object properties to strings.
        /// </summary>
        /// <returns>String representation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_DEBUG_DIRECTORY\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));

            return sb.ToString();
        }
    }
}