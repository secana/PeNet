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
using System.Text;

namespace PeNet.Structures
{
    /// <summary>
    ///     The resouce directory contains icons, mouse pointer, string
    ///     language files etc. which are used by the application.
    /// </summary>
    public class IMAGE_RESOURCE_DIRECTORY : AbstractStructure
    {
        /// <summary>
        ///     Array with the different directory entries.
        /// </summary>
        public readonly IMAGE_RESOURCE_DIRECTORY_ENTRY[] DirectoryEntries;

        /// <summary>
        ///     Create a new IMAGE_RESOURCE_DIRECTORY object.
        /// </summary>
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset to the resource directory.</param>
        /// <param name="resourceDirOffset">Raw offset to the resource directory entries.</param>
        public IMAGE_RESOURCE_DIRECTORY(byte[] buff, uint offset, uint resourceDirOffset)
            : base(buff, offset)
        {
            DirectoryEntries = ParseDirectoryEntries(resourceDirOffset);
        }

        /// <summary>
        ///     Characteristics.
        /// </summary>
        public uint Characteristics
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
        ///     Major version.
        /// </summary>
        public ushort MajorVersion
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0x8); }
            set { Utility.SetUInt16(value, Offset + 0x8, Buff); }
        }

        /// <summary>
        ///     Minor version.
        /// </summary>
        public ushort MinorVersion
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0xa); }
            set { Utility.SetUInt16(value, Offset + 0xa, Buff); }
        }

        /// <summary>
        ///     Number of named entries.
        /// </summary>
        public ushort NumberOfNameEntries
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0xc); }
            set { Utility.SetUInt16(value, Offset + 0xc, Buff); }
        }

        /// <summary>
        ///     Number of ID entries.
        /// </summary>
        public ushort NumberOfIdEntries
        {
            get { return Utility.BytesToUInt16(Buff, Offset + 0xe); }
            set { Utility.SetUInt16(value, Offset + 0xe, Buff); }
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_RESOURCE_DIRECTORY\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
            if(DirectoryEntries != null)
                foreach (var de in DirectoryEntries)
                    sb.Append($"{de}");
            return sb.ToString();
        }

        private IMAGE_RESOURCE_DIRECTORY_ENTRY[] ParseDirectoryEntries(uint resourceDirOffset)
        {
            var entries = new IMAGE_RESOURCE_DIRECTORY_ENTRY[NumberOfIdEntries + NumberOfNameEntries];

            for (var index = 0; index < entries.Length; index++)
            {
                try
                {
                    entries[index] = new IMAGE_RESOURCE_DIRECTORY_ENTRY(Buff, (uint) index*8 + Offset + 16,
                        resourceDirOffset);
                }
                catch (IndexOutOfRangeException)
                {
                    entries[index] = null;
                }
            }

            return entries;
        }
    }
}