using PeNet.FileParser;

namespace PeNet.Header.Pe
{
    /// <summary>
    ///     The runtime function struct is represents
    ///     a function in the exception header for x64
    ///     applications.
    /// </summary>
    public class RuntimeFunction : AbstractStructure
    {
        private UnwindInfo? _resolvedUnwindInfo;
        private readonly ImageSectionHeader[] _sectionHeaders;

        /// <summary>
        ///     Create a new RuntimeFunction object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the runtime function struct.</param>
        /// <param name="sh">Section Headers of the PE file.</param>
        public RuntimeFunction(IRawFile peFile, long offset, ImageSectionHeader[] sh)
            : base(peFile, offset)
        {
            _sectionHeaders = sh;
        }

        /// <summary>
        ///     RVA Start of the function in code.
        /// </summary>
        public uint FunctionStart
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     RVA End of the function in code.
        /// </summary>
        public uint FunctionEnd
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Pointer to the unwind information.
        /// </summary>
        public uint UnwindInfo
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        ///     Unwind Info object belonging to this Runtime Function.
        /// </summary>
        public UnwindInfo ResolvedUnwindInfo {
            get
            {
                _resolvedUnwindInfo ??= GetUnwindInfo(_sectionHeaders);
                return _resolvedUnwindInfo;
            }
        }

        /// <summary>
        ///     Get the UnwindInfo from a runtime function form the
        ///     Exception header in x64 applications.
        /// </summary>
        /// <param name="sh">Section Headers of the PE file.</param>
        /// <returns>UnwindInfo for the runtime function.</returns>
        private UnwindInfo GetUnwindInfo(ImageSectionHeader[] sh)
        {
            // Check if the last bit is set in the UnwindInfo. If so, it is a chained 
            // information.
            var uwAddress = (UnwindInfo & 0x1) == 0x1
                ? UnwindInfo & 0xFFFE
                : UnwindInfo;

            var uw = new UnwindInfo(PeFile, uwAddress.RvaToOffset(sh));
            return uw;
        }
    }
}