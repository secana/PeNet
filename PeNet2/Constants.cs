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
    /// <summary>
    /// This class contains constants and flags which are used in a PE file.
    /// The constants can be used to map a numeric value to an understanable string.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The SectionFlags enumeration lists all possible flags which can 
        /// be set in the section characteristics.
        /// </summary>
        [Flags]
        public enum SectionFlags : uint
        {
            /// <summary>
            /// Reserved.
            /// </summary>
            IMAGE_SCN_TYPE_NO_PAD = 0x00000008,
            /// <summary>
            /// Section contains code.
            /// </summary>
            IMAGE_SCN_CNT_CODE = 0x00000020,
            /// <summary>
            /// Section contains initialized data.
            /// </summary>
            IMAGE_SCN_CNT_INITIALIZED_DATA = 0x00000040,
            /// <summary>
            /// Section contains uninitialized data.
            /// </summary>
            IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x00000080,
            /// <summary>
            /// Reserved.
            /// </summary>
            IMAGE_SCN_LNK_OTHER = 0x00000100,
            /// <summary>
            /// Section contains comments or some  other type of information.
            /// </summary>
            IMAGE_SCN_LNK_INFO = 0x00000200,
            /// <summary>
            /// Section contents will not become part of image.
            /// </summary>
            IMAGE_SCN_LNK_REMOVE = 0x00000800,
            /// <summary>
            /// Section contents comdat.
            /// </summary>
            IMAGE_SCN_LNK_COMDAT = 0x00001000,
            /// <summary>
            /// Reset speculative exceptions handling bits in the TLB entries for this section.
            /// </summary>
            IMAGE_SCN_NO_DEFER_SPEC_EXC = 0x00004000,
            /// <summary>
            /// Section content can be accessed relative to GP.
            /// </summary>
            IMAGE_SCN_GPREL = 0x00008000,
            /// <summary>
            /// Unknown.
            /// </summary>
            IMAGE_SCN_MEM_FARDATA = 0x00008000,
            /// <summary>
            /// Unknown.
            /// </summary>
            IMAGE_SCN_MEM_PURGEABLE = 0x00020000,
            /// <summary>
            /// Unknown.
            /// </summary>
            IMAGE_SCN_MEM_16BIT = 0x00020000,
            /// <summary>
            /// Unknown.
            /// </summary>
            IMAGE_SCN_MEM_LOCKED = 0x00040000,
            /// <summary>
            /// Unknown.
            /// </summary>
            IMAGE_SCN_MEM_PRELOAD = 0x00080000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_1BYTES = 0x00100000, 
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_2BYTES = 0x00200000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_4BYTES = 0x00300000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_8BYTES = 0x00400000,
            /// <summary>
            /// Default alignment if no others are specified.
            /// </summary>
            IMAGE_SCN_ALIGN_16BYTES = 0x00500000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_32BYTES = 0x00600000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_64BYTES = 0x00700000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_128BYTES = 0x00800000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_256BYTES = 0x00900000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_512BYTES = 0x00A00000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_1024BYTES = 0x00B00000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_2048BYTES = 0x00C00000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_4096BYTES = 0x00D00000,
            /// <summary>
            /// Section alignment.
            /// </summary>
            IMAGE_SCN_ALIGN_8192BYTES = 0x00E00000,
            /// <summary>
            /// Alingment mask.
            /// </summary>
            IMAGE_SCN_ALIGN_MASK = 0x00F00000,
            /// <summary>
            /// Section contains extended relocations.
            /// </summary>
            IMAGE_SCN_LNK_NRELOC_OVFL = 0x01000000,
            /// <summary>
            /// Section can be discarded.
            /// </summary>
            IMAGE_SCN_MEM_DISCARDABLE = 0x02000000,
            /// <summary>
            /// Section is not cache-able.
            /// </summary>
            IMAGE_SCN_MEM_NOT_CACHED = 0x04000000,
            /// <summary>
            /// Section is not page-able.
            /// </summary>
            IMAGE_SCN_MEM_NOT_PAGED = 0x08000000,
            /// <summary>
            /// Section is shareable.
            /// </summary>
            IMAGE_SCN_MEM_SHARED = 0x10000000,
            /// <summary>
            /// Section is executable.
            /// </summary>
            IMAGE_SCN_MEM_EXECUTE = 0x20000000,
            /// <summary>
            /// Section is readable.
            /// </summary>
            IMAGE_SCN_MEM_READ = 0x40000000,
            /// <summary>
            /// Section is write-able.
            /// </summary>
            IMAGE_SCN_MEM_WRITE = 0x80000000
        }


        ////////////////////////
        // IMAGE_DATA_DIRECTORY
        ////////////////////////

        // Different data directories indices
        public enum DataDirectoryIndex
        {
            Export = 0, // Export directory
            Import = 1, // Import directory
            Resource = 2, // Resource directory
            Exception = 3,
            Security = 4,
            BaseReloc = 5,
            Debug = 6,
            Copyright = 7, // Useless
            Globalptr = 8, // Only interesting for Itanium systems
            TLS = 9,
            LoadConfig = 0xA,
            BoundImport = 0xB,
            IAT = 0xC,
            DelayImport = 0xD,
            COM_Descriptor = 0xE, // .Net Header
            Reserved = 0xF
        }


        //////////////////////////////////
        // IMAGE_RESOURCE_DIRECTORY_ENTRY
        //////////////////////////////////

        // Group IDs
        public enum ResourceGroupIDs : uint
        {
            Cursor = 1,
            Bitmap = 2,
            Icon = 3,
            Menu = 4,
            Dialog = 5,
            String = 6,
            FontDirectory = 7,
            Fonst = 8,
            Accelerator = 9,
            RcData = 10,
            MessageTable = 11,
            GroupIcon = 14,
            Version = 16,
            DlgInclude = 17,
            PlugAndPlay = 19,
            VXD = 20,
            AnimatedCurser = 21,
            AnimatedIcon = 22,
            HTML = 23,
            Manifest = 24
        }

        /////////////////
        // UNWINDE_CODE
        /////////////////

        // UnwindOp Codes
        public enum UnwindOpCodes : byte
        {
            UWOP_PUSH_NONVOL = 0,
            UWOP_ALLOC_LARGE = 1,
            UWOP_ALLOC_SMALL = 2,
            UWOP_SET_FPREG = 3,
            UWOP_SAVE_NONVOL = 4,
            UWOP_SAVE_NONVOL_FAR = 5,
            UWOP_SAVE_XMM128 = 8,
            UWOP_SAVE_XMM128_FAR = 9,
            UWOP_PUSH_MACHFRAME = 10
        }

        //////////////////////////////
        // IMAGE_FILE_HEADER constants
        //////////////////////////////

        // Machine
        public const ushort IMAGE_FILE_MACHINE_UNKNOWN = 0x0;
        public const ushort IMAGE_FILE_MACHINE_I386 = 0x14c;
        public const ushort IMAGE_FILE_MACHINE_R3000 = 0x162;
        public const ushort IMAGE_FILE_MACHINE_R4000 = 0x166;
        public const ushort IMAGE_FILE_MACHINE_R10000 = 0x168;
        public const ushort IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169;
        public const ushort IMAGE_FILE_MACHINE_ALPHA = 0x184;
        public const ushort IMAGE_FILE_MACHINE_SH3 = 0x1a2;
        public const ushort IMAGE_FILE_MACHINE_SH3DSP = 0x1a3;
        public const ushort IMAGE_FILE_MACHINE_SH3E = 0x1a4;
        public const ushort IMAGE_FILE_MACHINE_SH4 = 0x1a6;
        public const ushort IMAGE_FILE_MACHINE_SH5 = 0x1a8;
        public const ushort IMAGE_FILE_MACHINE_ARM = 0x1c0;
        public const ushort IMAGE_FILE_MACHINE_THUMB = 0x1c2;
        public const ushort IMAGE_FILE_MACHINE_AM33 = 0x1d3;
        public const ushort IMAGE_FILE_MACHINE_POWERPC = 0x1f0;
        public const ushort IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1;
        public const ushort IMAGE_FILE_MACHINE_IA64 = 0x200;
        public const ushort IMAGE_FILE_MACHINE_MIPS16 = 0x266;
        public const ushort IMAGE_FILE_MACHINE_M68K = 0x268;
        public const ushort IMAGE_FILE_MACHINE_ALPHA64 = 0x284;
        public const ushort IMAGE_FILE_MACHINE_MIPSFPU = 0x366;
        public const ushort IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466;
        public const ushort IMAGE_FILE_MACHINE_AXP64 = IMAGE_FILE_MACHINE_ALPHA64;
        public const ushort IMAGE_FILE_MACHINE_TRICORE = 0x520;
        public const ushort IMAGE_FILE_MACHINE_CEF = 0xcef;
        public const ushort IMAGE_FILE_MACHINE_EBC = 0xebc;
        public const ushort IMAGE_FILE_MACHINE_AMD64 = 0x8664;
        public const ushort IMAGE_FILE_MACHINE_M32R = 0x9041;
        public const ushort IMAGE_FILE_MACHINE_CEE = 0xc0ee;

        // Characteristics
        public const ushort IMAGE_FILE_RELOCS_STRIPPED = 0x01;
        public const ushort IMAGE_FILE_EXECUTABLE_IMAGE = 0x02;
        public const ushort IMAGE_FILE_LINE_NUMS_STRIPPED = 0x04;
        public const ushort IMAGE_FILE_LOCAL_SYMS_STRIPPED = 0x08;
        public const ushort IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x20;
        public const ushort IMAGE_FILE_32BIT_MACHINE = 0x100;
        public const ushort IMAGE_FILE_DEBUG_STRIPPED = 0x200;
        public const ushort IMAGE_FILE_DLL = 0x2000;

        //////////////////////////
        // IMAGE_OPTIONAL_HEADER
        //////////////////////////

        // Magic
        public const ushort IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b;
        public const ushort IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b;

        // Subsystem
        public const ushort IMAGE_SUBSYSTEM_NATIVE = 0x01; // driver
        public const ushort IMAGE_SUBSYSTEM_WINDOWS_GUI = 0x02;
        public const ushort IMAGE_SUBSYSTEM_WINDOWS_CUI = 0x03; // console

        // DllCharacteristics
        public const ushort IMAGE_DLLCHARACTERISTICS_DYNAMIC_BASE = 0x40; // ASLR
        public const ushort IMAGE_DLLCHARACTERISTICS_NX_COMPAT = 0x100; // DEP
        public const ushort IMAGE_DLLCHARACTERISTICS_NO_SEH = 0x400;
        public const ushort IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = 0x800;

        ////////////////////////
        // IMAGE_SECTION_HEADER
        ////////////////////////

        // Characteristics
        public const uint IMAGE_SCN_CNT_CODE = 0x20;
        public const uint IMAGE_SCN_CNT_INITIALIZED_DATA = 0x40;
        public const uint IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x80;
        public const uint IMAGE_SCN_MEM_DISCARDABLE = 0x02000000;
        public const uint IMAGE_SCN_MEM_SHARED = 0x10000000;
        public const uint IMAGE_SCN_MEM_EXECUTE = 0x20000000;
        public const uint IMAGE_SCN_MEM_READ = 0x40000000;
        public const uint IMAGE_SCN_MEM_WRITE = 0x80000000;

        //////////////////////////////////////
        // WIN_CERTIFICATE wCertificateType
        //////////////////////////////////////

        public const ushort WIN_CERT_TYPE_X509 = 0x0001;
        public const ushort WIN_CERT_TYPE_PKCS_SIGNED_DATA = 0x0002;
        public const ushort WIN_CERT_TYPE_RESERVED_1 = 0x0003;
        public const ushort WIN_CERT_TYPE_PKCS1_SIGN = 0x0009;
    }
}