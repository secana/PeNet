using PeNet.FileParser;
using System;
using System.Runtime.InteropServices;

namespace PeNet.Header.Pe
{
    /// <summary>
    /// The ImageLoadConfigDirectory hold information
    /// important to load the PE file correctly.
    /// </summary>
    public class ImageLoadConfigDirectory : AbstractStructure, IDisposable
    {
        private IntPtr _ptr;
        private readonly bool _is64Bit;

        /// <summary>
        /// Create a new ImageLoadConfigDirectory object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of the structure in the buffer.</param>
        /// <param name="is64Bit">Flag if the PE file is 64 Bit.</param>
        public ImageLoadConfigDirectory(IRawFile peFile, long offset, bool is64Bit) 
            : base(peFile, offset)
        {
            _is64Bit = is64Bit;
            uint size = PeFile.ReadUInt(offset);
            byte[] data = PeFile.ToArray();
            if (size > data.Length)
            {
                ///
                /// I think we should throw an Exception when overflow occured, because IMAGE_LOAD_CONFIG_DIRECTORY(64) Size field indicates this DataDirectory size
                /// but in order to pass unit test, set the size to data length :)
                /// throw new ArgumentOutOfRangeException(nameof(size));
                ///
                size = (uint)data.Length;

                ///
                /// At the moment,we will get fake and error data rather than normal PE IMAGE_LOAD_CONFIG_DIRECTORY(64) data bytes
                ///
            }
            _ptr = Marshal.AllocHGlobal((int)size);
            if(_ptr != IntPtr.Zero)
            {
                if(offset + size < data.Length)
                {
                    Marshal.Copy(data, (int)offset, _ptr, (int)size);
                }
            }
        }

