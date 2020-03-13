using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class RuntimeFunctionsParser : SafeParser<RuntimeFunction[]>
    {
        private readonly uint _directorySize;
        private readonly bool _is32Bit;
        private readonly ImageSectionHeader[] _sectionHeaders;

        public RuntimeFunctionsParser(
            IRawFile peFile,
            long offset,
            bool is32Bit,
            uint directorySize,
            ImageSectionHeader[] sectionHeaders
            )
            : base(peFile, offset)
        {
            _is32Bit = is32Bit;
            _directorySize = directorySize;
            _sectionHeaders = sectionHeaders;
        }

        protected override RuntimeFunction[]? ParseTarget()
        {
            if (_is32Bit || Offset == 0)
                return null;

            const int sizeOfRuntimeFunction = 0xC;
            var rf = new RuntimeFunction[_directorySize/sizeOfRuntimeFunction];

            for (var i = 0; i < rf.Length; i++)
            {
                rf[i] = new RuntimeFunction(PeFile, (uint) (Offset + i*sizeOfRuntimeFunction), _sectionHeaders);
            }

            return rf;
        }
    }
}