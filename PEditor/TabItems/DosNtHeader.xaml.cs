using System.Windows.Controls;
using PeNet;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for DosNtHeader.xaml
    /// </summary>
    public partial class DosNtHeader : UserControl
    {
        public DosNtHeader()
        {
            InitializeComponent();
        }

        public void SetDosHeader(PeFile peFile)
        {
            var magic = peFile.ImageDosHeader.e_magic;

            tbe_magic.Text = magic == 0x5A4D ? $"{magic.ToHexString()} <-> MZ" : magic.ToHexString();
            tbe_cblp.Text = peFile.ImageDosHeader.e_cblp.ToHexString();
            tbe_cp.Text = peFile.ImageDosHeader.e_cp.ToHexString();
            tbe_crlc.Text = peFile.ImageDosHeader.e_crlc.ToHexString();
            tbe_cparhdr.Text = peFile.ImageDosHeader.e_cparhdr.ToHexString();
            tbe_minalloc.Text = peFile.ImageDosHeader.e_minalloc.ToHexString();
            tbe_maxalloc.Text = peFile.ImageDosHeader.e_maxalloc.ToHexString();
            tbe_ss.Text = peFile.ImageDosHeader.e_ss.ToHexString();
            tbe_sp.Text = peFile.ImageDosHeader.e_sp.ToHexString();
            tbe_csum.Text = peFile.ImageDosHeader.e_csum.ToHexString();
            tbe_ip.Text = peFile.ImageDosHeader.e_ip.ToHexString();
            tbe_cs.Text = peFile.ImageDosHeader.e_cs.ToHexString();
            tbe_lfarlc.Text = peFile.ImageDosHeader.e_lfarlc.ToHexString();
            tbe_ovno.Text = peFile.ImageDosHeader.e_ovno.ToHexString();
            tbe_res.Text = peFile.ImageDosHeader.e_res.ToHexString();
            tbe_oemid.Text = peFile.ImageDosHeader.e_oemid.ToHexString();
            tbe_oeminfo.Text = peFile.ImageDosHeader.e_oeminfo.ToHexString();
            tbe_res2.Text = peFile.ImageDosHeader.e_res2.ToHexString();
            tbe_lfanew.Text = peFile.ImageDosHeader.e_lfanew.ToHexString();
        }

        public void SetNtHeader(PeFile peFile)
        {
            tbSignature.Text = peFile.ImageNtHeaders.Signature.ToHexString();
        }
    }
}