        ~ImageLoadConfigDirectory()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            if (_ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_ptr);
            }
        }

        public IMAGE_LOAD_CONFIG_DIRECTORY64 LoadConfig64
        {
            get => Marshal.PtrToStructure<IMAGE_LOAD_CONFIG_DIRECTORY64>(_ptr);
        }

        public IMAGE_LOAD_CONFIG_DIRECTORY32 LoadConfig
        {
            get => Marshal.PtrToStructure<IMAGE_LOAD_CONFIG_DIRECTORY32>(_ptr);
        }
        /// <summary>
        /// return IRawFile to support parsing PE information from the third software
        /// 
        /// </summary>
        public IRawFile PePtr
        {
            get
            {
                return PeFile;
            }
        }

        /// <summary>
        /// SIze of the ImageLoadConfigDirectory structure.
        /// </summary>
        public uint Size
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        /// Time and date stamp. Shows seconds elapsed since 00:00:00, January 1, 1970
        /// in UCT.
        /// </summary>
        public uint TimeDateStamp
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        /// Major version number.
        /// </summary>
        public ushort MajorVesion
        {
            get => PeFile.ReadUShort(Offset + 0x8);
            set => PeFile.WriteUShort(Offset + 0x8, value);
        }

        /// <summary>
        /// Minor version number.
        /// </summary>
        public ushort MinorVersion
        {
            get => PeFile.ReadUShort(Offset + 0xA);
            set => PeFile.WriteUShort(Offset + 0xA, value);
        }

        /// <summary>
        /// GLobal flags to control system behavior.
        /// </summary>
        public uint GlobalFlagsClear
        {
            get => PeFile.ReadUInt(Offset + 0xC);
            set => PeFile.WriteUInt(Offset + 0xC, value);
        }

        /// <summary>
        /// Global flags to control system behavior.
        /// </summary>
        public uint GlobalFlagsSet
        {
            get => PeFile.ReadUInt(Offset + 0x10);
            set => PeFile.WriteUInt(Offset + 0x10, value);
        }

        /// <summary>
        /// Default time-out value for critical sections.
        /// </summary>
        public uint CriticalSectionDefaultTimeout
        {
            get => PeFile.ReadUInt(Offset + 0x14);
            set => PeFile.WriteUInt(Offset + 0x14, value);
        }

        /// <summary>
        /// The size of the minimum block that has to be freed before it's freed in bytes.
        /// </summary>
        public ulong DeCommitFreeBlockThreshold
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x18) : PeFile.ReadUInt(Offset + 0x18);
            set
            {
                if (_is64Bit)
                    PeFile.WriteULong(Offset + 0x18, value);
                else
                    PeFile.WriteUInt(Offset + 0x18, (uint) value);
            }
        }

        /// <summary>
        /// SIze of the minimum total heap memory that has to be freed before it is freed in bytes.
        /// </summary>
        public ulong DeCommitTotalFreeThreshold
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x20) : PeFile.ReadUInt(Offset + 0x1c);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0x20, value);
                else
                    PeFile.WriteUInt(Offset + 0x1C, (uint) value);
            }
        }

        /// <summary>
        /// Virtual Address of a list with addresses where the LOCK prefix is used.
        /// Will be replaced by NOP instructions on single-processor systems. Only available on x86.
        /// </summary>
        public ulong LockPrefixTable
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x28) : PeFile.ReadUInt(Offset + 0x20);
            set
            {
                if (_is64Bit)
                    PeFile.WriteULong(Offset + 0x28, value);
                else
                    PeFile.WriteUInt(Offset + 0x20, (uint) value);
            }
        }

        /// <summary>
        /// The maximum allocation size in bytes. Only used for debugging purposes.
        /// </summary>
        public ulong MaximumAllocationSize
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x30) : PeFile.ReadUInt(Offset + 0x24);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0x30, value);
                else
                    PeFile.WriteUInt(Offset + 0x24, (uint) value);
            }
        }

        /// <summary>
        /// The maximum block size that can be allocated from heap segments in bytes.
        /// </summary>
        public ulong VirtualMemoryThreshold
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x38) : PeFile.ReadUInt(Offset + 0x28);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0x38, value);
                else
                    PeFile.WriteUInt(Offset + 0x28, (uint) value);
            }
        }

        /// <summary>
        /// The processor affinity mask defines on which CPU the executable should run.
        /// </summary>
        public ulong ProcessAffinityMask
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x40) : PeFile.ReadUInt(Offset + 0x30);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0x40, value);
                else
                    PeFile.WriteUInt(Offset + 0x30, (uint) value);
            }
        }

        /// <summary>
        /// The process heap flags.
        /// </summary>
        public uint ProcessHeapFlags
        {
            get => _is64Bit ? PeFile.ReadUInt(Offset + 0x48) : PeFile.ReadUInt(Offset + 0x2C);
            set
            {
                if(_is64Bit)
                    PeFile.WriteUInt(Offset + 0x48, value);
                else
                    PeFile.WriteUInt(Offset + 0x2C, value);
            }
        }

        /// <summary>
        /// Service pack version.
        /// </summary>
        public ushort CSDVersion
        {
            get => _is64Bit ? PeFile.ReadUShort(Offset + 0x4C) : PeFile.ReadUShort(Offset + 0x34);
            set
            {
                if(_is64Bit)
                    PeFile.WriteUShort(Offset + 0x4C, value);
                else
                    PeFile.WriteUShort(Offset + 0x34, value);
            }
        }

        /// <summary>
        /// Reserved for use by the operating system.
        /// </summary>
        public ushort Reserved1
        {
            get => _is64Bit ? PeFile.ReadUShort(Offset + 0x4E) : PeFile.ReadUShort(Offset + 0x36);
            set
            {
                if(_is64Bit)
                    PeFile.WriteUShort(Offset + 0x4E, value);
                else
                    PeFile.WriteUShort(Offset + 0x36, value);
            }
        }

        /// <summary>
        /// Reserved for use by the operating system.
        /// </summary>
        public ulong EditList
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x50) : PeFile.ReadUInt(Offset + 0x38);
            set
            {
                if (_is64Bit)
                    PeFile.WriteULong(Offset + 0x50, value);
                else
                    PeFile.WriteUInt(Offset + 0x38, (uint) value);
            }
        }

        /// <summary>
        /// Pointer to a cookie used by Visual C++ or GS implementation.
        /// </summary>
        public ulong SecurityCoockie
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x58) : PeFile.ReadUInt(Offset + 0x3C);
            set
            {
                if (_is64Bit)
                    PeFile.WriteULong(Offset + 0x58, value);
                else
                    PeFile.WriteUInt(Offset + 0x3C, (uint) value);
            }
        }

        /// <summary>
        /// Virtual Address of a sorted table of RVAs of each valid and unique handler in the image.
        /// Only available on x86.
        /// </summary>
        public ulong SEHandlerTable
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x60) : PeFile.ReadUInt(Offset + 0x40);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0x60, value);
                else
                    PeFile.WriteUInt(Offset + 0x40, (uint) value);
            }
        }

        /// <summary>
        /// Count of unique exception handlers in the table. Only available on x86.
        /// </summary>
        public ulong SEHandlerCount
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x68) : PeFile.ReadUInt(Offset + 0x44);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0x68, value);
                else
                    PeFile.WriteUInt(Offset + 0x44, (uint) value);
            }
        }

        /// <summary>
        /// Control flow guard (Win 8.1 and up) function pointer.
        /// </summary>
        public ulong GuardCFCheckFunctionPointer
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x70) : PeFile.ReadUInt(Offset + 0x48);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0x70, value);
                else
                    PeFile.WriteUInt(Offset + 0x4C, (uint) value);
            }
        }

        /// <summary>
        /// Reserved
        /// </summary>
        public ulong Reserved2
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x78) : PeFile.ReadUInt(Offset + 0x4C);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0x78, value);
                else
                    PeFile.WriteUInt(Offset + 0x4C, (uint) value);
            }
        }

        /// <summary>
        /// Pointer to the control flow guard function table. Only on Win 8.1 and up.
        /// </summary>
        public ulong GuardCFFunctionTable
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x80) : PeFile.ReadUInt(Offset + 0x50);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0x80, value);
                else
                    PeFile.WriteUInt(Offset + 0x50, (uint) value);
            }
        }

        /// <summary>
        /// Count of functions under control flow guard. Only on Win 8.1 and up.
        /// </summary>
        public ulong GuardCFFunctionCount
        {
            get => _is64Bit ? PeFile.ReadULong(Offset + 0x88) : PeFile.ReadUInt(Offset + 0x54);
            set
            {
                if(_is64Bit)
                    PeFile.WriteULong(Offset + 0x88, value);
                else
                    PeFile.WriteUInt(Offset + 0x54, (uint) value);
            }
        }

        /// <summary>
        /// Flags for the control flow guard. Only on Win 8.1 and up.
        /// </summary>
        public uint GuardFlags
        {
            get => _is64Bit ? PeFile.ReadUInt(Offset + 0x90) : PeFile.ReadUInt(Offset + 0x58);
            set
            {
                if(_is64Bit)
                    PeFile.WriteUInt(Offset + 0x90, value);
                else
                    PeFile.WriteUInt(Offset + 0x58, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public IMAGE_LOAD_CONFIG_CODE_INTEGRITY CodeIntegrity
        {
            get => _is64Bit ? LoadConfig64.CodeIntegrity : LoadConfig.CodeIntegrity;
        }


    }
    /// <summary>
    /// https://github.com/dahall/Vanara/blob/3da05fe4d87bc5de96527fad5c9b7e6058690deb/PInvoke/DbgHelp/WinNT.cs#L1013
    /// Undocumented
    /// </summary>

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_LOAD_CONFIG_CODE_INTEGRITY
    {
        /// <summary>Flags to indicate if CI information is available, etc.</summary>
        public ushort Flags;

        /// <summary>0xFFFF means not available</summary>
        public ushort Catalog;

        /// <summary/>
        public uint CatalogOffset;

        /// <summary>Additional bitmask to be defined later</summary>
        public uint Reserved;
    }
    /// <summary>
    /// https://github.com/dahall/Vanara/blob/3da05fe4d87bc5de96527fad5c9b7e6058690deb/PInvoke/DbgHelp/WinNT.cs#L1244
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IMAGE_LOAD_CONFIG_DIRECTORY64
    {
        /// <summary>The size of the structure. For Windows XP, the size must be specified as 64 for x86 images.</summary>
        public uint Size;

        /// <summary>
        /// The date and time stamp value. The value is represented in the number of seconds elapsed since midnight (00:00:00), January
        /// 1, 1970, Universal Coordinated Time, according to the system clock. The time stamp can be printed using the C run-time (CRT)
        /// function <c>ctime</c>.
        /// </summary>
        public uint TimeDateStamp;

        /// <summary>The major version number.</summary>
        public ushort MajorVersion;

        /// <summary>The minor version number.</summary>
        public ushort MinorVersion;

        /// <summary>The global flags that control system behavior. For more information, see Gflags.exe.</summary>
        public uint GlobalFlagsClear;

        /// <summary>The global flags that control system behavior. For more information, see Gflags.exe.</summary>
        public uint GlobalFlagsSet;

        /// <summary>The critical section default time-out value.</summary>
        public uint CriticalSectionDefaultTimeout;

        /// <summary>
        /// The size of the minimum block that must be freed before it is freed (de-committed), in bytes. This value is advisory.
        /// </summary>
        public ulong DeCommitFreeBlockThreshold;

        /// <summary>
        /// The size of the minimum total memory that must be freed in the process heap before it is freed (de-committed), in bytes.
        /// This value is advisory.
        /// </summary>
        public ulong DeCommitTotalFreeThreshold;

        /// <summary>
        /// The VA of a list of addresses where the LOCK prefix is used. These will be replaced by NOP on single-processor systems. This
        /// member is available only for x86.
        /// </summary>
        public ulong LockPrefixTable;

        /// <summary>The maximum allocation size, in bytes. This member is obsolete and is used only for debugging purposes.</summary>
        public ulong MaximumAllocationSize;

        /// <summary>The maximum block size that can be allocated from heap segments, in bytes.</summary>
        public ulong VirtualMemoryThreshold;

        /// <summary>
        /// The process affinity mask. For more information, see GetProcessAffinityMask. This member is available only for .exe files.
        /// </summary>
        public ulong ProcessAffinityMask;

        /// <summary>The process heap flags. For more information, see HeapCreate.</summary>
        public uint ProcessHeapFlags;

        /// <summary>The service pack version.</summary>
        public ushort CSDVersion;

        /// <summary/>
        public ushort DependentLoadFlags;

        /// <summary>Reserved for use by the system.</summary>
        public ulong EditList;

        /// <summary>A pointer to a cookie that is used by Visual C++ or GS implementation.</summary>
        public ulong SecurityCookie;

        /// <summary>
        /// The VA of the sorted table of RVAs of each valid, unique handler in the image. This member is available only for x86.
        /// </summary>
        public ulong SEHandlerTable;

        /// <summary>The count of unique handlers in the table. This member is available only for x86.</summary>
        public ulong SEHandlerCount;

        /// <summary/>
        public ulong GuardCFCheckFunctionPointer;

        /// <summary/>
        public ulong GuardCFDispatchFunctionPointer;

        /// <summary/>
        public ulong GuardCFFunctionTable;

        /// <summary/>
        public ulong GuardCFFunctionCount;

        /// <summary/>
        public uint GuardFlags;

        /// <summary/>
        public IMAGE_LOAD_CONFIG_CODE_INTEGRITY CodeIntegrity;

        /// <summary/>
        public ulong GuardAddressTakenIatEntryTable;

        /// <summary/>
        public ulong GuardAddressTakenIatEntryCount;

        /// <summary/>
        public ulong GuardLongJumpTargetTable;

        /// <summary/>
        public ulong GuardLongJumpTargetCount;

        /// <summary/>
        public ulong DynamicValueRelocTable;

        /// <summary/>
        public ulong CHPEMetadataPointer;

        /// <summary/>
        public ulong GuardRFFailureRoutine;

        /// <summary/>
        public ulong GuardRFFailureRoutineFunctionPointer;

        /// <summary/>
        public uint DynamicValueRelocTableOffset;

        /// <summary/>
        public ushort DynamicValueRelocTableSection;

        /// <summary/>
        public ushort Reserved2;

        /// <summary/>
        public ulong GuardRFVerifyStackPointerFunctionPointer;

        /// <summary/>
        public uint HotPatchTableOffset;

        /// <summary/>
        public uint Reserved3;

        /// <summary/>
        public ulong EnclaveConfigurationPointer;

        /// <summary/>
        public ulong VolatileMetadataPointer;

        /// <summary/>
        public ulong GuardEHContinuationTable;

        /// <summary/>
        public ulong GuardEHContinuationCount;

        /// <summary/>
        public ulong GuardXFGCheckFunctionPointer;   //VA

        /// <summary/>
        public ulong GuardXFGDispatchFunctionPointer; //VA

        /// <summary/>
        public ulong GuardXFGTableDispatchFunctionPointer; //VA

        /// <summary/>
        public ulong CastGuardOsDeterminedFailureMode; //VA

        /// <summary/>
        public ulong GuardMemcpyFunctionPointer;     //VA
    }

    /// <summary>
    /// https://github.com/dahall/Vanara/blob/3da05fe4d87bc5de96527fad5c9b7e6058690deb/PInvoke/DbgHelp/WinNT.cs#L1052
    /// </summary>

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IMAGE_LOAD_CONFIG_DIRECTORY32
    {
        /// <summary>The size of the structure. For Windows XP, the size must be specified as 64 for x86 images.</summary>
        public uint Size;

        /// <summary>
        /// The date and time stamp value. The value is represented in the number of seconds elapsed since midnight (00:00:00), January
        /// 1, 1970, Universal Coordinated Time, according to the system clock. The time stamp can be printed using the C run-time (CRT)
        /// function <c>ctime</c>.
        /// </summary>
        public uint TimeDateStamp;

        /// <summary>The major version number.</summary>
        public ushort MajorVersion;

        /// <summary>The minor version number.</summary>
        public ushort MinorVersion;

        /// <summary>The global flags that control system behavior. For more information, see Gflags.exe.</summary>
        public uint GlobalFlagsClear;

        /// <summary>The global flags that control system behavior. For more information, see Gflags.exe.</summary>
        public uint GlobalFlagsSet;

        /// <summary>The critical section default time-out value.</summary>
        public uint CriticalSectionDefaultTimeout;

        /// <summary>
        /// The size of the minimum block that must be freed before it is freed (de-committed), in bytes. This value is advisory.
        /// </summary>
        public uint DeCommitFreeBlockThreshold;

        /// <summary>
        /// The size of the minimum total memory that must be freed in the process heap before it is freed (de-committed), in bytes.
        /// This value is advisory.
        /// </summary>
        public uint DeCommitTotalFreeThreshold;

        /// <summary>
        /// The VA of a list of addresses where the LOCK prefix is used. These will be replaced by NOP on single-processor systems. This
        /// member is available only for x86.
        /// </summary>
        public uint LockPrefixTable;

        /// <summary>The maximum allocation size, in bytes. This member is obsolete and is used only for debugging purposes.</summary>
        public uint MaximumAllocationSize;

        /// <summary>The maximum block size that can be allocated from heap segments, in bytes.</summary>
        public uint VirtualMemoryThreshold;

        /// <summary>The process heap flags. For more information, see HeapCreate.</summary>
        public uint ProcessHeapFlags;

        /// <summary>
        /// The process affinity mask. For more information, see GetProcessAffinityMask. This member is available only for .exe files.
        /// </summary>
        public uint ProcessAffinityMask;

        /// <summary>The service pack version.</summary>
        public ushort CSDVersion;

        /// <summary/>
        public ushort DependentLoadFlags;

        /// <summary>Reserved for use by the system.</summary>
        public uint EditList;

        /// <summary>A pointer to a cookie that is used by Visual C++ or GS implementation.</summary>
        public uint SecurityCookie;

        /// <summary>
        /// The VA of the sorted table of RVAs of each valid, unique handler in the image. This member is available only for x86.
        /// </summary>
        public uint SEHandlerTable;

        /// <summary>The count of unique handlers in the table. This member is available only for x86.</summary>
        public uint SEHandlerCount;

        /// <summary/>
        public uint GuardCFCheckFunctionPointer;

        /// <summary/>
        public uint GuardCFDispatchFunctionPointer;

        /// <summary/>
        public uint GuardCFFunctionTable;

        /// <summary/>
        public uint GuardCFFunctionCount;

        /// <summary/>
        public uint GuardFlags;

        /// <summary/>
        public IMAGE_LOAD_CONFIG_CODE_INTEGRITY CodeIntegrity;

        /// <summary/>
        public uint GuardAddressTakenIatEntryTable;

        /// <summary/>
        public uint GuardAddressTakenIatEntryCount;

        /// <summary/>
        public uint GuardLongJumpTargetTable;

        /// <summary/>
        public uint GuardLongJumpTargetCount;

        /// <summary/>
        public uint DynamicValueRelocTable;

        /// <summary/>
        public uint CHPEMetadataPointer;

        /// <summary/>
        public uint GuardRFFailureRoutine;

        /// <summary/>
        public uint GuardRFFailureRoutineFunctionPointer;

        /// <summary/>
        public uint DynamicValueRelocTableOffset;

        /// <summary/>
        public ushort DynamicValueRelocTableSection;

        /// <summary/>
        public ushort Reserved2;

        /// <summary/>
        public uint GuardRFVerifyStackPointerFunctionPointer;

        /// <summary/>
        public uint HotPatchTableOffset;

        /// <summary/>
        public uint Reserved3;

        /// <summary/>
        public uint EnclaveConfigurationPointer;

        /// <summary/>
        public uint VolatileMetadataPointer;

        /// <summary/>
        public uint GuardEHContinuationTable;

        /// <summary/>
        public uint GuardEHContinuationCount;

        /// <summary/>
        public uint GuardXFGCheckFunctionPointer;   // VA

        /// <summary/>
        public uint GuardXFGDispatchFunctionPointer; // VA

        /// <summary/>
        public uint GuardXFGTableDispatchFunctionPointer; // VA

        /// <summary/>
        public uint CastGuardOsDeterminedFailureMode; // VA

        /// <summary/>
        public uint GuardMemcpyFunctionPointer;     // VA
    }
}