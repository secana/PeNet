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
using PeNet.Structures;

namespace PeNet.Structures
{
    public class IMAGE_RESOURCE_DIRECTORY_ENTRY
    {
        private readonly byte[] _buff;
        private readonly uint _offset;

        public IMAGE_RESOURCE_DIRECTORY_ENTRY(byte[] buff, uint offset, uint resourceDirOffset)
        {
            _offset = offset;
            _buff = buff;

            // Resolve the Name
            try
            {
                if (IsIdEntry)
                {
                    ResolvedName = Utility.ResolveResourceId(ID);
                }
                else if (IsNamedEntry)
                {
                    var nameAddress = resourceDirOffset + (Name & 0x7FFFFFFF);
                    var unicodeName = new IMAGE_RESOURCE_DIR_STRING_U(_buff, nameAddress);
                    ResolvedName = unicodeName.NameString;
                }
            }
            catch (Exception)
            {
                ResolvedName = null;
            }
        }

        public uint Name
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        public string ResolvedName { get; private set; }

        public uint ID
        {
            get { return Name & 0xFFFF; }
            set { Name = value & 0xFFFF; }
        }

        public uint OffsetToData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value, _offset + 0x4, _buff); }
        }

        public uint OffsetToDirectory => OffsetToData & 0x7FFFFFFF;

        /// <summary>
        ///     True if the entry data is a directory
        /// </summary>
        public bool DataIsDirectory
        {
            get
            {
                if ((OffsetToData & 0x80000000) == 0x80000000)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///     True if the entry is a resource with a name.
        /// </summary>
        public bool IsNamedEntry
        {
            get
            {
                if ((Name & 0x80000000) == 0x80000000)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///     True if the entry is a resource with an ID instead of a name.
        /// </summary>
        public bool IsIdEntry => !IsNamedEntry;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_RESOURCE_DIRECTORY_ENTRY\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}