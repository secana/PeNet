using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
                using var sha256 = new SHA256Managed();
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(typeRefsAsString));
                var stringBuilder = new StringBuilder();
                foreach (var b in bytes)
                    stringBuilder.AppendFormat("{0:x2}", b);
                return stringBuilder.ToString();
            }

            var typeRefs = MdtHdr?.Tables.TypeRef;
            if (typeRefs is null || MdsStream is null) return null;

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
