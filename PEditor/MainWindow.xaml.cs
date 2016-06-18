using System.Deployment.Application;
using System.Windows;
using Microsoft.Win32;
using PeNet;

namespace PEditor
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static PeFile _peFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            var file = openFileDialog.FileName;
            FileOpen(file);
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void FileOpen(string file)
        {
            // Set status bar location for the file.
            tbStatusBarLocation.Text = file;

            // Parse the PE file
            if (!PeFile.IsPEFile(file))
            {
                MessageBox.Show("Not a PE file.");
                return;
            }

            var peFile = new PeFile(file);
            _peFile = peFile;

            // Set all FileInfo fields.
            FileInfo.SetFileInfo(peFile);

            // Set the DOS header fields
            DosNtHeader.SetDosHeader(peFile);

            // Set the PE File fields
            DosNtHeader.SetNtHeader(peFile);

            // Set the File header
            FileHeader.SetFileHeader(peFile);

            // Set the Optional header
            OptionalHeader.SetOptionalHeader(peFile);

            // Set the imports.
            Imports.SetImports(peFile);

            // Set the exports.
            Exports.SetExports(peFile);

            // Set the resources.
            Resource.SetResources(peFile);

            // Set the sections.
            SectionHeaders.SetSections(peFile);

            // Set the Exception (only for x64)
            Exceptions.SetException(peFile);

            // Set the Relocations.
            Relocation.SetRelocations(peFile);

            // Set the Digital Signature information.
            Signature.SetDigSignature(peFile);

            // Set the Debug directory.
            DebugBoundImport.SetDebug(peFile);

            // Set the Bound Import directory.
            DebugBoundImport.SetBoundImport(peFile);
        }

        private void MenuHelp_Click(object sender, RoutedEventArgs e)
        {
            var version = "DEBUG";
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                version =
                    $"Your application name - v{ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4)}";
            }

            MessageBox.Show($"PEditor\nVersion {version}\nCopyright by Secana 2016", "About");
        }
    }
}