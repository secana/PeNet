using System.Collections.Generic;

namespace PeNet.Header.Pe
{
    public class AdditionalImport
    {
        public string Module { get; }
        public List<string> Functions { get; }

        public AdditionalImport(string module, List<string> funcs)
        {
            Module = module;
            Functions = funcs;
        }
    }
}