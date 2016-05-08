using Microsoft.Win32;
using PeNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
            var openFileDialog = new OpenFileDialog()
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
            if(!PeFile.IsValidPEFile(file))
            {
                MessageBox.Show("Not a valid PE file.");
                return;
            }

            var peFile = new PeFile(file);
            _peFile = peFile;

            // Set all FileInfo fields.
            SetFileInfo(peFile);

            // Set signature info.
            SetSignature(peFile);

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
        }

        private void SetSections(PeFile peFile)
        {
            var num = 1;
            foreach(var sec in peFile.ImageSectionHeaders)
            {
                var flags = string.Join(", ", Utility.ResolveSectionFlags(sec.Characteristics));
                dgSections.Items.Add(new {
                    Number      = num,
                    Name        = Utility.ResolveSectionName(sec.Name),
                    VSize       = Utility.ToHexString(sec.VirtualSize),
                    VAddress    = Utility.ToHexString(sec.VirtualAddress),
                    PSize       = Utility.ToHexString(sec.SizeOfRawData),
                    PAddress    = Utility.ToHexString(sec.PhysicalAddress),
                    Flags       = Utility.ToHexString(sec.Characteristics)
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

            var root = new MyTreeViewItem<PeNet.Structures.IMAGE_RESOURCE_DIRECTORY_ENTRY>(null)
            {
                Header = "Resource Directory",
            };

            // Type
            foreach (var de in rd.DirectoryEntries)
            {
                MyTreeViewItem<PeNet.Structures.IMAGE_RESOURCE_DIRECTORY_ENTRY> item = null;
                if (de.IsIdEntry)
                {
                    item = new MyTreeViewItem<PeNet.Structures.IMAGE_RESOURCE_DIRECTORY_ENTRY>(de)
                    {
                        Header = Utility.ResolveResourceId(de.ID)
                    };
                }
                else if (de.IsNamedEntry)
                {
                    item = new MyTreeViewItem<PeNet.Structures.IMAGE_RESOURCE_DIRECTORY_ENTRY>(de)
                    {
                        Header = de.ResolvedName
                    };
                }

                // name/IDs
                foreach(var de2 in de.ResourceDirectory.DirectoryEntries)
                {
                    MyTreeViewItem<PeNet.Structures.IMAGE_RESOURCE_DIRECTORY_ENTRY> item2 = null;
                    item2 = new MyTreeViewItem<PeNet.Structures.IMAGE_RESOURCE_DIRECTORY_ENTRY>(de2)
                    {
                        Header = de2.ID.ToString()
                    };

                    foreach(var de3 in de2.ResourceDirectory.DirectoryEntries)
                    {
                        item2.Items.Add(new MyTreeViewItem<PeNet.Structures.IMAGE_RESOURCE_DIRECTORY_ENTRY>(de3)
                        {
                            Header = Utility.ToHexString(de3.ID)
                        });
                    }

                    item.Items.Add(item2);
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
            if (tree.SelectedItem == null)
                return;
            var directoryEntry = (tree.SelectedItem as MyTreeViewItem<PeNet.Structures.IMAGE_RESOURCE_DIRECTORY_ENTRY>).MyItem;
            if (directoryEntry?.ResourceDataEntry == null)
                return;

            // Set all values.
            tbOffsetToData.Text = Utility.ToHexString(directoryEntry.ResourceDataEntry.OffsetToData);
            tbSize1.Text = Utility.ToHexString(directoryEntry.ResourceDataEntry.Size1);
            tbCodePage.Text = Utility.ToHexString(directoryEntry.ResourceDataEntry.CodePage);
            tbReserved.Text = Utility.ToHexString(directoryEntry.ResourceDataEntry.Reserved);

            // Build the hex output
            var rawOffset = Utility.RVAtoFileMapping(
                directoryEntry.ResourceDataEntry.OffsetToData, 
                _peFile.ImageSectionHeaders
                );
            
            tbResource.Text = string.Join(" ", Utility.ToHexString(_peFile.Buff, rawOffset, directoryEntry.ResourceDataEntry.Size1));
        }

        

        private void SetSignature(PeFile peFile)
        {
            cbIsSigned.IsChecked = peFile.IsSigned;
            cbChainValid.IsChecked = peFile.IsValidCertChain(true);
            cbSigValid.IsChecked = Utility.IsSignatureValid(peFile.Location);
        }

        private void SetExports(PeFile peFile)
        {
            lbExports.Items.Clear();

            if (peFile.ExportedFunctions == null)
                return;

            foreach(var export in peFile.ExportedFunctions)
            {
                lbExports.Items.Add(new { Name = export.Name, Ordinal = export.Ordinal, RVA = $"0x{export.Address.ToString("X")}" });
            }
        }

        private void SetImports(PeFile peFile)
        {
            lbImportDlls.Items.Clear();

            if (peFile.ImportedFunctions == null)
                return;

            var dllNames = peFile.ImportedFunctions?.Select(x => x.DLL).Distinct();
            var dllFunctions = new Dictionary<string, IEnumerable<PeFile.ImportFunction>>();

            foreach(var dllName in dllNames)
            {
                var functions = peFile.ImportedFunctions.Where(x => x.DLL == dllName);
                dllFunctions.Add(dllName, functions);
            }

            foreach(var kv in dllFunctions)
            {
                lbImportDlls.Items.Add(new { DLL = kv.Key });
            }
        }

        private void SetFileInfo(PeFile peFile)
        {
            tbLocation.Text = peFile.Location;
            tbSize.Text = $"{peFile.FileSize} Bytes";
            tbMD5.Text = peFile.MD5;
            tbSHA1.Text = peFile.SHA1;
            tbSHA256.Text = peFile.SHA256;
            tbImpHash.Text = peFile.ImpHash;
        }

        private void SetDosHeader(PeFile peFile)
        {
            var magic = peFile.ImageDosHeader.e_magic;

            tbe_magic.Text = magic == 0x5A4D ? $"{Utility.ToHexString(magic)} <-> {"MZ"}" : Utility.ToHexString(magic);
            tbe_cblp.Text = Utility.ToHexString(peFile.ImageDosHeader.e_cblp);
            tbe_cp.Text = Utility.ToHexString(peFile.ImageDosHeader.e_cp);
            tbe_crlc.Text = Utility.ToHexString(peFile.ImageDosHeader.e_crlc);
            tbe_cparhdr.Text = Utility.ToHexString(peFile.ImageDosHeader.e_cparhdr);
            tbe_minalloc.Text = Utility.ToHexString(peFile.ImageDosHeader.e_minalloc);
            tbe_maxalloc.Text = Utility.ToHexString(peFile.ImageDosHeader.e_maxalloc);
            tbe_ss.Text = Utility.ToHexString(peFile.ImageDosHeader.e_ss);
            tbe_sp.Text = Utility.ToHexString(peFile.ImageDosHeader.e_sp);
            tbe_csum.Text = Utility.ToHexString(peFile.ImageDosHeader.e_csum);
            tbe_ip.Text = Utility.ToHexString(peFile.ImageDosHeader.e_ip);
            tbe_cs.Text = Utility.ToHexString(peFile.ImageDosHeader.e_cs);
            tbe_lfarlc.Text = Utility.ToHexString(peFile.ImageDosHeader.e_lfarlc);
            tbe_ovno.Text = Utility.ToHexString(peFile.ImageDosHeader.e_ovno);
            tbe_res.Text = Utility.ToHexString(peFile.ImageDosHeader.e_res);
            tbe_oemid.Text = Utility.ToHexString(peFile.ImageDosHeader.e_oemid);
            tbe_oeminfo.Text = Utility.ToHexString(peFile.ImageDosHeader.e_oeminfo);
            tbe_res2.Text = Utility.ToHexString(peFile.ImageDosHeader.e_res2);
            tbe_lfanew.Text = Utility.ToHexString(peFile.ImageDosHeader.e_lfanew);
        }

        private void SetNtHeader(PeFile peFile)
        {
            tbSignature.Text = Utility.ToHexString(peFile.ImageNtHeaders.Signature);
        }

        private void SetFileHeader(PeFile peFile)
        {
            var fileHeader = peFile.ImageNtHeaders.FileHeader;
            var machine = fileHeader.Machine;
            var characteristics = fileHeader.Characteristics;

            tbMachine.Text = $"{Utility.ToHexString(machine)} <-> {Utility.ResolveTargetMachine(machine)}";
            tbNumberOfSections.Text = Utility.ToHexString(fileHeader.NumberOfSections);
            tbTimeDateStamp.Text = Utility.ToHexString(fileHeader.TimeDateStamp);
            tbPointerToSymbolTable.Text = Utility.ToHexString(fileHeader.PointerToSymbolTable);
            tbNumberOfSymbols.Text = Utility.ToHexString(fileHeader.NumberOfSymbols);
            tbSizeOfOptionalHeader.Text = Utility.ToHexString(fileHeader.SizeOfOptionalHeader);
            tbCharacteristics.Text = $"{Utility.ToHexString(characteristics)}\n\n{Utility.ResolveFileCharacteristics(characteristics)}";
        }

        private void SetOptionalHeader(PeFile peFile)
        {
            var oh = peFile.ImageNtHeaders.OptionalHeader;

            tbMagic.Text = Utility.ToHexString(oh.Magic);
            tbMajorLinkerVersion.Text = Utility.ToHexString(oh.MajorLinkerVersion);
            tbMinorLinkerVersion.Text = Utility.ToHexString(oh.MinorLinkerVersion);
            tbSizeOfCode.Text = Utility.ToHexString(oh.SizeOfCode);
            tbSizeOfInitializedData.Text = Utility.ToHexString(oh.SizeOfInitializedData);
            tbSizeOfUninitializedData.Text = Utility.ToHexString(oh.SizeOfUninitializedData);
            tbAddressOfEntryPoint.Text = Utility.ToHexString(oh.AddressOfEntryPoint);
            tbBaseOfCode.Text = Utility.ToHexString(oh.BaseOfCode);
            tbBaseOfData.Text = Utility.ToHexString(oh.BaseOfData);
            tbImageBase.Text = Utility.ToHexString(oh.ImageBase);
            tbSectionAlignment.Text = Utility.ToHexString(oh.SectionAlignment);
            tbFileAlignment.Text = Utility.ToHexString(oh.FileAlignment);
            tbMajorOperatingSystemVersion.Text = Utility.ToHexString(oh.MajorOperatingSystemVersion);
            tbMinorOperatingSystemVersion.Text = Utility.ToHexString(oh.MinorOperatingSystemVersion);
            tbMajorImageVersion.Text = Utility.ToHexString(oh.MajorImageVersion);
            tbMinorImageVersion.Text = Utility.ToHexString(oh.MinorImageVersion);
            tbMajorSubsystemVersion.Text = Utility.ToHexString(oh.MajorSubsystemVersion);
            tbMinorSubsystemVersion.Text = Utility.ToHexString(oh.MinorSubsystemVersion);
            tbWin32VersionValue.Text = Utility.ToHexString(oh.Win32VersionValue);
            tbSizeOfImage.Text = Utility.ToHexString(oh.SizeOfImage);
            tbSizeOfHeaders.Text = Utility.ToHexString(oh.SizeOfHeaders);
            tbCheckSum.Text = Utility.ToHexString(oh.CheckSum);
            tbSubsystem.Text = Utility.ToHexString(oh.Subsystem);
            tbDllCharacteristics.Text = Utility.ToHexString(oh.DllCharacteristics);
            tbSizeOfStackReserve.Text = Utility.ToHexString(oh.SizeOfStackReserve);
            tbSizeOfStackCommit.Text = Utility.ToHexString(oh.SizeOfStackCommit);
            tbSizeOfHeapReserve.Text = Utility.ToHexString(oh.SizeOfHeapReserve);
            tbSizeOfHeapCommit.Text = Utility.ToHexString(oh.SizeOfHeapCommit);
            tbLoaderFlags.Text = Utility.ToHexString(oh.LoaderFlags);
            tbNumberOfRvaAndSizes.Text = Utility.ToHexString(oh.NumberOfRvaAndSizes);
        }

        private void SetException(PeFile peFile)
        {
            lbRuntimeFunctions.Items.Clear();

            if (peFile.Is32Bit || peFile.RuntimeFunctions == null)
                return;

            foreach(var rt in peFile.RuntimeFunctions)
            {
                lbRuntimeFunctions.Items.Add(new
                {
                    FunctionStart = Utility.ToHexString(rt.FunctionStart),
                    FunctionEnd = Utility.ToHexString(rt.FunctionEnd),
                    UnwindInfo = Utility.ToHexString(rt.UnwindInfo),
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

            foreach(var function in functions)
            {
                lbImportFunctions.Items.Add(new { Name = function.Name, Hint = function.Hint });
            }
        }

        private void lbRuntimeFunctions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selected = (sender as ListBox).SelectedItem;

            // Convert string of format 0x... to an integer.
            var funcStart = (int)new System.ComponentModel.Int32Converter().ConvertFromString(selected.FunctionStart);
            var funcEnd = (int)new System.ComponentModel.Int32Converter().ConvertFromString(selected.FunctionEnd);
            var uw = (int)new System.ComponentModel.Int32Converter().ConvertFromString(selected.UnwindInfo);

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
            tbUIExHandlerFuncEntry.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.ExceptionHandler);
            // TODO: display excetption data as a hex array.
            //tbUIExData.Text = string.Format("", rt.ResolvedUnwindInfo.ExceptionData);

            // Set the UNWIND_CODE structures for the UNWIND_INFO
            lbUnwindCode.Items.Clear();
            foreach(var uc in rt.ResolvedUnwindInfo.UnwindCode)
            {
                lbUnwindCode.Items.Add(new
                {
                    CodeOffset = Utility.ToHexString(uc.CodeOffset),
                    UnwindOp = Utility.ToHexString(uc.UnwindOp),
                    FrameOffset = Utility.ToHexString(uc.FrameOffset)
                });
            }
        }
    }
}
