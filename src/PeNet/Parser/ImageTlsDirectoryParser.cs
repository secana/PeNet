using System.Collections.Generic;
using PeNet.Structures;
using PeNet.Utilities;

namespace PeNet.Parser
{
    internal class ImageTlsDirectoryParser : SafeParser<IMAGE_TLS_DIRECTORY>
    {
        private readonly bool _is64Bit;
        private readonly IMAGE_SECTION_HEADER[] _sectionsHeaders;

        internal ImageTlsDirectoryParser(
            byte[] buff, 
            uint offset, 
            bool is64Bit, 
            IMAGE_SECTION_HEADER[] sectionHeaders
            ) 
            : base(buff, offset)
        {
            _is64Bit = is64Bit;
            _sectionsHeaders = sectionHeaders;
        }

        protected override IMAGE_TLS_DIRECTORY ParseTarget()
        {
            var tlsDir = new IMAGE_TLS_DIRECTORY(_buff, _offset, _is64Bit);
            tlsDir.TlsCallbacks = ParseTlsCallbacks(tlsDir.AddressOfCallBacks);
            return tlsDir;
        }

        private IMAGE_TLS_CALLBACK[] ParseTlsCallbacks(ulong addressOfCallBacks)
        {
            var callbacks = new List<IMAGE_TLS_CALLBACK>();
            var rawAddressOfCallbacks = (uint) addressOfCallBacks.VAtoFileMapping(_sectionsHeaders);

            uint count = 0;
            while (true)
            {
                if (_is64Bit)
                {
                    var cb = new IMAGE_TLS_CALLBACK(_buff, rawAddressOfCallbacks + count*8, _is64Bit);
                    if (cb.Callback == 0)
                        break;

                    callbacks.Add(cb);
                    count++;
                }
                else
                {
                    var cb = new IMAGE_TLS_CALLBACK(_buff, rawAddressOfCallbacks + count*4, _is64Bit);
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