using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using PeNet;
using PeNet.Structures;

namespace PEditor
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static PeFile PeFile;

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
            PeFile = peFile;

            // Set all FileInfo fields.
            SetFileInfo(peFile);

            // Set the DOS header fields
            SetDosHeader(peFile);

            // Set the PE File fields
            SetNtHeader(peFile);

            // Set the File header
            SetFileHeader(peFile);

            // Set the Optional header
            SetOptionalHeader(peFile);

            // Set the imports.
            SetImports(peFile);

            // Set the exports.
            SetExports(peFile);

            // Set the resources.
            SetResources(peFile);

            // Set the sections.
            SetSections(peFile);

            // Set the Exception (only for x64)
            SetException(peFile);

            // Set the Relocations.
            SetRelocations(peFile);

            // Set the Digital Signature information.
            SetDigSignature(peFile);

            // Set the Debug directory.
            SetDebug(peFile);

            // Set the Bound Import directory.
            SetBoundImport(peFile);
        }

        private void SetBoundImport(PeFile peFile)
        {
            // Clean
            DebugBoundImport.tbBoundImportNumberOfModuleForwarderRefs.Text = string.Empty;
            DebugBoundImport.tbBoundImportOffsetModuleName.Text = string.Empty;
            DebugBoundImport.tbBoundImportTimeDateStamp.Text = string.Empty;

            if(peFile.ImageBoundImportDescriptor == null)
                return;

            // Set
            DebugBoundImport.tbBoundImportNumberOfModuleForwarderRefs.Text =
                peFile.ImageBoundImportDescriptor.NumberOfModuleForwarderRefs.ToHexString();
            DebugBoundImport.tbBoundImportOffsetModuleName.Text = peFile.ImageBoundImportDescriptor.OffsetModuleName.ToHexString();
            DebugBoundImport.tbBoundImportTimeDateStamp.Text = peFile.ImageBoundImportDescriptor.TimeDateStamp.ToHexString();
        }

        private void SetDebug(PeFile peFile)
        {
            // Clean
            DebugBoundImport.tbDebugCharacteristics.Text = string.Empty;
            DebugBoundImport.tbDebugTimeDateStamp.Text = string.Empty;
            DebugBoundImport.tbDebugMajorVersion.Text = string.Empty;
            DebugBoundImport.tbDebugMinorVersion.Text = string.Empty;
            DebugBoundImport.tbDebugType.Text = string.Empty;
            DebugBoundImport.tbDebugSizeOfData.Text = string.Empty;
            DebugBoundImport.tbDebugAddressOfRawData.Text = string.Empty;
            DebugBoundImport.tbDebugPointerToRawData.Text = string.Empty;

            // Set
            DebugBoundImport.tbDebugCharacteristics.Text = peFile.ImageDebugDirectory.Characteristics.ToHexString();
            DebugBoundImport.tbDebugTimeDateStamp.Text = peFile.ImageDebugDirectory.TimeDateStamp.ToHexString();
            DebugBoundImport.tbDebugMajorVersion.Text = peFile.ImageDebugDirectory.MajorVersion.ToHexString();
            DebugBoundImport.tbDebugMinorVersion.Text = peFile.ImageDebugDirectory.MinorVersion.ToHexString();
            DebugBoundImport.tbDebugType.Text = peFile.ImageDebugDirectory.Type.ToHexString();
            DebugBoundImport.tbDebugSizeOfData.Text = peFile.ImageDebugDirectory.SizeOfData.ToHexString();
            DebugBoundImport.tbDebugAddressOfRawData.Text = peFile.ImageDebugDirectory.AddressOfRawData.ToHexString();
            DebugBoundImport.tbDebugPointerToRawData.Text = peFile.ImageDebugDirectory.PointerToRawData.ToHexString();
        }

        private void SetDigSignature(PeFile peFile)
        {
            // Clear all fields.
            Signature.cbCertIsSigned.IsChecked = false;
            Signature.cbCertIsValid.IsChecked = false;
            Signature.cbCertIsValidChain.IsChecked = false;
            Signature.tbCertLength.Text = string.Empty;
            Signature.tbCertRevision.Text = string.Empty;
            Signature.tbCertType.Text = string.Empty;

            Signature.cbX509Archived.IsChecked = false;
            Signature.cbX509HasPrivateKey.IsChecked = false;
            Signature.tbX509FriendlyName.Text = string.Empty;
            Signature.tbX509Issuer.Text = string.Empty;
            Signature.tbX509Thumbprint.Text = string.Empty;
            Signature.tbX509Version.Text = string.Empty;
            Signature.tbX509NotAfter.Text = string.Empty;
            Signature.tbX509NotBefore.Text = string.Empty;
            Signature.tbX509SerialNumber.Text = string.Empty;
            Signature.tbX509SignatureAlgorithm.Text = string.Empty;
            Signature.tbX509Subject.Text = string.Empty;
            Signature.tbX509PrivateKey.Text = string.Empty;
            Signature.tbX509PublicKey.Text = string.Empty;
            Signature.tbX509Extensions.Text = string.Empty;
            Signature.tbX509CrlUrls.Text = string.Empty;

            if (!peFile.IsSigned)
                return;

            Signature.cbCertIsValid.IsChecked = Utility.IsSignatureValid(peFile.FileLocation);
            Signature.cbCertIsSigned.IsChecked = peFile.IsSigned;
            Signature.cbCertIsValidChain.IsChecked = peFile.IsValidCertChain(true);
            Signature.tbCertLength.Text = peFile.WinCertificate.dwLength.ToHexString();
            Signature.tbCertRevision.Text = peFile.WinCertificate.wRevision.ToHexString();
            Signature.tbCertType.Text = peFile.WinCertificate.wCertificateType.ToHexString();

            Signature.cbX509Archived.IsChecked = peFile.PKCS7.Archived;
            Signature.cbX509HasPrivateKey.IsChecked = peFile.PKCS7.HasPrivateKey;
            Signature.tbX509FriendlyName.Text = peFile.PKCS7.FriendlyName;
            Signature.tbX509Issuer.Text = peFile.PKCS7.Issuer.Replace(", ", "\n");
            Signature.tbX509Thumbprint.Text = peFile.PKCS7.Thumbprint;
            Signature.tbX509Version.Text = peFile.PKCS7.Version.ToString();
            Signature.tbX509NotBefore.Text = peFile.PKCS7.NotBefore.ToLongDateString();
            Signature.tbX509NotAfter.Text = peFile.PKCS7.NotAfter.ToLongDateString();
            Signature.tbX509SerialNumber.Text = peFile.PKCS7.SerialNumber;
            Signature.tbX509SignatureAlgorithm.Text = peFile.PKCS7.SignatureAlgorithm.FriendlyName;
            Signature.tbX509Subject.Text = peFile.PKCS7.Subject.Replace(", ", "\n");
            Signature.tbX509PublicKey.Text = peFile.PKCS7.PublicKey.EncodedKeyValue.Format(true);
            Signature.tbX509PrivateKey.Text = peFile.PKCS7.PrivateKey?.ToXmlString(false);

            foreach (var x509Extension in peFile.PKCS7.Extensions)
            {
                Signature.tbX509Extensions.Text += $"{x509Extension.Format(true)}\n";
            }

            foreach (var url in peFile.GetCrlUrlList().Urls)
            {
                Signature.tbX509CrlUrls.Text += $"{url}\n";
            }
        }

        private void SetRelocations(PeFile peFile)
        {
            Relocation.lbRelocationEntries.Items.Clear();
            Relocation.lbRelocTypeOffsets.Items.Clear();

            if (!peFile.HasValidRelocDir)
                return;

            foreach (var reloc in peFile.ImageRelocationDirectory)
            {
                Relocation.lbRelocationEntries.Items.Add(new
                {
                    VirtualAddress = reloc.VirtualAddress.ToHexString(),
                    SizeOfBlock = reloc.SizeOfBlock.ToHexString()
                });
            }
        }

        private void SetSections(PeFile peFile)
        {
            var num = 1;
            foreach (var sec in peFile.ImageSectionHeaders)
            {
                var flags = string.Join(", ", Utility.ResolveSectionFlags(sec.Characteristics));
                SectionHeaders.dgSections.Items.Add(new
                {
                    Number = num,
                    Name = Utility.ResolveSectionName(sec.Name),
                    VSize = sec.VirtualSize.ToHexString(),
                    VAddress = sec.VirtualAddress.ToHexString(),
                    PSize = sec.SizeOfRawData.ToHexString(),
                    PAddress = sec.PhysicalAddress.ToHexString(),
                    Flags = sec.Characteristics.ToHexString(),
                    RFlags = flags
                });
                num++;
            }
        }

        private void SetResources(PeFile peFile)
        {
            // Clear the tree.
            Resource.tbResources.Items.Clear();


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

            Resource.tbResources.Items.Add(root);
        }

        private void SetExports(PeFile peFile)
        {
            Exports.lbExports.Items.Clear();

            if (peFile.ExportedFunctions == null)
                return;

            foreach (var export in peFile.ExportedFunctions)
            {
                Exports.lbExports.Items.Add(new {export.Name, export.Ordinal, RVA = $"0x{export.Address.ToString("X")}"});
            }
        }

        private void SetImports(PeFile peFile)
        {
            Imports.lbImportDlls.Items.Clear();

            if (peFile.ImportedFunctions == null)
                return;

            var dllNames = peFile.ImportedFunctions?.Select(x => x.DLL).Distinct();
            var dllFunctions = new Dictionary<string, IEnumerable<ImportFunction>>();

            foreach (var dllName in dllNames)
            {
                var functions = peFile.ImportedFunctions.Where(x => x.DLL == dllName);
                dllFunctions.Add(dllName, functions);
            }

            foreach (var kv in dllFunctions)
            {
                Imports.lbImportDlls.Items.Add(new {DLL = kv.Key});
            }
        }

        private void SetFileInfo(PeFile peFile)
        {
            FileInfo.tbLocation.Text = peFile.FileLocation;
            FileInfo.tbSize.Text = $"{peFile.FileSize} Bytes";
            FileInfo.tbMD5.Text = peFile.MD5;
            FileInfo.tbSHA1.Text = peFile.SHA1;
            FileInfo.tbSHA256.Text = peFile.SHA256;
            FileInfo.tbImpHash.Text = peFile.ImpHash;
        }

        private void SetDosHeader(PeFile peFile)
        {
            var magic = peFile.ImageDosHeader.e_magic;

            DosNtHeader.tbe_magic.Text = magic == 0x5A4D ? $"{magic.ToHexString()} <-> MZ" : magic.ToHexString();
            DosNtHeader.tbe_cblp.Text = peFile.ImageDosHeader.e_cblp.ToHexString();
            DosNtHeader.tbe_cp.Text = peFile.ImageDosHeader.e_cp.ToHexString();
            DosNtHeader.tbe_crlc.Text = peFile.ImageDosHeader.e_crlc.ToHexString();
            DosNtHeader.tbe_cparhdr.Text = peFile.ImageDosHeader.e_cparhdr.ToHexString();
            DosNtHeader.tbe_minalloc.Text = peFile.ImageDosHeader.e_minalloc.ToHexString();
            DosNtHeader.tbe_maxalloc.Text = peFile.ImageDosHeader.e_maxalloc.ToHexString();
            DosNtHeader.tbe_ss.Text = peFile.ImageDosHeader.e_ss.ToHexString();
            DosNtHeader.tbe_sp.Text = peFile.ImageDosHeader.e_sp.ToHexString();
            DosNtHeader.tbe_csum.Text = peFile.ImageDosHeader.e_csum.ToHexString();
            DosNtHeader.tbe_ip.Text = peFile.ImageDosHeader.e_ip.ToHexString();
            DosNtHeader.tbe_cs.Text = peFile.ImageDosHeader.e_cs.ToHexString();
            DosNtHeader.tbe_lfarlc.Text = peFile.ImageDosHeader.e_lfarlc.ToHexString();
            DosNtHeader.tbe_ovno.Text = peFile.ImageDosHeader.e_ovno.ToHexString();
            DosNtHeader.tbe_res.Text = peFile.ImageDosHeader.e_res.ToHexString();
            DosNtHeader.tbe_oemid.Text = peFile.ImageDosHeader.e_oemid.ToHexString();
            DosNtHeader.tbe_oeminfo.Text = peFile.ImageDosHeader.e_oeminfo.ToHexString();
            DosNtHeader.tbe_res2.Text = peFile.ImageDosHeader.e_res2.ToHexString();
            DosNtHeader.tbe_lfanew.Text = peFile.ImageDosHeader.e_lfanew.ToHexString();
        }

        private void SetNtHeader(PeFile peFile)
        {
            DosNtHeader.tbSignature.Text = peFile.ImageNtHeaders.Signature.ToHexString();
        }

        private void SetFileHeader(PeFile peFile)
        {
            var fileHeader = peFile.ImageNtHeaders.FileHeader;
            var machine = fileHeader.Machine;
            var characteristics = fileHeader.Characteristics;

            FileHeader.tbMachine.Text = $"{machine.ToHexString()} <-> {Utility.ResolveTargetMachine(machine)}";
            FileHeader.tbNumberOfSections.Text = fileHeader.NumberOfSections.ToHexString();
            FileHeader.tbTimeDateStamp.Text = fileHeader.TimeDateStamp.ToHexString();
            FileHeader.tbPointerToSymbolTable.Text = fileHeader.PointerToSymbolTable.ToHexString();
            FileHeader.tbNumberOfSymbols.Text = fileHeader.NumberOfSymbols.ToHexString();
            FileHeader.tbSizeOfOptionalHeader.Text = fileHeader.SizeOfOptionalHeader.ToHexString();
            FileHeader.tbCharacteristics.Text =
                $"{characteristics.ToHexString()}\n\n{Utility.ResolveFileCharacteristics(characteristics)}";
        }

        private void SetOptionalHeader(PeFile peFile)
        {
            var oh = peFile.ImageNtHeaders.OptionalHeader;

            OptionalHeader.tbMagic.Text = oh.Magic.ToHexString();
            OptionalHeader.tbMajorLinkerVersion.Text = Utility.ToHexString(oh.MajorLinkerVersion);
            OptionalHeader.tbMinorLinkerVersion.Text = Utility.ToHexString(oh.MinorLinkerVersion);
            OptionalHeader.tbSizeOfCode.Text = oh.SizeOfCode.ToHexString();
            OptionalHeader.tbSizeOfInitializedData.Text = oh.SizeOfInitializedData.ToHexString();
            OptionalHeader.tbSizeOfUninitializedData.Text = oh.SizeOfUninitializedData.ToHexString();
            OptionalHeader.tbAddressOfEntryPoint.Text = oh.AddressOfEntryPoint.ToHexString();
            OptionalHeader.tbBaseOfCode.Text = oh.BaseOfCode.ToHexString();
            OptionalHeader.tbBaseOfData.Text = oh.BaseOfData.ToHexString();
            OptionalHeader.tbImageBase.Text = oh.ImageBase.ToHexString();
            OptionalHeader.tbSectionAlignment.Text = oh.SectionAlignment.ToHexString();
            OptionalHeader.tbFileAlignment.Text = oh.FileAlignment.ToHexString();
            OptionalHeader.tbMajorOperatingSystemVersion.Text = oh.MajorOperatingSystemVersion.ToHexString();
            OptionalHeader.tbMinorOperatingSystemVersion.Text = oh.MinorOperatingSystemVersion.ToHexString();
            OptionalHeader.tbMajorImageVersion.Text = oh.MajorImageVersion.ToHexString();
            OptionalHeader.tbMinorImageVersion.Text = oh.MinorImageVersion.ToHexString();
            OptionalHeader.tbMajorSubsystemVersion.Text = oh.MajorSubsystemVersion.ToHexString();
            OptionalHeader.tbMinorSubsystemVersion.Text = oh.MinorSubsystemVersion.ToHexString();
            OptionalHeader.tbWin32VersionValue.Text = oh.Win32VersionValue.ToHexString();
            OptionalHeader.tbSizeOfImage.Text = oh.SizeOfImage.ToHexString();
            OptionalHeader.tbSizeOfHeaders.Text = oh.SizeOfHeaders.ToHexString();
            OptionalHeader.tbCheckSum.Text = oh.CheckSum.ToHexString();
            OptionalHeader.tbSubsystem.Text = oh.Subsystem.ToHexString();
            OptionalHeader.tbDllCharacteristics.Text = oh.DllCharacteristics.ToHexString();
            OptionalHeader.tbSizeOfStackReserve.Text = oh.SizeOfStackReserve.ToHexString();
            OptionalHeader.tbSizeOfStackCommit.Text = oh.SizeOfStackCommit.ToHexString();
            OptionalHeader.tbSizeOfHeapReserve.Text = oh.SizeOfHeapReserve.ToHexString();
            OptionalHeader.tbSizeOfHeapCommit.Text = oh.SizeOfHeapCommit.ToHexString();
            OptionalHeader.tbLoaderFlags.Text = oh.LoaderFlags.ToHexString();
            OptionalHeader.tbNumberOfRvaAndSizes.Text = oh.NumberOfRvaAndSizes.ToHexString();
        }

        private void SetException(PeFile peFile)
        {
            Exceptions.lbRuntimeFunctions.Items.Clear();

            if (peFile.Is32Bit || peFile.RuntimeFunctions == null)
                return;

            foreach (var rt in peFile.RuntimeFunctions)
            {
                Exceptions.lbRuntimeFunctions.Items.Add(new
                {
                    FunctionStart = rt.FunctionStart.ToHexString(),
                    FunctionEnd = rt.FunctionEnd.ToHexString(),
                    UnwindInfo = rt.UnwindInfo.ToHexString()
                });
            }
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