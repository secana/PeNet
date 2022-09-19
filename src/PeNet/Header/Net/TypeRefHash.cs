using System;
using System.Linq;
using System.Text;
using PeNet.Crypto;

namespace PeNet.Header.Net
{
    public class TypeRefHash
    {
        public MetaDataTablesHdr? MdtHdr { get; }
        public MetaDataStreamString? MdsStream { get; }

        public TypeRefHash(MetaDataTablesHdr? mdtHdr, MetaDataStreamString? mdsStream)
            => (MdtHdr, MdsStream) = (mdtHdr, mdsStream);

        public string? ComputeHash()
        {
            static string GetSha256(string typeRefsAsString)
            {
                var input = Encoding.UTF8.GetBytes(typeRefsAsString);
                return Hash.ComputeHash(input, Algorithm.Sha256);
            }

            var typeRefs = MdtHdr?.Tables.TypeRef;
            if (typeRefs is null || MdsStream is null || MdtHdr?.TableDefinitions[(int)MetadataToken.TypeReference].IsInvalid == true) return null;

            try
            {
                var namespacesAndTypes = typeRefs
                    .OrderBy(t => MdsStream.GetStringAtIndex(t.TypeNamespace))
                    .ThenBy(t => MdsStream.GetStringAtIndex(t.TypeName))
                    .Select(t => string.Join("-",
                        MdsStream.GetStringAtIndex(t.TypeNamespace),
                        MdsStream.GetStringAtIndex(t.TypeName)
                    ))
                    .ToList();

                var typeRefsAsString = string.Join(",", namespacesAndTypes);
                return GetSha256(typeRefsAsString);
            }
            catch (Exception) { return null; }
        }
    }
}
