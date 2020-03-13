using System.Collections.Generic;
using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class ImageTlsDirectoryParser : SafeParser<ImageTlsDirectory>
    {
        private readonly bool _is64Bit;
        private readonly ImageSectionHeader[] _sectionsHeaders;

        internal ImageTlsDirectoryParser(
            IRawFile peFile, 
            uint offset, 
            bool is64Bit, 
            ImageSectionHeader[] sectionHeaders
            ) 
            : base(peFile, offset)
        {
            _is64Bit = is64Bit;
            _sectionsHeaders = sectionHeaders;
        }

        protected override ImageTlsDirectory ParseTarget()
        {
            var tlsDir = new ImageTlsDirectory(PeFile, Offset, _is64Bit);
            tlsDir.TlsCallbacks = ParseTlsCallbacks(tlsDir.AddressOfCallBacks);
            return tlsDir;
        }

        private ImageTlsCallback[] ParseTlsCallbacks(ulong addressOfCallBacks)
        {
            var callbacks = new List<ImageTlsCallback>();
            var rawAddressOfCallbacks = (uint) addressOfCallBacks.VaToOffset(_sectionsHeaders);

            uint count = 0;
            while (true)
            {
                if (_is64Bit)
                {
                    var cb = new ImageTlsCallback(PeFile, rawAddressOfCallbacks + count*8, _is64Bit);
                    if (cb.Callback == 0)
                        break;

                    callbacks.Add(cb);
                    count++;
                }
                else
                {
                    var cb = new ImageTlsCallback(PeFile, rawAddressOfCallbacks + count*4, _is64Bit);
                    if (cb.Callback == 0)
                        break;

                    callbacks.Add(cb);
                    count++;
                }
            }

            return callbacks.ToArray();
        }
    }
}