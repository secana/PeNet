using System.Windows.Controls;
using PeNet;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for DirectoryView.xaml
    /// </summary>
    public partial class DirectoryView : UserControl
    {
        public DirectoryView()
        {
            InitializeComponent();
        }

        public void SetDirectoryView(PeFile peFile)
        {
            for (var i = 0; i < peFile.ImageNtHeaders.OptionalHeader.NumberOfRvaAndSizes; i++)
            {
                dgDirectories.Items.Add(new
                {
                    Number = i,
                    Name = GetDirectoryNameByIndex(i),
                    VAddress = peFile.ImageNtHeaders.OptionalHeader.DataDirectory[i].VirtualAddress.ToHexString(),
                    VSize = peFile.ImageNtHeaders.OptionalHeader.DataDirectory[i].Size.ToHexString()
                });
            }
        }

        private string GetDirectoryNameByIndex(int index)
        {
            return ((PeNet.Constants.DataDirectoryIndex) index).ToString();
        }
    }
}
