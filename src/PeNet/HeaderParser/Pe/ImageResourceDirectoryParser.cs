using System.Linq;
using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet.HeaderParser.Pe
{
    internal class ImageResourceDirectoryParser : SafeParser<ImageResourceDirectory>
    {
        private long _resourceDirSize;
        
        internal ImageResourceDirectoryParser(IRawFile peFile, long offset, long size)
            : base(peFile, offset)
        {
            _resourceDirSize = size;
        }

        protected override ImageResourceDirectory? ParseTarget()
        {
            if (Offset == 0)
                return null;

            // Parse the root directory.
            var root = new ImageResourceDirectory(PeFile, Offset, Offset);

            // Check if the number of entries is bigger than the resource directory
            // and thus cannot be parsed correctly.
            // 10 byte is the minimal size of an entry.
            if ((root.NumberOfIdEntries + root.NumberOfNameEntries) * 10 >= _resourceDirSize)
                return root;

            if (root.DirectoryEntries is null)
                return root;

            // Parse the second stage (type)
            foreach (var de in root.DirectoryEntries)
            {
                if(SanityCheckFailed(de))
                    continue;
                
                // This check only applies to the second level.
                if (de!.IsIdEntry && de.NameResolved == "unknown")
                   continue;

                de!.ResourceDirectory = new ImageResourceDirectory(
                    PeFile,
                    Offset + de.OffsetToDirectory,
                    Offset
                );
                
                var sndLevel = de?.ResourceDirectory?.DirectoryEntries;
                if(sndLevel is null)
                    continue;

                // Parse the third stage (name/IDs)
                foreach (var de2 in sndLevel)
                {
                    if(SanityCheckFailed(de2))
                        continue;
                    
                    de2!.ResourceDirectory = new ImageResourceDirectory(
                        PeFile,
                        Offset + de2.OffsetToDirectory,
                        Offset
                        );

                    var thrdLevel = de2?.ResourceDirectory?.DirectoryEntries;
                    if(thrdLevel is null)
                        continue;
                    
             
                    // Parse the forth stage (language) with the data.
                    foreach (var de3 in thrdLevel)
                    {
                        if(SanityCheckFailed(de3))
                            continue;
                        
                        de3!.ResourceDataEntry = new ImageResourceDataEntry(PeFile,
                            Offset + de3.OffsetToData);
                    }
                }
            }

            return root;
        }
        
        bool SanityCheckFailed(ImageResourceDirectoryEntry? rd)
        {
            if (rd == null)
                return true;
        
            if(rd.IsNamedEntry && rd.NameResolved == null ) 
                return true;

            if (rd.IsNamedEntry && rd.NameResolved == "unknown")
                return true;
            
            if(Offset + rd.OffsetToDirectory > Offset + _resourceDirSize)
                return true;

            return false;
        }
    }
}