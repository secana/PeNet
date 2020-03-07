using PeNet.Structures;

namespace PeNet.Parser
{
    internal class RuntimeFunctionsParser : SafeParser<RUNTIME_FUNCTION[]>
    {
        private readonly uint _directorySize;
        private readonly bool _is32Bit;
        private readonly IMAGE_SECTION_HEADER[] _sectionHeaders;

        public RuntimeFunctionsParser(
            IRawFile peFile,
            long offset,
            bool is32Bit,
            uint directorySize,
            IMAGE_SECTION_HEADER[] sectionHeaders
            )
            : base(peFile, offset)
        {
            _is32Bit = is32Bit;
            _directorySize = directorySize;
            _sectionHeaders = sectionHeaders;
        }

        protected override RUNTIME_FUNCTION[]? ParseTarget()
        {
            if (_is32Bit || Offset == 0)
                return null;

            const int sizeOfRuntimeFunction = 0xC;
            var rf = new RUNTIME_FUNCTION[_directorySize/sizeOfRuntimeFunction];

            for (var i = 0; i < rf.Length; i++)
            {
                rf[i] = new RUNTIME_FUNCTION(PeFile, (uint) (Offset + i*sizeOfRuntimeFunction), _sectionHeaders);
            }

            return rf;
        }
    }
}