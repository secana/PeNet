using System.IO;
using Xunit;

namespace Asn1.Tests
{
    public class Asn1Ia5StringTest : BaseTest {

        private static readonly byte[] _etalon = {
            0x16, 0x41, 0x53, 0x74, 0x61, 0x72, 0x74, 0x43, 0x6F, 0x6D, 0x20, 0x43, 0x6C, 0x61, 0x73, 0x73,
            0x20, 0x32, 0x20, 0x50, 0x72, 0x69, 0x6D, 0x61, 0x72, 0x79, 0x20, 0x49, 0x6E, 0x74, 0x65, 0x72,
            0x6D, 0x65, 0x64, 0x69, 0x61, 0x74, 0x65, 0x20, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x20, 0x53,
            0x69, 0x67, 0x6E, 0x69, 0x6E, 0x67, 0x20, 0x43, 0x65, 0x72, 0x74, 0x69, 0x66, 0x69, 0x63, 0x61,
            0x74, 0x65, 0x73 };

        [Fact]
        public void GenericTest()
        {
            var cert = File.ReadAllBytes(@"pidgin.pkcs7");
            var asn1 = Asn1Node.ReadNode(cert);
            Assert.NotNull(asn1);
        }

        [Fact]
        public void ReadTest()
        {
            var node = Asn1Node.ReadNode(new MemoryStream(_etalon));
            var typed = node as Asn1Ia5String;
            Assert.NotNull(typed);
            Assert.Equal("StartCom Class 2 Primary Intermediate Object Signing Certificates", typed.Value);
        }

        [Fact]
        public void WriteTest()
        {
            var node = new Asn1Ia5String("StartCom Class 2 Primary Intermediate Object Signing Certificates");
            var data = node.GetBytes();
            AreEqual(_etalon, data);
        }
     
    }
}