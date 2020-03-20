using System.Collections.Generic;
using System.Linq;
using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class ImageBaseRelocationsParser : SafeParser<ImageBaseRelocation[]>
    {
        private readonly uint _directorySize;

        public ImageBaseRelocationsParser(
            IRawFile peFile,
            uint offset,
            uint directorySize
            )
            : base(peFile, offset)
        {
            _directorySize = directorySize;
        }

        protected override ImageBaseRelocation[]? ParseTarget()
        {
            if (Offset == 0)
                return null;

            var imageBaseRelocations = new List<ImageBaseRelocation>();
            var currentBlock = Offset;


            while (true)
            {
                if (currentBlock >= Offset + _directorySize - 8)
                    break;

                imageBaseRelocations.Add(new ImageBaseRelocation(PeFile, currentBlock, _directorySize));
                currentBlock += imageBaseRelocations.Last().SizeOfBlock;
            }

            return imageBaseRelocations.ToArray();
        }
    }
}