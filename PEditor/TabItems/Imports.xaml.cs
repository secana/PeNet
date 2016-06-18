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
    /// Interaction logic for Imports.xaml
    /// </summary>
    public partial class Imports : UserControl
    {
        public Imports()
        {
            InitializeComponent();
        }

        private void lbImportDlls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbImportFunctions.Items.Clear();

            if (e.AddedItems.Count == 0)
                return;

            dynamic selected = e.AddedItems[0];
            var functions = MainWindow.PeFile.ImportedFunctions.Where(x => x.DLL == selected.DLL);

            foreach (var function in functions)
            {
                lbImportFunctions.Items.Add(new { function.Name, function.Hint });
            }
        }
    }
}
