using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class ImageDebugDirectoryParser : SafeParser<ImageDebugDirectory[]>
    {
        private readonly uint _size;

        internal ImageDebugDirectoryParser(IRawFile peFile, uint offset, uint size)
            : base(peFile, offset)
        {
            this._size = size;
        }

        protected override ImageDebugDirectory[] ParseTarget()
        {
            var numEntries = _size / 28; // Debug entry is 28 bytes
            var entries = new ImageDebugDirectory[numEntries];

            for(uint i = 0; i < numEntries; i++)
            {
                entries[i] = new ImageDebugDirectory(PeFile, Offset + (i * 28));
            }

            return entries;
        }
    }
}