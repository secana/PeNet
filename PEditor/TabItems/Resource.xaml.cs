using System.Windows;
using System.Windows.Controls;
using PeNet;
using PeNet.Structures;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for Resource.xaml
    /// </summary>
    public partial class Resource : UserControl
    {
        private PeFile _peFile;
        public Resource()
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
            var rawOffset = directoryEntry.ResourceDataEntry.OffsetToData.RVAtoFileMapping(_peFile.ImageSectionHeaders
                );

            tbResource.Text = string.Join(" ",
                _peFile.Buff.ToHexString(rawOffset, directoryEntry.ResourceDataEntry.Size1));
        }

        public void SetResources(PeFile peFile)
        {
            _peFile = peFile;
            // Clear the tree.
            tbResources.Items.Clear();


            // ROOT
            var rd = peFile.ImageResourceDirectory;

            if (rd == null)
                return;

            var root = new MyTreeViewItem<IMAGE_RESOURCE_DIRECTORY_ENTRY>(null)
            {
                Header = "Resource Directory"
            };

            // Type
            foreach (var de in rd.DirectoryEntries)
            {
                MyTreeViewItem<IMAGE_RESOURCE_DIRECTORY_ENTRY> item = null;
                if (de.IsIdEntry)
                {
                    item = new MyTreeViewItem<IMAGE_RESOURCE_DIRECTORY_ENTRY>(de)
                    {
                        Header = Utility.ResolveResourceId(de.ID)
                    };
                }
                else if (de.IsNamedEntry)
                {
                    item = new MyTreeViewItem<IMAGE_RESOURCE_DIRECTORY_ENTRY>(de)
                    {
                        Header = de.ResolvedName
                    };
                }

                // name/IDs
                foreach (var de2 in de.ResourceDirectory.DirectoryEntries)
                {
                    MyTreeViewItem<IMAGE_RESOURCE_DIRECTORY_ENTRY> item2 = null;
                    item2 = new MyTreeViewItem<IMAGE_RESOURCE_DIRECTORY_ENTRY>(de2)
                    {
                        Header = de2.ID.ToString()
                    };

                    foreach (var de3 in de2.ResourceDirectory.DirectoryEntries)
                    {
                        item2.Items.Add(new MyTreeViewItem<IMAGE_RESOURCE_DIRECTORY_ENTRY>(de3)
                        {
                            Header = de3.ID.ToHexString()
                        });
                    }

                    item?.Items.Add(item2);
                }

                root.Items.Add(item);
            }

            tbResources.Items.Add(root);
        }
    }
}
