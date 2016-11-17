using System.Windows.Controls;
using PeNet;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for Exports.xaml
    /// </summary>
    public partial class Exports : UserControl
    {
        public Exports()
        {
            InitializeComponent();
        }


        public void SetExports(PeFile peFile)
        {
            lbExports.Items.Clear();

            if (peFile.ExportedFunctions == null)
                return;

            foreach (var export in peFile.ExportedFunctions)
            {
                lbExports.Items.Add(new { export.Name, export.Ordinal, RVA = $"0x{export.Address.ToString("X")}" });
            }
        }

    }
}
