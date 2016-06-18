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
    /// Interaction logic for Relocation.xaml
    /// </summary>
    public partial class Relocation : UserControl
    {
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
                MainWindow.PeFile.ImageRelocationDirectory.First(
                    x => x.VirtualAddress == Utility.ToIntFromHexString(selected.VirtualAddress));

            foreach (var to in reloc.TypeOffsets)
            {
                lbRelocTypeOffsets.Items.Add(new
                {
                    Type = Utility.ToHexString(to.Type),
                    Offset = to.Offset.ToHexString()
                });
            }
        }
    }
}
