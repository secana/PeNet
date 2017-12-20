using System.IO;
using System.Xml.Linq;

namespace Asn1 {
    public abstract class Asn1CompositeNode : Asn1Node {

        public override Asn1TagForm TagForm => Asn1TagForm.Constructed;


        public override XElement ToXElement() {
            var res = base.ToXElement();
            foreach (var node in Nodes) {
                res.Add(node.ToXElement());
            }
            return res;
        }

        protected void ParseChildren(XElement xNode) {
            foreach (var xChild in xNode.Elements()) {
                var child = Parse(xChild);
                Nodes.Add(child);
            }
        }

        protected override byte[] GetBytesCore() {
            var mem = new MemoryStream();
            foreach (var node in Nodes) {
                var data = node.GetBytes();
                mem.Write(data, 0, data.Length);
            }
            return mem.ToArray();
        }

    }
}
