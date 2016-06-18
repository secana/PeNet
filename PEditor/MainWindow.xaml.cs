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
        private PeFile _peFile;

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
            tbBoundImportNumberOfModuleForwarderRefs.Text = string.Empty;
            tbBoundImportOffsetModuleName.Text = string.Empty;
            tbBoundImportTimeDateStamp.Text = string.Empty;

            if(peFile.ImageBoundImportDescriptor == null)
                return;

            // Set
            tbBoundImportNumberOfModuleForwarderRefs.Text =
                peFile.ImageBoundImportDescriptor.NumberOfModuleForwarderRefs.ToHexString();
            tbBoundImportOffsetModuleName.Text = peFile.ImageBoundImportDescriptor.OffsetModuleName.ToHexString();
            tbBoundImportTimeDateStamp.Text = peFile.ImageBoundImportDescriptor.TimeDateStamp.ToHexString();
        }

        private void SetDebug(PeFile peFile)
        {
            // Clean
            tbDebugCharacteristics.Text = string.Empty;
            tbDebugTimeDateStamp.Text = string.Empty;
            tbDebugMajorVersion.Text = string.Empty;
            tbDebugMinorVersion.Text = string.Empty;
            tbDebugType.Text = string.Empty;
            tbDebugSizeOfData.Text = string.Empty;
            tbDebugAddressOfRawData.Text = string.Empty;
            tbDebugPointerToRawData.Text = string.Empty;

            // Set
            tbDebugCharacteristics.Text = peFile.ImageDebugDirectory.Characteristics.ToHexString();
            tbDebugTimeDateStamp.Text = peFile.ImageDebugDirectory.TimeDateStamp.ToHexString();
            tbDebugMajorVersion.Text = peFile.ImageDebugDirectory.MajorVersion.ToHexString();
            tbDebugMinorVersion.Text = peFile.ImageDebugDirectory.MinorVersion.ToHexString();
            tbDebugType.Text = peFile.ImageDebugDirectory.Type.ToHexString();
            tbDebugSizeOfData.Text = peFile.ImageDebugDirectory.SizeOfData.ToHexString();
            tbDebugAddressOfRawData.Text = peFile.ImageDebugDirectory.AddressOfRawData.ToHexString();
            tbDebugPointerToRawData.Text = peFile.ImageDebugDirectory.PointerToRawData.ToHexString();
        }

        private void SetDigSignature(PeFile peFile)
        {
            // Clear all fields.
            cbCertIsSigned.IsChecked = false;
            cbCertIsValid.IsChecked = false;
            cbCertIsValidChain.IsChecked = false;
            tbCertLength.Text = string.Empty;
            tbCertRevision.Text = string.Empty;
            tbCertType.Text = string.Empty;

            cbX509Archived.IsChecked = false;
            cbX509HasPrivateKey.IsChecked = false;
            tbX509FriendlyName.Text = string.Empty;
            tbX509Issuer.Text = string.Empty;
            tbX509Thumbprint.Text = string.Empty;
            tbX509Version.Text = string.Empty;
            tbX509NotAfter.Text = string.Empty;
            tbX509NotBefore.Text = string.Empty;
            tbX509SerialNumber.Text = string.Empty;
            tbX509SignatureAlgorithm.Text = string.Empty;
            tbX509Subject.Text = string.Empty;
            tbX509PrivateKey.Text = string.Empty;
            tbX509PublicKey.Text = string.Empty;
            tbX509Extensions.Text = string.Empty;
            tbX509CrlUrls.Text = string.Empty;

            if (!peFile.IsSigned)
                return;

            cbCertIsValid.IsChecked = Utility.IsSignatureValid(peFile.FileLocation);
            cbCertIsSigned.IsChecked = peFile.IsSigned;
            cbCertIsValidChain.IsChecked = peFile.IsValidCertChain(true);
            tbCertLength.Text = peFile.WinCertificate.dwLength.ToHexString();
            tbCertRevision.Text = peFile.WinCertificate.wRevision.ToHexString();
            tbCertType.Text = peFile.WinCertificate.wCertificateType.ToHexString();

            cbX509Archived.IsChecked = peFile.PKCS7.Archived;
            cbX509HasPrivateKey.IsChecked = peFile.PKCS7.HasPrivateKey;
            tbX509FriendlyName.Text = peFile.PKCS7.FriendlyName;
            tbX509Issuer.Text = peFile.PKCS7.Issuer.Replace(", ", "\n");
            tbX509Thumbprint.Text = peFile.PKCS7.Thumbprint;
            tbX509Version.Text = peFile.PKCS7.Version.ToString();
            tbX509NotBefore.Text = peFile.PKCS7.NotBefore.ToLongDateString();
            tbX509NotAfter.Text = peFile.PKCS7.NotAfter.ToLongDateString();
            tbX509SerialNumber.Text = peFile.PKCS7.SerialNumber;
            tbX509SignatureAlgorithm.Text = peFile.PKCS7.SignatureAlgorithm.FriendlyName;
            tbX509Subject.Text = peFile.PKCS7.Subject.Replace(", ", "\n");
            tbX509PublicKey.Text = peFile.PKCS7.PublicKey.EncodedKeyValue.Format(true);
            tbX509PrivateKey.Text = peFile.PKCS7.PrivateKey?.ToXmlString(false);

            foreach (var x509Extension in peFile.PKCS7.Extensions)
            {
                tbX509Extensions.Text += $"{x509Extension.Format(true)}\n";
            }

            foreach (var url in peFile.GetCrlUrlList().Urls)
            {
                tbX509CrlUrls.Text += $"{url}\n";
            }
        }

        private void SetRelocations(PeFile peFile)
        {
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

        private void SetSections(PeFile peFile)
        {
            var num = 1;
            foreach (var sec in peFile.ImageSectionHeaders)
            {
                var flags = string.Join(", ", Utility.ResolveSectionFlags(sec.Characteristics));
                dgSections.Items.Add(new
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

        private void SetExports(PeFile peFile)
        {
            lbExports.Items.Clear();

            if (peFile.ExportedFunctions == null)
                return;

            foreach (var export in peFile.ExportedFunctions)
            {
                lbExports.Items.Add(new {export.Name, export.Ordinal, RVA = $"0x{export.Address.ToString("X")}"});
            }
        }

        private void SetImports(PeFile peFile)
        {
            lbImportDlls.Items.Clear();

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
                lbImportDlls.Items.Add(new {DLL = kv.Key});
            }
        }

        private void SetFileInfo(PeFile peFile)
        {
            tbLocation.Text = peFile.FileLocation;
            tbSize.Text = $"{peFile.FileSize} Bytes";
            tbMD5.Text = peFile.MD5;
            tbSHA1.Text = peFile.SHA1;
            tbSHA256.Text = peFile.SHA256;
            tbImpHash.Text = peFile.ImpHash;
        }

        private void SetDosHeader(PeFile peFile)
        {
            var magic = peFile.ImageDosHeader.e_magic;

            tbe_magic.Text = magic == 0x5A4D ? $"{magic.ToHexString()} <-> MZ" : magic.ToHexString();
            tbe_cblp.Text = peFile.ImageDosHeader.e_cblp.ToHexString();
            tbe_cp.Text = peFile.ImageDosHeader.e_cp.ToHexString();
            tbe_crlc.Text = peFile.ImageDosHeader.e_crlc.ToHexString();
            tbe_cparhdr.Text = peFile.ImageDosHeader.e_cparhdr.ToHexString();
            tbe_minalloc.Text = peFile.ImageDosHeader.e_minalloc.ToHexString();
            tbe_maxalloc.Text = peFile.ImageDosHeader.e_maxalloc.ToHexString();
            tbe_ss.Text = peFile.ImageDosHeader.e_ss.ToHexString();
            tbe_sp.Text = peFile.ImageDosHeader.e_sp.ToHexString();
            tbe_csum.Text = peFile.ImageDosHeader.e_csum.ToHexString();
            tbe_ip.Text = peFile.ImageDosHeader.e_ip.ToHexString();
            tbe_cs.Text = peFile.ImageDosHeader.e_cs.ToHexString();
            tbe_lfarlc.Text = peFile.ImageDosHeader.e_lfarlc.ToHexString();
            tbe_ovno.Text = peFile.ImageDosHeader.e_ovno.ToHexString();
            tbe_res.Text = peFile.ImageDosHeader.e_res.ToHexString();
            tbe_oemid.Text = peFile.ImageDosHeader.e_oemid.ToHexString();
            tbe_oeminfo.Text = peFile.ImageDosHeader.e_oeminfo.ToHexString();
            tbe_res2.Text = peFile.ImageDosHeader.e_res2.ToHexString();
            tbe_lfanew.Text = peFile.ImageDosHeader.e_lfanew.ToHexString();
        }

        private void SetNtHeader(PeFile peFile)
        {
            tbSignature.Text = peFile.ImageNtHeaders.Signature.ToHexString();
        }

        private void SetFileHeader(PeFile peFile)
        {
            var fileHeader = peFile.ImageNtHeaders.FileHeader;
            var machine = fileHeader.Machine;
            var characteristics = fileHeader.Characteristics;

            tbMachine.Text = $"{machine.ToHexString()} <-> {Utility.ResolveTargetMachine(machine)}";
            tbNumberOfSections.Text = fileHeader.NumberOfSections.ToHexString();
            tbTimeDateStamp.Text = fileHeader.TimeDateStamp.ToHexString();
            tbPointerToSymbolTable.Text = fileHeader.PointerToSymbolTable.ToHexString();
            tbNumberOfSymbols.Text = fileHeader.NumberOfSymbols.ToHexString();
            tbSizeOfOptionalHeader.Text = fileHeader.SizeOfOptionalHeader.ToHexString();
            tbCharacteristics.Text =
                $"{characteristics.ToHexString()}\n\n{Utility.ResolveFileCharacteristics(characteristics)}";
        }

        private void SetOptionalHeader(PeFile peFile)
        {
            var oh = peFile.ImageNtHeaders.OptionalHeader;

            tbMagic.Text = oh.Magic.ToHexString();
            tbMajorLinkerVersion.Text = Utility.ToHexString(oh.MajorLinkerVersion);
            tbMinorLinkerVersion.Text = Utility.ToHexString(oh.MinorLinkerVersion);
            tbSizeOfCode.Text = oh.SizeOfCode.ToHexString();
            tbSizeOfInitializedData.Text = oh.SizeOfInitializedData.ToHexString();
            tbSizeOfUninitializedData.Text = oh.SizeOfUninitializedData.ToHexString();
            tbAddressOfEntryPoint.Text = oh.AddressOfEntryPoint.ToHexString();
            tbBaseOfCode.Text = oh.BaseOfCode.ToHexString();
            tbBaseOfData.Text = oh.BaseOfData.ToHexString();
            tbImageBase.Text = oh.ImageBase.ToHexString();
            tbSectionAlignment.Text = oh.SectionAlignment.ToHexString();
            tbFileAlignment.Text = oh.FileAlignment.ToHexString();
            tbMajorOperatingSystemVersion.Text = oh.MajorOperatingSystemVersion.ToHexString();
            tbMinorOperatingSystemVersion.Text = oh.MinorOperatingSystemVersion.ToHexString();
            tbMajorImageVersion.Text = oh.MajorImageVersion.ToHexString();
            tbMinorImageVersion.Text = oh.MinorImageVersion.ToHexString();
            tbMajorSubsystemVersion.Text = oh.MajorSubsystemVersion.ToHexString();
            tbMinorSubsystemVersion.Text = oh.MinorSubsystemVersion.ToHexString();
            tbWin32VersionValue.Text = oh.Win32VersionValue.ToHexString();
            tbSizeOfImage.Text = oh.SizeOfImage.ToHexString();
            tbSizeOfHeaders.Text = oh.SizeOfHeaders.ToHexString();
            tbCheckSum.Text = oh.CheckSum.ToHexString();
            tbSubsystem.Text = oh.Subsystem.ToHexString();
            tbDllCharacteristics.Text = oh.DllCharacteristics.ToHexString();
            tbSizeOfStackReserve.Text = oh.SizeOfStackReserve.ToHexString();
            tbSizeOfStackCommit.Text = oh.SizeOfStackCommit.ToHexString();
            tbSizeOfHeapReserve.Text = oh.SizeOfHeapReserve.ToHexString();
            tbSizeOfHeapCommit.Text = oh.SizeOfHeapCommit.ToHexString();
            tbLoaderFlags.Text = oh.LoaderFlags.ToHexString();
            tbNumberOfRvaAndSizes.Text = oh.NumberOfRvaAndSizes.ToHexString();
        }

        private void SetException(PeFile peFile)
        {
            lbRuntimeFunctions.Items.Clear();

            if (peFile.Is32Bit || peFile.RuntimeFunctions == null)
                return;

            foreach (var rt in peFile.RuntimeFunctions)
            {
                lbRuntimeFunctions.Items.Add(new
                {
                    FunctionStart = rt.FunctionStart.ToHexString(),
                    FunctionEnd = rt.FunctionEnd.ToHexString(),
                    UnwindInfo = rt.UnwindInfo.ToHexString()
                });
            }
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
                lbImportFunctions.Items.Add(new {function.Name, function.Hint});
            }
        }

        private void lbRuntimeFunctions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null) return;
            dynamic selected = listBox.SelectedItem;
            if(selected == null) return;

            // Convert string of format 0x... to an integer.
            var funcStart = Utility.ToIntFromHexString(selected.FunctionStart);
            var funcEnd = Utility.ToIntFromHexString(selected.FunctionEnd);
            var uw = Utility.ToIntFromHexString(selected.UnwindInfo);

            // Find the RUNTIME_FUNCTION which was selected.
            var rt = _peFile.RuntimeFunctions.First(x => x.FunctionStart == funcStart
                                                         && x.FunctionEnd == funcEnd
                                                         && x.UnwindInfo == uw
                );

            // Set the UNWIND_INFO properties.
            tbUIVersion.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.Version);
            tbUIFlags.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.Flags);
            tbUISizeOfProlog.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.SizeOfProlog);
            tbUICountOfCodes.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.CountOfCodes);
            tbUIFrameRegister.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.FrameRegister);
            tbUIFrameOffset.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.FrameOffset);
            tbUIExHandlerFuncEntry.Text = rt.ResolvedUnwindInfo.ExceptionHandler.ToHexString();
            // TODO: display excetption data as a hex array.
            //tbUIExData.Text = string.Format("", rt.ResolvedUnwindInfo.ExceptionData);

            // Set the UNWIND_CODE structures for the UNWIND_INFO
            lbUnwindCode.Items.Clear();
            foreach (var uc in rt.ResolvedUnwindInfo.UnwindCode)
            {
                lbUnwindCode.Items.Add(new
                {
                    CodeOffset = Utility.ToHexString(uc.CodeOffset),
                    UnwindOp = Utility.ToHexString(uc.UnwindOp),
                    FrameOffset = uc.FrameOffset.ToHexString()
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