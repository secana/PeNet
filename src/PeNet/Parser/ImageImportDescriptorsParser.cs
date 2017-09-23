using System.Collections.Generic;
using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageImportDescriptorsParser : SafeParser<IMAGE_IMPORT_DESCRIPTOR[]>
    {
        public ImageImportDescriptorsParser(byte[] buff, uint offset)
            : base(buff, offset)
        {
        }

        protected override IMAGE_IMPORT_DESCRIPTOR[] ParseTarget()
        {
            if (_offset == 0)
                return null;

            var idescs = new List<IMAGE_IMPORT_DESCRIPTOR>();
            uint idescSize = 20; // Size of IMAGE_IMPORT_DESCRIPTOR (5 * 4 Byte)
            uint round = 0;

            while (true)
            {
                var idesc = new IMAGE_IMPORT_DESCRIPTOR(_buff, _offset + idescSize*round);

                // Found the last IMAGE_IMPORT_DESCRIPTOR which is completely null (except TimeDateStamp).
                if (idesc.OriginalFirstThunk == 0
                    //&& idesc.TimeDateStamp == 0
                    && idesc.ForwarderChain == 0
                    && idesc.Name == 0
                    && idesc.FirstThunk == 0)
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