using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
