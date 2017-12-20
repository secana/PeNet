using System.IO;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1Set : Asn1CompositeNode {

        public const string NODE_NAME = "Set";

        public static Asn1Set ReadFrom(Stream stream) {
            var res = new Asn1Set();
            res.ReadChildren(stream);
            return res;
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.Set;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME);
        }

        public new static Asn1Set Parse(XElement xNode) {
            var res = new Asn1Set();
            res.ParseChildren(xNode);
            return res;
        }
    }
}
