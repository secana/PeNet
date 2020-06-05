using System.Collections.Generic;

namespace PeNet
{
    /// <summary>
    /// Additional import used to add new 
    /// imports to the PE file.
    /// </summary>
    public class AdditionalImport
    {
        /// <summary>
        /// Module name to add the imports from, e.g. "kernel32.dll" or "HAL.dll".
        /// </summary>
        public string Module { get; }

        /// <summary>
        /// List with functions names to import from the module.
        /// </summary> 
        public List<string> Functions { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="module">Module name to add the imports from, e.g. "kernel32.dll" or "HAL.dll".</param>
        /// <param name="funcs">List with functions names to import from the module.</param>
        public AdditionalImport(string module, List<string> funcs)
        {
            Module = module;
            Functions = funcs;
        }
    }
}