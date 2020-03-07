using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageDebugDirectoryParser : SafeParser<IMAGE_DEBUG_DIRECTORY[]>
    {
        private readonly uint _size;

        internal ImageDebugDirectoryParser(IRawFile peFile, uint offset, uint size)
            : base(peFile, offset)
        {
            this._size = size;
        }

        protected override IMAGE_DEBUG_DIRECTORY[] ParseTarget()
        {
            var numEntries = _size / 28; // Debug entry is 28 bytes
            var entries = new IMAGE_DEBUG_DIRECTORY[numEntries];

            for(uint i = 0; i < numEntries; i++)
            {
                entries[i] = new IMAGE_DEBUG_DIRECTORY(PeFile, Offset + (i * 28));
            }

            return entries;
        }
    }
}