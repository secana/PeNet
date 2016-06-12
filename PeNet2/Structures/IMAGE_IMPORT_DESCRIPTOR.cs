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
    ///     The IMAGE_IMPORT_DESCRIPTORs are contained in the Import Directory
    ///     and holds all the information about function and symbol imports.
    /// </summary>
    public class IMAGE_IMPORT_DESCRIPTOR : AbstractStructure
    {
        /// <summary>
        ///     Create a new IMAGE_IMPORT_DESCRIPTOR object.
        /// </summary>
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset of the descriptor.</param>
        public IMAGE_IMPORT_DESCRIPTOR(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        /// <summary>
        ///     Points to the first IMAGE_IMPORT_BY_NAME struct.
        /// </summary>
        public uint OriginalFirstThunk
        {
            get { return Utility.BytesToUInt32(Buff, Offset); }
            set { Utility.SetUInt32(value, Offset, Buff); }
        }

        /// <summary>
        ///     Time and date stamp.
        /// </summary>
        public uint TimeDateStamp
        {
            get { return Utility.BytesToUInt32(Buff, Offset + 0x4); }
            set { Utility.SetUInt32(value, Offset + 0x4, Buff); }
        }

        /// <summary>
        ///     Forwarder Chain.
        /// </summary>
        public uint ForwarderChain
        {
            get { return Utility.BytesToUInt32(Buff, Offset + 0x8); }
            set { Utility.SetUInt32(value, Offset + 0x8, Buff); }
        }

        /// <summary>
        ///     RVA to the name of the DLL.
        /// </summary>
        public uint Name
        {
            get { return Utility.BytesToUInt32(Buff, Offset + 0xC); }
            set { Utility.SetUInt32(value, Offset + 0xC, Buff); }
        }

        /// <summary>
        ///     Points to an IMAGE_IMPORT_BY_NAME struct or
        ///     to the address of the first function.
        /// </summary>
        public uint FirstThunk
        {
            get { return Utility.BytesToUInt32(Buff, Offset + 0x10); }
            set { Utility.SetUInt32(value, Offset + 0x10, Buff); }
        }

        /// <summary>
        ///     Creates a string representation of the objects porperties.
        /// </summary>
        /// <returns>The import decscriptors properties as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_IMPORT_DESCRIPTOR\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}