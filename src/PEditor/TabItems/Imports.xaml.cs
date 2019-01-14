using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using PeNet;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for Imports.xaml
    /// </summary>
    public partial class Imports : UserControl
    {
        private PeFile _peFile;

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
            var functions = _peFile.ImportedFunctions.Where(x => x.DLL == selected.DLL);

            foreach (var function in functions)
            {
                lbImportFunctions.Items.Add(new { function.Name, function.Hint });
            }
        }


        public void SetImports(PeFile peFile)
        {
            _peFile = peFile;
            lbImportDlls.Items.Clear();

            if (peFile.ImportedFunctions == null)
                return;

            var dllNames = peFile.ImportedFunctions?.Select(x => x.DLL).Distinct();
            var dllFunctions = new Dictionary<string, IEnumerable<ImportFunction>>();

            foreach (var dllName in peFile.ImportedFunctions?.Select(x => x.DLL).Distinct())
            {
                lbImportDlls.Items.Add(new { DLL = dllName });
            }
        }
    }
}
