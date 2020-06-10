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
            try
            {
                var typeRefs = MdtHdr?.Tables.TypeRef;
                if (typeRefs is null || MdsStream is null) return null;

                var noNamespace = typeRefs
                    .Where(t => string.IsNullOrEmpty(MdsStream.GetStringAtIndex(t.TypeNamespace)))
                    .Select(t => string.Join("-",
                        MdsStream.GetStringAtIndex(t.TypeNamespace),
                        MdsStream.GetStringAtIndex(t.TypeName)
                    ))
                    .OrderBy(t => t)
                    .ToList();

                var withNamespace = typeRefs
                    .Where(t => !string.IsNullOrEmpty(MdsStream.GetStringAtIndex(t.TypeNamespace)))
                    .Select(t => string.Join("-",
                        MdsStream.GetStringAtIndex(t.TypeNamespace),
                        MdsStream.GetStringAtIndex(t.TypeName)
                    ))
                    .OrderBy(t => t)
                    .ToList();

                var allNamespaces = noNamespace.Concat(withNamespace);
                var typeRefsAsString = string.Join(",", allNamespaces);

                using var sha256 = new SHA256Managed();
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(typeRefsAsString));
                var stringBuilder = new StringBuilder();
                foreach (var b in bytes)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }

                return stringBuilder.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
