using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    /// The ImageLoadConfigDirectory hold information
    /// important to load the PE file correctly.
    /// </summary>
    public class ImageLoadConfigDirectory : AbstractStructure
    {
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
    }
}