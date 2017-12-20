using System;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1Real : Asn1Node {

        public override Asn1UniversalNodeType NodeType { get; }

        public override Asn1TagForm TagForm { get; }

        protected override XElement ToXElementCore() {
            throw new NotImplementedException();
        }

        protected override byte[] GetBytesCore() {
            throw new NotImplementedException();
        }
    }
}
