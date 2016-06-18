using System.Windows.Controls;
using PeNet;

namespace PEditor
{
    /// <summary>
    /// Interaction logic for FileInfo.xaml
    /// </summary>
    public partial class FileInfo : UserControl
    {
        public FileInfo()
        {
            InitializeComponent();
        }

        public void SetFileInfo(PeFile peFile)
        {
            tbLocation.Text = peFile.FileLocation;
            tbSize.Text = $"{peFile.FileSize} Bytes";
            tbMD5.Text = peFile.MD5;
            tbSHA1.Text = peFile.SHA1;
            tbSHA256.Text = peFile.SHA256;
            tbImpHash.Text = peFile.ImpHash;
        }
    }
}
