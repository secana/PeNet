using System.Linq;
using System.Windows.Controls;
using PeNet;
using PeNet.Utilities;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for Relocation.xaml
    /// </summary>
    public partial class Relocation : UserControl
    {
        private PeFile _peFile;
        public Relocation()
        {
            InitializeComponent();
        }

        private void lbRelocationEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbRelocTypeOffsets.Items.Clear();
            var listBox = sender as ListBox;
            if (listBox == null) return;
            dynamic selected = listBox.SelectedItem;
            if (selected == null)
                return;

            var reloc =
                _peFile.ImageRelocationDirectory.First(
                    x => x.VirtualAddress == selected.VirtualAddress.ToIntFromHexString());

            foreach (var to in reloc.TypeOffsets)
            {
                lbRelocTypeOffsets.Items.Add(new
                {
                    Type = to.Type.ToHexString(),
                    Offset = to.Offset.ToHexString()
                });
            }
        }


        public void SetRelocations(PeFile peFile)
        {
            _peFile = peFile;
            lbRelocationEntries.Items.Clear();
            lbRelocTypeOffsets.Items.Clear();

            if (!peFile.HasValidRelocDir)
                return;

            foreach (var reloc in peFile.ImageRelocationDirectory)
            {
                lbRelocationEntries.Items.Add(new
                {
                    VirtualAddress = reloc.VirtualAddress.ToHexString(),
                    SizeOfBlock = reloc.SizeOfBlock.ToHexString()
                });
            }
        }

    }
}
