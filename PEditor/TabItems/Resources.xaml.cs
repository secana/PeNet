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
using PeNet.Structures;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for Resources.xaml
    /// </summary>
    public partial class Resources : UserControl
    {
        public Resources()
        {
            InitializeComponent();
        }


        private void Resources_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Clear all fields.
            tbOffsetToData.Clear();
            tbSize1.Clear();
            tbCodePage.Clear();
            tbReserved.Clear();
            tbResource.Clear();

            // Get the resource data entry. If no data entry is give, return.
            var tree = sender as TreeView;
            var directoryEntry = (tree?.SelectedItem as MyTreeViewItem<IMAGE_RESOURCE_DIRECTORY_ENTRY>)?.MyItem;
            if (directoryEntry?.ResourceDataEntry == null)
                return;

            // Set all values.
            tbOffsetToData.Text = directoryEntry.ResourceDataEntry.OffsetToData.ToHexString();
            tbSize1.Text = directoryEntry.ResourceDataEntry.Size1.ToHexString();
            tbCodePage.Text = directoryEntry.ResourceDataEntry.CodePage.ToHexString();
            tbReserved.Text = directoryEntry.ResourceDataEntry.Reserved.ToHexString();

            // Build the hex output
            var rawOffset = directoryEntry.ResourceDataEntry.OffsetToData.RVAtoFileMapping(MainWindow.PeFile.ImageSectionHeaders
                );

            tbResource.Text = string.Join(" ",
                MainWindow.PeFile.Buff.ToHexString(rawOffset, directoryEntry.ResourceDataEntry.Size1));
        }
    }
}
