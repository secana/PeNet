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

namespace PeNet.UnitTest.Structures
{
    /// <summary>
    /// See for more information: 
    /// http://www.ntcore.com/files/dotnetformat.htm 
    /// http://www.codeproject.com/Articles/12585/The-NET-File-Format
    /// </summary>
    public static class RawDotNetStructures
    {
        public static byte[] RawImageCor20Header =
        {
            0x00, // Garbage to test offset
            0x11,
            0x22,

            0x00, // cb
            0x11,
            0x22,
            0x33,

            0x44, // MajorRuntimeVersion
            0x55, 

            0x66, // MinorRuntimeVersion
            0x77,

            0x88, // MetaData RVA
            0x99,
            0xaa,
            0xbb,
            0xcc, // MetaData Size
            0xdd,
            0xee,
            0xff,

            0x11, // Flags
            0x22,
            0x33,
            0x44,

            0x55, // EntryPointToken/RVA (Union)
            0x66,
            0x77,
            0x88,

            0x99, // Resources RVA
            0xaa,
            0xbb,
            0xcc,
            0xdd, // Resources Size
            0xee,
            0xff,
            0x11,

            0xaa, // StrongNameSignature RVA
            0xbb,
            0xcc,
            0xdd,
            0xee, // StrongNamesSignature Size
            0xff,
            0x11,
            0x22,

            0xbb, // CodeManagerTable RVA
            0xcc,
            0xdd,
            0xee,
            0xff, // CodeManagerTable Size
            0x11,
            0x22,
            0x33,

            0xcc, // VTableFixups RVA
            0xdd,
            0xee,
            0xff,
            0x11, // VTableFixups Size
            0x22,
            0x33,
            0x44,

            0xdd, // ExportAddressTableJumps RVA
            0xee,
            0xff,
            0x11,
            0x22, // ExportAddressTableJumps Size
            0x33,
            0x44,
            0x55,

            0xee, // ManagedNativeHeader RVA
            0xff,
            0x11,
            0x22,
            0x33, // ManagedNativeHeader Size
            0x44,
            0x55,
            0x66
        };

        public static byte[] RawMetaDataHeader =
        {
            0x00, // Garbage for offset test
            0x11,

            0x22, // Signature
            0x33,
            0x44,
            0x55,

            0x66, // MajorVersion
            0x77,

            0x88, // MinorVersion
            0x99,

            0xaa, // Reserved
            0xbb,
            0xcc,
            0xdd,

            0x0C, // VersionLength
            0x00, // = 12
            0x00,
            0x00,

            0x76, // Version
            0x34, // v4.0.30319
            0x2E,
            0x30,
            0x2E,
            0x33,
            0x30,
            0x33,
            0x31,
            0x39,
            0x00,
            0x00,

            0x11, // Flags
            0x22,

            0x02, // Streams (number of streams)
            0x00,

            // Stream Header 1
            0x6C,   // Offset
            0x00,
            0x00,
            0x00,

            0x04,  // Size
            0x18,
            0x00,
            0x00,

            0x23, // Stream name = #~
            0x7E, 
            0x00, // Padding
            0x00, 

            // Stream Header 2
            0x70, // Offset
            0x18,
            0x00,
            0x00,

            0x68, // Size
            0x14,
            0x00,
            0x00,

            0x23, // Stream name = #Strings
            0x53,
            0x74,
            0x72,
            0x69,
            0x6E,
            0x67,
            0x73, // Padding
            0x00,
            0x00,
            0x00,
            0x00
        };

        public static byte[] RawMetaDataDataTablesHdr =
        {
            0x00, // Garbage to test offset
            0x11,

            0x22, // Reserved1
            0x33,
            0x44,
            0x55,

            0x66, // MajorVersion

            0x77, // MinorVersion
            
            0x88, // HeapOffsetSizes

            0x99, // Reserved2

            0xbb, // MaskValid
            0xcc,
            0xdd,
            0xee,
            0xff,
            0x00,
            0x11,
            0x22,

            0x33, // MaskSorted
            0x44,
            0x55,
            0x66,
            0x77,
            0x88,
            0x99,
            0xaa
        };
    }
}