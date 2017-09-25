using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class WIN_CERTIFICATE_Test
    {
        [Fact]
        public void WinCertificateConstructorWorks_Test()
        {
            var winCertifiacte = new WIN_CERTIFICATE(RawStructures.RawWinCertificate, 2);
            Assert.Equal((uint) 0x0000000b, winCertifiacte.dwLength);
            Assert.Equal((ushort) 0x5544, winCertifiacte.wRevision);
            Assert.Equal((ushort) 0x7766, winCertifiacte.wCertificateType);
            Assert.Equal((byte) 0x11, winCertifiacte.bCertificate[0]);
            Assert.Equal((byte) 0x22, winCertifiacte.bCertificate[1]);
            Assert.Equal((byte) 0x33, winCertifiacte.bCertificate[2]);
        }
    }
}