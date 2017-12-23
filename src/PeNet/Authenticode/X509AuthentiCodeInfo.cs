using System;
using PeNet.Asn1;

namespace PeNet.Authenticode
{
    public class ContentInfo
    {
        public Asn1Node Content { get; }
        public string ContentType { get; }

        public ContentInfo(byte[] data)
            : this(Asn1Node.ReadNode(data)) {}

        public ContentInfo(Asn1Node asn1)
        {
            var nodes = asn1.Nodes;
            // SEQUENCE with 1 or 2 elements
            if ((asn1.NodeType != Asn1UniversalNodeType.Sequence) || (nodes.Count < 1 && nodes.Count > 2))
                throw new ArgumentException("Invalid ASN1");
            if (!(nodes[0] is Asn1ObjectIdentifier))
                throw new ArgumentException("Invalid contentType");
            ContentType = ((Asn1ObjectIdentifier) nodes[0]).FriendlyName;
            if (nodes.Count <= 1) return;
            if (nodes[1].TagClass != Asn1TagClass.ContextDefined || nodes[1].TagForm != Asn1TagForm.Constructed)
                throw new ArgumentException("Invalid content");
            Content = nodes[1];
        }
    }

    public class SignedData
    {
        public SignedData(Asn1Node asn1)
        {
            var node = asn1.Nodes[0];
            if ((node.NodeType != Asn1UniversalNodeType.Sequence) || (node.Nodes.Count < 4))
                throw new ArgumentException("Invalid SignedData");

            if (node.Nodes[0].NodeType != Asn1UniversalNodeType.Integer)
                throw new ArgumentException("Invalid version");

            ContentInfo = new ContentInfo(node.Nodes[2]);
        }

        public ContentInfo ContentInfo { get; }
    }
}