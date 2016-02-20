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

namespace PeNet
{
    public class IMAGE_RESOURCE_DIRECTORY_ENTRY
    {
        byte[] _buff;
        UInt32 _offset;

        public UInt32 Name
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        public UInt32 ID
        {
            get { return Name & 0xFFFF; }
            set { Name = value & 0xFFFF; }
        }

        public UInt32 OffsetToData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value, _offset + 0x4, _buff); }
        }

        public UInt32 OffsetToDirectory
        {
            get { return OffsetToData & 0x7FFFFFFF; }
        }

        /// <summary>
        /// True if the entry data is a directory
        /// </summary>
        public bool DataIsDirectory
        {
            get
            {
                if ((OffsetToData & 0x80000000) == 0x80000000)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// True if the entry is a resource with a name.
        /// </summary>
        public bool IsNamedEntry
        {
            get
            {
                if ((Name & 0x80000000) == 0x80000000)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// True if the entry is a resource with an ID instead of a name.
        /// </summary>
        public bool IsIdEntry
        {
            get { return !IsNamedEntry; }
        }

        public IMAGE_RESOURCE_DIRECTORY_ENTRY(byte[] buff, UInt32 offset)
        {
            _offset = offset;
            _buff = buff;
        }
    }
}
