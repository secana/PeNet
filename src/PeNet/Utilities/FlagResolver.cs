using System;
using System.Collections.Generic;

namespace PeNet.Utilities
{
    /// <summary>
    /// Parser functions for different flags in the PE header.
    /// </summary>
    public static class FlagResolver
    {
        

        

        

        /// <summary>
        ///     Resolve flags from the ImageCor20Header COM+ 2 (CLI) header to
        ///     their string representation.
        /// </summary>
        /// <param name="comImageFlags">Flags from ImageCor20Header.</param>
        /// <returns>List with resolved flag names.</returns>
        public static List<string> ResolveComImageFlags(uint comImageFlags)
        {
            var st = new List<string>();
            foreach (var flag in (DotNetConstants.COMImageFlag[])Enum.GetValues(typeof(DotNetConstants.COMImageFlag)))
            {
                if ((comImageFlags & (uint)flag) == (uint)flag)
                {
                    st.Add(flag.ToString());
                }
            }
            return st;
        }

        /// <summary>
        ///     Resolve which tables are present in the .Net header based
        ///     on the MaskValid flags from the MetaDataTablesHdr.
        /// </summary>
        /// <param name="maskValid">MaskValid value from the MetaDataTablesHdr</param>
        /// <returns>List with present table names.</returns>
        public static List<string> ResolveMaskValidFlags(ulong maskValid)
        {
            var st = new List<string>();
            foreach (var flag in (DotNetConstants.MaskValidFlags[])Enum.GetValues(typeof(DotNetConstants.MaskValidFlags)))
            {
                if ((maskValid & (ulong)flag) == (ulong)flag)
                {
                    st.Add(flag.ToString());
                }
            }
            return st;
        }
    }
}