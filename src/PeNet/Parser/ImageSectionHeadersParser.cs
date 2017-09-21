using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageSectionHeadersParser : SafeParser<IMAGE_SECTION_HEADER[]>
    {
        private readonly ushort _numOfSections;

        internal ImageSectionHeadersParser(byte[] buff, uint offset, ushort numOfSections)
            : base(buff, offset)
        {
            _numOfSections = numOfSections;
        }

        protected override IMAGE_SECTION_HEADER[] ParseTarget()
        {
            var sh = new IMAGE_SECTION_HEADER[_numOfSections];
            uint secSize = 0x28; // Every section header is 40 bytes in size.
            for (uint i = 0; i < _numOfSections; i++)
            {
                sh[i] = new IMAGE_SECTION_HEADER(_buff, _offset + i*secSize);
            }

            return sh;
        }
    }
}