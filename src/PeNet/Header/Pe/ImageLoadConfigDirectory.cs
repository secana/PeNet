using PeNet.FileParser;
using System;
using System.Runtime.InteropServices;

namespace PeNet.Header.Pe
{
    /// <summary>
    /// The ImageLoadConfigDirectory holds information
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
                // Ideally throw an exception when overflow occurred, but set size to data length to handle malformed files gracefully.
                size = (uint)data.Length;

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

        ~ImageLoadConfigDirectory() => Dispose();

        public void Dispose()
        {
            if (_ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_ptr);
                _ptr = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        private ulong ReadSize(long offset64, long offset32)
            => _is64Bit ? PeFile.ReadULong(Offset + offset64) : PeFile.ReadUInt(Offset + offset32);

        private void WriteSize(long offset64, long offset32, ulong value)
        {
            if (_is64Bit)
                PeFile.WriteULong(Offset + offset64, value);
            else
                PeFile.WriteUInt(Offset + offset32, (uint)value);
        }

        private uint ReadUInt32(long offset64, long offset32)
            => _is64Bit ? PeFile.ReadUInt(Offset + offset64) : PeFile.ReadUInt(Offset + offset32);

        private void WriteUInt32(long offset64, long offset32, uint value)
        {
            if (_is64Bit)
                PeFile.WriteUInt(Offset + offset64, value);
            else
                PeFile.WriteUInt(Offset + offset32, value);
        }

        private ushort ReadUInt16(long offset64, long offset32)
            => _is64Bit ? PeFile.ReadUShort(Offset + offset64) : PeFile.ReadUShort(Offset + offset32);

        private void WriteUInt16(long offset64, long offset32, ushort value)
        {
            if (_is64Bit)
                PeFile.WriteUShort(Offset + offset64, value);
            else
                PeFile.WriteUShort(Offset + offset32, value);
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
        /// Size of the ImageLoadConfigDirectory structure.
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
        public ushort MajorVersion
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
        /// Global flags to control system behavior.
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
            get => ReadSize(0x18, 0x18);
            set => WriteSize(0x18, 0x18, value);
        }

        /// <summary>
        /// Size of the minimum total heap memory that has to be freed before it is freed in bytes.
        /// </summary>
        public ulong DeCommitTotalFreeThreshold
        {
            get => ReadSize(0x20, 0x1c);
            set => WriteSize(0x20, 0x1c, value);
        }

        /// <summary>
        /// Virtual Address of a list with addresses where the LOCK prefix is used.
        /// Will be replaced by NOP instructions on single-processor systems. Only available on x86.
        /// </summary>
        public ulong LockPrefixTable
        {
            get => ReadSize(0x28, 0x20);
            set => WriteSize(0x28, 0x20, value);
        }

        /// <summary>
        /// The maximum allocation size in bytes. Only used for debugging purposes.
        /// </summary>
        public ulong MaximumAllocationSize
        {
            get => ReadSize(0x30, 0x24);
            set => WriteSize(0x30, 0x24, value);
        }

        /// <summary>
        /// The maximum block size that can be allocated from heap segments in bytes.
        /// </summary>
        public ulong VirtualMemoryThreshold
        {
            get => ReadSize(0x38, 0x28);
            set => WriteSize(0x38, 0x28, value);
        }

        /// <summary>
        /// The processor affinity mask defines on which CPU the executable should run.
        /// </summary>
        public ulong ProcessAffinityMask
        {
            get => ReadSize(0x40, 0x30);
            set => WriteSize(0x40, 0x30, value);
        }

        /// <summary>
        /// The process heap flags.
        /// </summary>
        public uint ProcessHeapFlags
        {
            get => ReadUInt32(0x48, 0x2C);
            set => WriteUInt32(0x48, 0x2C, value);
        }

        /// <summary>
        /// Service pack version.
        /// </summary>
        public ushort CSDVersion
        {
            get => ReadUInt16(0x4C, 0x34);
            set => WriteUInt16(0x4C, 0x34, value);
        }

        /// <summary>
        /// Reserved for use by the operating system.
        /// </summary>
        public ushort Reserved1
        {
            get => ReadUInt16(0x4E, 0x36);
            set => WriteUInt16(0x4E, 0x36, value);
        }

        /// <summary>
        /// Reserved for use by the operating system.
        /// </summary>
        public ulong EditList
        {
            get => ReadSize(0x50, 0x38);
            set => WriteSize(0x50, 0x38, value);
        }

        /// <summary>
        /// Pointer to a cookie used by Visual C++ or GS implementation.
        /// </summary>
        public ulong SecurityCookie
        {
            get => ReadSize(0x58, 0x3C);
            set => WriteSize(0x58, 0x3C, value);
        }

        /// <summary>
        /// Virtual Address of a sorted table of RVAs of each valid and unique handler in the image.
        /// Only available on x86.
        /// </summary>
        public ulong SEHandlerTable
        {
            get => ReadSize(0x60, 0x40);
            set => WriteSize(0x60, 0x40, value);
        }

        /// <summary>
        /// Count of unique exception handlers in the table. Only available on x86.
        /// </summary>
        public ulong SEHandlerCount
        {
            get => ReadSize(0x68, 0x44);
            set => WriteSize(0x68, 0x44, value);
        }

        /// <summary>
        /// Control flow guard (Win 8.1 and up) function pointer.
        /// </summary>
        public ulong GuardCFCheckFunctionPointer
        {
            get => ReadSize(0x70, 0x48);
            set => WriteSize(0x70, 0x48, value);
        }

        /// <summary>
        /// Reserved
        /// </summary>
        public ulong Reserved2
        {
            get => ReadSize(0x78, 0x4C);
            set => WriteSize(0x78, 0x4C, value);
        }

        /// <summary>
        /// Pointer to the control flow guard function table. Only on Win 8.1 and up.
        /// </summary>
        public ulong GuardCFFunctionTable
        {
            get => ReadSize(0x80, 0x50);
            set => WriteSize(0x80, 0x50, value);
        }

        /// <summary>
        /// Count of functions under control flow guard. Only on Win 8.1 and up.
        /// </summary>
        public ulong GuardCFFunctionCount
        {
            get => ReadSize(0x88, 0x54);
            set => WriteSize(0x88, 0x54, value);
        }

        /// <summary>
        /// Flags for the control flow guard. Only on Win 8.1 and up.
        /// </summary>
        public uint GuardFlags
        {
            get => ReadUInt32(0x90, 0x58);
            set => WriteUInt32(0x90, 0x58, value);
        }
        /// <summary>
        /// Code integrity information.
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

        public ulong GuardCFCheckFunctionPointer;

        public ulong GuardCFDispatchFunctionPointer;

        public ulong GuardCFFunctionTable;

        public ulong GuardCFFunctionCount;

        public uint GuardFlags;

        public IMAGE_LOAD_CONFIG_CODE_INTEGRITY CodeIntegrity;

        public ulong GuardAddressTakenIatEntryTable;

        public ulong GuardAddressTakenIatEntryCount;

        public ulong GuardLongJumpTargetTable;

        public ulong GuardLongJumpTargetCount;

        public ulong DynamicValueRelocTable;

        public ulong CHPEMetadataPointer;

        public ulong GuardRFFailureRoutine;

        public ulong GuardRFFailureRoutineFunctionPointer;

        public uint DynamicValueRelocTableOffset;

        public ushort DynamicValueRelocTableSection;

        public ushort Reserved2;

        public ulong GuardRFVerifyStackPointerFunctionPointer;

        public uint HotPatchTableOffset;

        public uint Reserved3;

        public ulong EnclaveConfigurationPointer;

        public ulong VolatileMetadataPointer;

        public ulong GuardEHContinuationTable;

        public ulong GuardEHContinuationCount;

        public ulong GuardXFGCheckFunctionPointer;   //VA

        public ulong GuardXFGDispatchFunctionPointer; //VA

        public ulong GuardXFGTableDispatchFunctionPointer; //VA

        public ulong CastGuardOsDeterminedFailureMode; //VA

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

        public uint GuardCFCheckFunctionPointer;

        public uint GuardCFDispatchFunctionPointer;

        public uint GuardCFFunctionTable;

        public uint GuardCFFunctionCount;

        public uint GuardFlags;

        public IMAGE_LOAD_CONFIG_CODE_INTEGRITY CodeIntegrity;

        public uint GuardAddressTakenIatEntryTable;

        public uint GuardAddressTakenIatEntryCount;

        public uint GuardLongJumpTargetTable;

        public uint GuardLongJumpTargetCount;

        public uint DynamicValueRelocTable;

        public uint CHPEMetadataPointer;

        public uint GuardRFFailureRoutine;

        public uint GuardRFFailureRoutineFunctionPointer;

        public uint DynamicValueRelocTableOffset;

        public ushort DynamicValueRelocTableSection;

        public ushort Reserved2;

        public uint GuardRFVerifyStackPointerFunctionPointer;

        public uint HotPatchTableOffset;

        public uint Reserved3;

        public uint EnclaveConfigurationPointer;

        public uint VolatileMetadataPointer;

        public uint GuardEHContinuationTable;

        public uint GuardEHContinuationCount;

        public uint GuardXFGCheckFunctionPointer;   // VA

        public uint GuardXFGDispatchFunctionPointer; // VA

        public uint GuardXFGTableDispatchFunctionPointer; // VA

        public uint CastGuardOsDeterminedFailureMode; // VA

        public uint GuardMemcpyFunctionPointer;     // VA
    }
}