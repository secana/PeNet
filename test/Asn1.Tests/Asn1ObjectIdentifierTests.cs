using System.IO;
using Xunit;

namespace Asn1.Tests {
    public class Asn1ObjectIdentifierTests : BaseTest {

        private static readonly byte[] _etalon = { 0x06, 0x03, 0x55, 0x04, 0x0a };

        [Fact]
        public void ReadTest() {
            var node = Asn1Node.ReadNode(new MemoryStream(_etalon));
            var typed = node as Asn1ObjectIdentifier;
            Assert.NotNull(typed);
            Assert.Equal("2.5.4.10", typed.Value);
        }

        [Fact]
        public void WriteTest() {
            var node = new Asn1ObjectIdentifier("2.5.4.10");
            var data = node.GetBytes();
            AreEqual(_etalon, data);
        }
    }
}
