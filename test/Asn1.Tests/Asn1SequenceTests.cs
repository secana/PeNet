using System.IO;
using Xunit;

namespace Asn1.Tests {
    public class Asn1SequenceTests : BaseTest {

        private static readonly byte[] _etalon = { 0x30, 0x0e, 0x06, 0x03, 0x55, 0x04, 0x0a, 0x13, 0x07, 0x54, 0x65, 0x73, 0x74, 0x4f, 0x72, 0x67 };

        [Fact]
        public void ReadTest() {
            var node = Asn1Node.ReadNode(new MemoryStream(_etalon));
            var typed = node as Asn1Sequence;
            Assert.NotNull(typed);
            Assert.Equal(2, typed.Nodes.Count);
        }

        [Fact]
        public void WriteTest() {
            var node = new Asn1Sequence();
            node.Nodes.Add(new Asn1ObjectIdentifier("2.5.4.10"));
            node.Nodes.Add(new Asn1PrintableString { Value = "TestOrg" });
            var data = node.GetBytes();
            AreEqual(_etalon, data);
        }
    }
}
