using PeNet.FileParser;
using PeNet.Header.Pe;
using System.Collections.Generic;

namespace PeNet.HeaderParser.Pe
{
    internal class ImageDelayImportDescriptorParser : SafeParser<ImageDelayImportDescriptor[]>
    {
        internal ImageDelayImportDescriptorParser(IRawFile peFile, long offset) 
            : base(peFile, offset)
        {
        }

        protected override ImageDelayImportDescriptor[]? ParseTarget()
        {
            if (Offset == 0)
                return null;

            var idescs = new List<ImageDelayImportDescriptor>();
            uint idescSize = 32; // Size of ImageDelayImportDescriptor (8 * 4 Byte)
            int round = 0;

            while (true)
            {
                var idesc = new ImageDelayImportDescriptor(PeFile, Offset + idescSize * round);

                // Find the last ImageDelayImportDescriptor which is completely null (except for TimeDateStamp member).
                if (idesc.GrAttrs == 0
                    && idesc.SzName == 0
                    && idesc.Phmod == 0
                    && idesc.PIat == 0
                    && idesc.PInt == 0
                    && idesc.PBoundIAT == 0
                    && idesc.PUnloadIAT == 0
                    )
                {
                    break;
                }

                idescs.Add(idesc);
                round++;
            }

            return idescs.ToArray();
        }
    }
}