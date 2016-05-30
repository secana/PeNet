/***********************************************************************
Copyright 2016 Stefan Hausotte

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using PeNet.Structures;

namespace PeNet
{
    /// <summary>
    ///     This class represents a Portable Executable (PE) file and makes the different
    ///     header and properties accessable.
    /// </summary>
    public class PeFile
    {
        private readonly uint _secHeaderOffset;

        /// <summary>
        ///     The PE binary as a byte array.
        /// </summary>
        public readonly byte[] Buff;

        private bool _alreadyParsedDosHeader;

        private bool _alreadyParsedExceptionDirectory;
        private bool _alreadyParsedExportDirectory;
        private bool _alreadyParsedExportedFuntions;
        private bool _alreadyParsedImportDirectory;
        private bool _alreadyParsedImportedFunctions;
        private bool _alreadyParsedNtHeaders;
        private bool _alreadyParsedPKCS7;
        private bool _alreadyParsedRelocationDirectory;
        private bool _alreadyParsedResourceDirectory;
        private bool _alreadyParsedSectionHeaders;
        private bool _alreadyParsedSecurityDirectory;
        private ExportFunction[] _exportedFunctions;
        private IMAGE_DOS_HEADER _imageDosHeader;
        private IMAGE_EXPORT_DIRECTORY _imageExportDirectory;
        private IMAGE_IMPORT_DESCRIPTOR[] _imageImportDescriptors;
        private IMAGE_NT_HEADERS _imageNtHeaders;
        private IMAGE_RESOURCE_DIRECTORY _imageResourceDirectory;
        private IMAGE_BASE_RELOCATION[] _imagerRelocationDirectory;
        private IMAGE_SECTION_HEADER[] _imageSectionHeaders;
        private string _impHash;
        private ImportFunction[] _importedFunctions;
        private string _md5;
        private X509Certificate2 _pkcs7;
        private RUNTIME_FUNCTION[] _runtimeFunctions;
        private string _sha1;
        private string _sha256;
        private WIN_CERTIFICATE _winCertificate;


        /// <summary>
        ///     Create a new PeFile object.
        /// </summary>
        /// <param name="buff">A PE file a byte array.</param>
        public PeFile(byte[] buff)
        {
            Buff = buff;
            _secHeaderOffset = (uint) (Is64Bit ? 0x108 : 0xF8);
        }

        /// <summary>
        ///     Create a new PeFile object.
        /// </summary>
        /// <param name="peFile">Path to a PE file.</param>
        public PeFile(string peFile)
            : this(File.ReadAllBytes(peFile))
        {
            Location = peFile;
        }

        /// <summary>
        ///     List with all exceptions that have occured during the PE header parsing.
        /// </summary>
        public List<Exception> Exceptions { get; } = new List<Exception>();

        /// <summary>
        ///     Returns true if the Exception Dir, Export Dir, Import Dir,
        ///     Resource Dir and Security Dir are valid and the MZ header is set.
        /// </summary>
        public bool IsValidPeFile => HasValidExceptionDir
                                     && HasValidExportDir
                                     && HasValidImportDir
                                     && HasValidResourceDir
                                     && HasValidSecurityDir
                                     && (ImageDosHeader.e_magic == 0x5a4d);

        /// <summary>
        ///     Returns true if the Export directory is valid.
        /// </summary>
        public bool HasValidExportDir => ImageExportDirectory != null;

        /// <summary>
        ///     Returns true if the Import directory is valid.
        /// </summary>
        public bool HasValidImportDir => ImageImportDescriptors != null;

        /// <summary>
        ///     Returns true if the Resource directory is valid.
        /// </summary>
        public bool HasValidResourceDir => ImageResourceDirectory != null;

        /// <summary>
        ///     Returns true if the Exception directory is valid.
        /// </summary>
        public bool HasValidExceptionDir => Exceptions != null;

        /// <summary>
        ///     Returns true if the Security directory is valid.
        /// </summary>
        public bool HasValidSecurityDir => WinCertificate != null;

        /// <summary>
        ///     Returns true if the Relocation Directory is valid.
        /// </summary>
        public bool HasValidRelocDir => ImageRelocationDirectory != null;

        /// <summary>
        ///     Returns true if the DLL flag in the
        ///     File Header is set.
        /// </summary>
        public bool IsDLL
            =>
                (ImageNtHeaders.FileHeader.Characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_DLL) >
                0;

        /// <summary>
        ///     Returns true if the Executable flag in the
        ///     File Header is set.
        /// </summary>
        public bool IsEXE
            =>
                (ImageNtHeaders.FileHeader.Characteristics &
                 (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_EXECUTABLE_IMAGE) > 0;

        /// <summary>
        ///     Returns true if the PE file is signed. It
        ///     does not check if the signature is valid!
        /// </summary>
        public bool IsSigned => PKCS7 != null;

        /// <summary>
        ///     Returns true if the PE file is x64.
        /// </summary>
        public bool Is64Bit => Utility.BytesToUInt16(Buff, ImageDosHeader.e_lfanew + 0x4) ==
                               (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_AMD64;

        /// <summary>
        ///     Returns true if the PE file is x32.
        /// </summary>
        public bool Is32Bit => !Is64Bit;

        /// <summary>
        ///     Access the IMAGE_DOS_HEADER of the PE file.
        /// </summary>
        public IMAGE_DOS_HEADER ImageDosHeader
        {
            get
            {
                if (_alreadyParsedDosHeader)
                    return _imageDosHeader;

                _alreadyParsedDosHeader = true;
                _imageDosHeader = new IMAGE_DOS_HEADER(Buff);
                return _imageDosHeader;
            }
        }

        /// <summary>
        ///     Access the IMAGE_NT_HEADERS of the PE file.
        /// </summary>
        public IMAGE_NT_HEADERS ImageNtHeaders
        {
            get
            {
                if (_alreadyParsedNtHeaders)
                    return _imageNtHeaders;

                _alreadyParsedNtHeaders = true;
                _imageNtHeaders = new IMAGE_NT_HEADERS(Buff, ImageDosHeader.e_lfanew, Is64Bit);
                return _imageNtHeaders;
            }
        }

        /// <summary>
        ///     Access the IMAGE_SECTION_HEADERS of the PE file.
        /// </summary>
        public IMAGE_SECTION_HEADER[] ImageSectionHeaders
        {
            get
            {
                if (_alreadyParsedSectionHeaders)
                    return _imageSectionHeaders;

                _alreadyParsedSectionHeaders = true;

                _imageSectionHeaders = ParseImageSectionHeaders(
                    Buff,
                    ImageNtHeaders.FileHeader.NumberOfSections,
                    ImageDosHeader.e_lfanew + _secHeaderOffset
                    );

                return _imageSectionHeaders;
            }
        }

        /// <summary>
        ///     Access the IMAGE_EXPORT_DIRECTORY of the PE file.
        /// </summary>
        public IMAGE_EXPORT_DIRECTORY ImageExportDirectory
        {
            get
            {
                if (_alreadyParsedExportDirectory)
                    return _imageExportDirectory;

                _alreadyParsedExportDirectory = true;

                if (
                    ImageNtHeaders.OptionalHeader.DataDirectory[(int) Constants.DataDirectoryIndex.Export]
                        .VirtualAddress == 0)
                    return _imageExportDirectory;

                _imageExportDirectory = ParseImageExportDirectory(
                    Buff,
                    Utility.RVAtoFileMapping(
                        ImageNtHeaders.OptionalHeader.DataDirectory[(int) Constants.DataDirectoryIndex.Export]
                            .VirtualAddress,
                        ImageSectionHeaders
                        )
                    );
                return _imageExportDirectory;
            }
        }

        /// <summary>
        ///     Access the IMAGE_IMPORT_DESCRIPTOR array of the PE file.
        /// </summary>
        public IMAGE_IMPORT_DESCRIPTOR[] ImageImportDescriptors
        {
            get
            {
                if (_alreadyParsedImportDirectory)
                    return _imageImportDescriptors;

                _alreadyParsedImportDirectory = true;

                if (ImageNtHeaders.OptionalHeader.DataDirectory[(int) Constants.DataDirectoryIndex.Import]
                    .VirtualAddress == 0)
                    return null;

                _imageImportDescriptors = ParseImportDescriptors(
                    Buff,
                    Utility.RVAtoFileMapping(
                        ImageNtHeaders.OptionalHeader.DataDirectory[(int) Constants.DataDirectoryIndex.Import]
                            .VirtualAddress,
                        ImageSectionHeaders)
                    );
                return _imageImportDescriptors;
            }
        }

        /// <summary>
        ///     Access the IMAGE_BASE_RELOCATION array of the PE file.
        /// </summary>
        public IMAGE_BASE_RELOCATION[] ImageRelocationDirectory
        {
            get
            {
                if (_alreadyParsedRelocationDirectory)
                    return _imagerRelocationDirectory;


                _alreadyParsedRelocationDirectory = true;
                _imagerRelocationDirectory = ParseRelocationDirectory(
                    Buff,
                    ImageNtHeaders.OptionalHeader.DataDirectory[(int) Constants.DataDirectoryIndex.BaseReloc]
                        .VirtualAddress,
                    ImageNtHeaders.OptionalHeader.DataDirectory[(int) Constants.DataDirectoryIndex.BaseReloc].Size,
                    ImageSectionHeaders
                    );

                return _imagerRelocationDirectory;
            }
        }

        /// <summary>
        ///     Access the exported functions as an array of parsed objects.
        /// </summary>
        public ExportFunction[] ExportedFunctions
        {
            get
            {
                if (_alreadyParsedExportedFuntions)
                    return _exportedFunctions;

                _alreadyParsedExportedFuntions = true;
                _exportedFunctions = ParseExportedFunctions(Buff, ImageExportDirectory, ImageSectionHeaders);

                return _exportedFunctions;
            }
        }

        /// <summary>
        ///     Access the imported functions as an array of parsed objects.
        /// </summary>
        public ImportFunction[] ImportedFunctions
        {
            get
            {
                if (_alreadyParsedImportedFunctions)
                    return _importedFunctions;

                _alreadyParsedImportedFunctions = true;
                _importedFunctions = ParseImportedFunctions(Buff, ImageImportDescriptors, ImageSectionHeaders);
                return _importedFunctions;
            }
        }

        /// <summary>
        ///     Access the IMAGE_RESOURCE_DIRECTORY of the PE file.
        /// </summary>
        public IMAGE_RESOURCE_DIRECTORY ImageResourceDirectory
        {
            get
            {
                if (_alreadyParsedResourceDirectory)
                    return _imageResourceDirectory;

                _alreadyParsedResourceDirectory = true;
                _imageResourceDirectory = ParseImageResourceDirectory(
                    Buff,
                    Utility.RVAtoFileMapping(
                        ImageNtHeaders.OptionalHeader.DataDirectory[(int) Constants.DataDirectoryIndex.Resource]
                            .VirtualAddress,
                        ImageSectionHeaders)
                    );
                return _imageResourceDirectory;
            }
        }

        /// <summary>
        ///     Access the array of RUNTIME_FUNCTION from the Exception header.
        /// </summary>
        public RUNTIME_FUNCTION[] RuntimeFunctions
        {
            get
            {
                if (_alreadyParsedExceptionDirectory)
                    return _runtimeFunctions;

                _alreadyParsedExceptionDirectory = true;

                _runtimeFunctions = ParseExceptionDirectory(
                    Buff,
                    Utility.RVAtoFileMapping(
                        ImageNtHeaders.OptionalHeader.DataDirectory[(uint) Constants.DataDirectoryIndex.Exception]
                            .VirtualAddress,
                        ImageSectionHeaders
                        ),
                    ImageNtHeaders.OptionalHeader.DataDirectory[(uint) Constants.DataDirectoryIndex.Exception].Size
                    );

                return _runtimeFunctions;
            }
        }

        /// <summary>
        ///     Access the WIN_CERTIFICATE from the Security header.
        /// </summary>
        public WIN_CERTIFICATE WinCertificate
        {
            get
            {
                if (_alreadyParsedSecurityDirectory)
                    return _winCertificate;

                _alreadyParsedSecurityDirectory = true;
                _winCertificate = ParseImageSecurityDirectory(
                    Buff,
                    ImageNtHeaders.OptionalHeader.DataDirectory[(int) Constants.DataDirectoryIndex.Security]
                        .VirtualAddress
                    );

                return _winCertificate;
            }
        }

        /// <summary>
        ///     A X509 PKCS7 signature if the PE file was digitally signed with such
        ///     a signature.
        /// </summary>
        public X509Certificate2 PKCS7
        {
            get
            {
                if (_alreadyParsedPKCS7)
                    return _pkcs7;

                _alreadyParsedPKCS7 = true;

                if (WinCertificate == null)
                {
                    return null;
                }

                if (WinCertificate.wCertificateType ==
                    (ushort) Constants.WinCertificateType.WIN_CERT_TYPE_PKCS_SIGNED_DATA)
                {
                    var cert = WinCertificate.bCertificate;
                    _pkcs7 = new X509Certificate2(cert);
                }

                return _pkcs7;
            }
        }

        /// <summary>
        ///     The SHA-256 hash sum of the binary.
        /// </summary>
        public string SHA256 => _sha256 ?? (_sha256 = Utility.Sha256(Buff));

        /// <summary>
        ///     The SHA-1 hash sum of the binary.
        /// </summary>
        public string SHA1 => _sha1 ?? (_sha1 = Utility.Sha1(Buff));

        /// <summary>
        ///     The MD5 of hash sum of the binary.
        /// </summary>
        public string MD5 => _md5 ?? (_md5 = Utility.MD5(Buff));

        /// <summary>
        ///     The Import Hash of the binary if any imports are
        ///     given esle null;
        /// </summary>
        public string ImpHash => _impHash ?? (_impHash = GetImpHash());

        /// <summary>
        ///     Returns the file size in bytes.
        /// </summary>
        public int FileSize => Buff.Length;

        /// <summary>
        ///     Location of the PE file if it was opened by location.
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="online">Check certificate chain online or offline.</param>
        /// <returns>True of cert chain is valid and from a trusted CA.</returns>
        public bool IsValidCertChain(bool online)
        {
            if (!IsSigned)
                return false;

            return Utility.IsValidCertChain(PKCS7, online);
        }

        private IMAGE_EXPORT_DIRECTORY ParseImageExportDirectory(byte[] buff, uint offset)
        {
            if (offset == 0)
                return null;

            IMAGE_EXPORT_DIRECTORY imageExportDirectory = null;

            try
            {
                imageExportDirectory = new IMAGE_EXPORT_DIRECTORY(buff, offset);
            }
            catch (Exception exception)
            {
                Exceptions.Add(exception);
            }

            return imageExportDirectory;
        }

        /// <summary>
        ///     Get an object which holds information about
        ///     the Certificate Revocation Lists of the signing
        ///     certificate if any is present.
        /// </summary>
        /// <returns>Certificate Revocation List information or null if binary is not signed.</returns>
        public CrlUrlList GetCrlUrlList()
        {
            if (PKCS7 == null)
                return null;
            return new CrlUrlList(PKCS7);
        }


        private WIN_CERTIFICATE ParseImageSecurityDirectory(byte[] buff, uint dirOffset)
        {
            if (dirOffset == 0)
                return null;

            WIN_CERTIFICATE wc = null;

            try
            {
                wc = new WIN_CERTIFICATE(buff, dirOffset);
            }
            catch (Exception exception)
            {
                Exceptions.Add(exception);
            }

            return wc;
        }

        private ImportFunction[] ParseImportedFunctions(byte[] buff, IMAGE_IMPORT_DESCRIPTOR[] idescs,
            IMAGE_SECTION_HEADER[] sh)
        {
            if (idescs == null)
                return null;

            var impFuncs = new List<ImportFunction>();
            var sizeOfThunk = (uint) (Is64Bit ? 0x8 : 0x4); // Size of IMAGE_THUNK_DATA
            var ordinalBit = Is64Bit ? 0x8000000000000000 : 0x80000000;
            var ordinalMask = (ulong) (Is64Bit ? 0x7FFFFFFFFFFFFFFF : 0x7FFFFFFF);

            try
            {
                foreach (var idesc in idescs)
                {
                    var dllAdr = Utility.RVAtoFileMapping(idesc.Name, sh);
                    var dll = Utility.GetName(dllAdr, buff);
                    var tmpAdr = idesc.OriginalFirstThunk != 0 ? idesc.OriginalFirstThunk : idesc.FirstThunk;
                    if (tmpAdr == 0)
                        continue;

                    var thunkAdr = Utility.RVAtoFileMapping(tmpAdr, sh);
                    uint round = 0;
                    while (true)
                    {
                        var t = new IMAGE_THUNK_DATA(buff, thunkAdr + round*sizeOfThunk, Is64Bit);

                        if (t.AddressOfData == 0)
                            break;

                        // Check if import by name or by ordinal.
                        // If it is an import by ordinal, the most significant bit of "Ordinal" is "1" and the ordinal can
                        // be extracted from the least significant bits.
                        // Else it is an import by name and the link to the IMAGE_IMPORT_BY_NAME has to be followed

                        if ((t.Ordinal & ordinalBit) == ordinalBit) // Import by ordinal
                        {
                            impFuncs.Add(new ImportFunction(null, dll, (ushort) (t.Ordinal & ordinalMask)));
                        }
                        else // Import by name
                        {
                            var ibn = new IMAGE_IMPORT_BY_NAME(buff, Utility.RVAtoFileMapping(t.AddressOfData, sh));
                            impFuncs.Add(new ImportFunction(ibn.Name, dll, ibn.Hint));
                        }

                        round++;
                    }
                }
            }
            catch (Exception exception)
            {
                Exceptions.Add(exception);
                return null;
            }

            return impFuncs.ToArray();
        }


        private IMAGE_IMPORT_DESCRIPTOR[] ParseImportDescriptors(byte[] buff, uint offset)
        {
            if (offset == 0)
                return null;

            var idescs = new List<IMAGE_IMPORT_DESCRIPTOR>();
            uint idescSize = 20; // Size of IMAGE_IMPORT_DESCRIPTOR (5 * 4 Byte)
            uint round = 0;

            try
            {
                while (true)
                {
                    var idesc = new IMAGE_IMPORT_DESCRIPTOR(buff, offset + idescSize*round);

                    // Found the last IMAGE_IMPORT_DESCRIPTOR which is completely null (except TimeDateStamp).
                    if (idesc.OriginalFirstThunk == 0
                        //&& idesc.TimeDateStamp == 0
                        && idesc.ForwarderChain == 0
                        && idesc.Name == 0
                        && idesc.FirstThunk == 0)
                    {
                        break;
                    }

                    idescs.Add(idesc);
                    round++;
                }
            }
            catch (Exception exception)
            {
                Exceptions.Add(exception);
                return null;
            }


            return idescs.ToArray();
        }

        private ExportFunction[] ParseExportedFunctions(
            byte[] buff,
            IMAGE_EXPORT_DIRECTORY ed,
            IMAGE_SECTION_HEADER[] sh
            )
        {
            if (ed == null || ed.AddressOfFunctions == 0)
                return null;

            var expFuncs = new ExportFunction[ed.NumberOfFunctions];

            try
            {
                
                var funcOffsetPointer = Utility.RVAtoFileMapping(ed.AddressOfFunctions, sh);
                var ordOffset = Utility.RVAtoFileMapping(ed.AddressOfNameOrdinals, sh);
                var nameOffsetPointer = Utility.RVAtoFileMapping(ed.AddressOfNames, sh);

                //Get addresses
                for (uint i = 0; i < expFuncs.Length; i++)
                {
                    var ordinal = i + ed.Base;
                    var address = Utility.BytesToUInt32(buff, funcOffsetPointer + sizeof(uint)*i);

                    expFuncs[i] = new ExportFunction(null, address, (ushort) ordinal);
                }

                //Associate names
                for (uint i = 0; i < ed.NumberOfNames; i++)
                {
                    var namePtr = Utility.BytesToUInt32(buff, nameOffsetPointer + sizeof(uint)*i);
                    var nameAdr = Utility.RVAtoFileMapping(namePtr, sh);
                    var name = Utility.GetName(nameAdr, buff);
                    var ordinalIndex = (uint) Utility.GetOrdinal(ordOffset + sizeof(ushort)*i, buff);

                    expFuncs[ordinalIndex] = new ExportFunction(name, expFuncs[ordinalIndex].Address,
                        expFuncs[ordinalIndex].Ordinal);
                }
            }
            catch (Exception exception)
            {
                Exceptions.Add(exception);
            }

            return expFuncs;
        }

        private IMAGE_SECTION_HEADER[] ParseImageSectionHeaders(byte[] buff, ushort numOfSections, uint offset)
        {
            var sh = new IMAGE_SECTION_HEADER[numOfSections];
            uint secSize = 0x28; // Every section header is 40 bytes in size.
            for (uint i = 0; i < numOfSections; i++)
            {
                sh[i] = new IMAGE_SECTION_HEADER(buff, offset + i*secSize);
            }

            return sh;
        }

        /// <summary>
        ///     http://www.brokenthorn.com/Resources/OSDevPE.html
        /// </summary>
        /// <param name="buff">Byte buffer with the whole binary.</param>
        /// <param name="offsetFirstRescDir">Offset to the first resource directory (= DataDirectory[2].VirtualAddress)</param>
        /// <returns>The image resource directory.</returns>
        private IMAGE_RESOURCE_DIRECTORY ParseImageResourceDirectory(byte[] buff, uint offsetFirstRescDir)
        {
            if (offsetFirstRescDir == 0)
                return null;

            // Parse the root directory.
            var root = new IMAGE_RESOURCE_DIRECTORY(buff, offsetFirstRescDir, offsetFirstRescDir);

            try
            {
                // Parse the second stage (type)
                foreach (var de in root.DirectoryEntries)
                {
                    de.ResourceDirectory = new IMAGE_RESOURCE_DIRECTORY(
                        buff,
                        offsetFirstRescDir + de.OffsetToDirectory,
                        offsetFirstRescDir
                        );

                    // Parse the third stage (name/IDs)
                    foreach (var de2 in de.ResourceDirectory.DirectoryEntries)
                    {
                        de2.ResourceDirectory = new IMAGE_RESOURCE_DIRECTORY(
                            buff,
                            offsetFirstRescDir + de2.OffsetToDirectory,
                            offsetFirstRescDir
                            );

                        // Parse the forth stage (language) with the data.
                        foreach (var de3 in de2.ResourceDirectory.DirectoryEntries)
                        {
                            de3.ResourceDataEntry = new IMAGE_RESOURCE_DATA_ENTRY(buff,
                                offsetFirstRescDir + de3.OffsetToData);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Exceptions.Add(exception);
                return null;
            }


            return root;
        }

        private RUNTIME_FUNCTION[] ParseExceptionDirectory(byte[] buff, uint offset, uint size)
        {
            if (Is32Bit || offset == 0)
                return null;

            var sizeOfRuntimeFunction = 0xC;
            var rf = new RUNTIME_FUNCTION[size/sizeOfRuntimeFunction];

            try
            {
                for (var i = 0; i < rf.Length; i++)
                {
                    rf[i] = new RUNTIME_FUNCTION(buff, (uint) (offset + i*sizeOfRuntimeFunction), ImageSectionHeaders);
                }
            }
            catch (Exception exception)
            {
                Exceptions.Add(exception);
                return null;
            }

            return rf;
        }

        private IMAGE_BASE_RELOCATION[] ParseRelocationDirectory(
            byte[] buff,
            uint offset,
            uint size,
            IMAGE_SECTION_HEADER[] sh)
        {
            if (offset == 0)
                return null;

            var imageBaseRelocations = new List<IMAGE_BASE_RELOCATION>();
            var rvaOffset = Utility.RVAtoFileMapping(offset, sh);
            var currentBlock = rvaOffset;

            try
            {
                while (true)
                {
                    if (currentBlock >= rvaOffset + size - 8)
                        break;

                    imageBaseRelocations.Add(new IMAGE_BASE_RELOCATION(buff, currentBlock, size));
                    currentBlock += imageBaseRelocations.Last().SizeOfBlock;
                }
            }
            catch (Exception exception)
            {
                Exceptions.Add(exception);
                return null;
            }

            return imageBaseRelocations.ToArray();
        }

        /// <summary>
        ///     Tries to parse the PE file and checks all directories.
        /// </summary>
        /// <param name="file">Path to a possible PE file.</param>
        /// <returns>
        ///     True if the file could be parsed as a PE file and
        ///     all directories are valid.
        /// </returns>
        public static bool IsValidPEFile(string file)
        {
            PeFile pe;
            try
            {
                pe = new PeFile(file);
            }
            catch
            {
                return false;
            }
            return pe.IsValidPeFile;
        }

        /// <summary>
        ///     Tests is a file is a PE file based on the MZ
        ///     header. It is not checked if the PE file is correct
        ///     in all other parts.
        /// </summary>
        /// <param name="file">Path to a possible PE file.</param>
        /// <returns>True if the MZ header is set.</returns>
        public static bool IsPEFile(string file)
        {
            var buff = File.ReadAllBytes(file);
            IMAGE_DOS_HEADER dosHeader = null;
            try
            {
                dosHeader = new IMAGE_DOS_HEADER(buff);
            }
            catch (Exception)
            {
                return false;
            }
            return dosHeader.e_magic == 0x5a4d;
        }

        /// <summary>
        ///     Returns if the file is a PE file and 64 Bit.
        /// </summary>
        /// <param name="file">Path to a possible PE file.</param>
        /// <returns>True if file is PE and x64.</returns>
        public static bool Is64BitPeFile(string file)
        {
            var buff = File.ReadAllBytes(file);
            IMAGE_DOS_HEADER dosHeader;
            bool is64;
            try
            {
                dosHeader = new IMAGE_DOS_HEADER(buff);
                is64 = Utility.BytesToUInt16(buff, dosHeader.e_lfanew + 0x4) ==
                       (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_AMD64;
            }
            catch (Exception)
            {
                return false;
            }

            return (dosHeader.e_magic == 0x5a4d) && is64;
        }

        /// <summary>
        ///     Returns if the file is a PE file and 32 Bit.
        /// </summary>
        /// <param name="file">Path to a possible PE file.</param>
        /// <returns>True if file is PE and x32.</returns>
        public static bool Is32BitPeFile(string file)
        {
            var buff = File.ReadAllBytes(file);
            IMAGE_DOS_HEADER dosHeader;
            bool is32;
            try
            {
                dosHeader = new IMAGE_DOS_HEADER(buff);
                is32 = Utility.BytesToUInt16(buff, dosHeader.e_lfanew + 0x4) ==
                       (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_I386;
            }
            catch (Exception)
            {
                return false;
            }

            return (dosHeader.e_magic == 0x5a4d) && is32;
        }

        /// <summary>
        ///     Returns if the PE file is a EXE, DLL and which architecture
        ///     is used (32/64).
        ///     Architectures: "I386", "AMD64", "UNKNOWN"
        ///     DllOrExe: "DLL", "EXE", "UNKNOWN"
        /// </summary>
        /// <returns>
        ///     A string "architecture_dllOrExe".
        ///     E.g. "AMD64_DLL"
        /// </returns>
        public string GetFileType()
        {
            string fileType;

            switch (ImageNtHeaders.FileHeader.Machine)
            {
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_I386:
                    fileType = "I386";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_AMD64:
                    fileType = "AMD64";
                    break;
                default:
                    fileType = "UNKNOWN";
                    break;
            }

            if ((ImageNtHeaders.FileHeader.Characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_DLL) !=
                0)
                fileType += "_DLL";
            else if ((ImageNtHeaders.FileHeader.Characteristics &
                      (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_EXECUTABLE_IMAGE) != 0)
                fileType += "_EXE";
            else
                fileType += "_UNKNOWN";


            return fileType;
        }

        /// <summary>
        ///     Mandiant’s imphash convention requires the following:
        ///     Resolving ordinals to function names when they appear.
        ///     Converting both DLL names and function names to all lowercase.
        ///     Removing the file extensions from imported module names.
        ///     Building and storing the lowercased strings in an ordered list.
        ///     Generating the MD5 hash of the ordered list.
        ///     oleaut32, ws2_32 and wsock32 can resolve ordinals to functions names.
        ///     The implementation is equal to the python module "pefile" 1.2.10-139
        ///     https://code.google.com/p/pefile/
        /// </summary>
        /// <returns>The ImpHash of the PE file.</returns>
        public string GetImpHash()
        {
            if (ImportedFunctions == null || ImportedFunctions.Length == 0)
                return null;

            var list = new List<string>();
            foreach (var impFunc in ImportedFunctions)
            {
                var tmp = impFunc.DLL.Split('.')[0];
                tmp += ".";
                if (impFunc.Name == null) // Import by ordinal
                {
                    if (impFunc.DLL == "oleaut32.dll")
                    {
                        tmp += OrdinalSymbolMapping.Lookup(OrdinalSymbolMapping.Modul.oleaut32, impFunc.Hint);
                    }
                    else if (impFunc.DLL == "ws2_32.dll")
                    {
                        tmp += OrdinalSymbolMapping.Lookup(OrdinalSymbolMapping.Modul.ws2_32, impFunc.Hint);
                    }
                    else if (impFunc.DLL == "wsock32.dll")
                    {
                        tmp += OrdinalSymbolMapping.Lookup(OrdinalSymbolMapping.Modul.wsock32, impFunc.Hint);
                    }
                    else // cannot resolve ordinal to a function name
                    {
                        tmp += "ord";
                        tmp += impFunc.Hint.ToString();
                    }
                }
                else // Import by name
                {
                    tmp += impFunc.Name;
                }

                list.Add(tmp.ToLower());
            }

            // Concatenate all imports to one string separated by ','.
            var imports = string.Join(",", list);

            var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(imports);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var t in hash)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Represents an exported function.
        /// </summary>
        public class ExportFunction
        {
            /// <summary>
            ///     Create a new ExportFunction object.
            /// </summary>
            /// <param name="name">Name of the function.</param>
            /// <param name="address">Address of function.</param>
            /// <param name="ordinal">Ordinal of the function.</param>
            public ExportFunction(string name, uint address, ushort ordinal)
            {
                Name = name;
                Address = address;
                Ordinal = ordinal;
            }

            /// <summary>
            ///     Function name.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            ///     Function RVA.
            /// </summary>
            public uint Address { get; }

            /// <summary>
            ///     Function Ordinal.
            /// </summary>
            public ushort Ordinal { get; }

            /// <summary>
            ///     Creates a string representation of all
            ///     properties of the object.
            /// </summary>
            /// <returns>The exported function as a string.</returns>
            public override string ToString()
            {
                var sb = new StringBuilder("ExportFunction\n");
                sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
                return sb.ToString();
            }
        }

        /// <summary>
        ///     Represents an imported function.
        /// </summary>
        public class ImportFunction
        {
            /// <summary>
            ///     Create a new ImportFunction object.
            /// </summary>
            /// <param name="name">Function name.</param>
            /// <param name="dll">DLL where the function comes from.</param>
            /// <param name="hint">Function hint.</param>
            public ImportFunction(string name, string dll, ushort hint)
            {
                Name = name;
                DLL = dll;
                Hint = hint;
            }

            /// <summary>
            ///     Function name.
            /// </summary>
            public string Name { get; }

            /// <summary>
            ///     DLL where the function comes from.
            /// </summary>
            public string DLL { get; }

            /// <summary>
            ///     Function hint.
            /// </summary>
            public ushort Hint { get; }
        }

        /// <summary>
        ///     Creates a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>PE Header propteries as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("PE HEADER:\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-15}:\t{1,10:X}\n"));
            return sb.ToString();
        }

        /// <summary>
        ///     This class parses the Certificate Revocation Lists
        ///     of a signing certificate. It provides access to all
        ///     CRL URLs in the certificate.
        /// </summary>
        public class CrlUrlList
        {
            /// <summary>
            ///     Create a new CrlUrlList object.
            /// </summary>
            /// <param name="rawData">A byte array containing a X509 certificate</param>
            public CrlUrlList(byte[] rawData)
            {
                Urls = new List<string>();
                ParseCrls(rawData);
            }

            /// <summary>
            ///     Create a new CrlUrlList object.
            /// </summary>
            /// <param name="cert">A X509 certificate object.</param>
            public CrlUrlList(X509Certificate2 cert)
            {
                Urls = new List<string>();
                foreach (var ext in cert.Extensions)
                {
                    if (ext.Oid.Value == "2.5.29.31")
                    {
                        ParseCrls(ext.RawData);
                    }
                }
            }

            /// <summary>
            ///     List with all CRL URLs.
            /// </summary>
            public List<string> Urls { get; }

            private void ParseCrls(byte[] rawData)
            {
                var rawLength = rawData.Length;
                for (var i = 0; i < rawLength - 5; i++)
                {
                    // Find a HTTP(s) string.
                    if ((rawData[i] == 'h'
                         && rawData[i + 1] == 't'
                         && rawData[i + 2] == 't'
                         && rawData[i + 3] == 'p'
                         && rawData[i + 4] == ':')
                        || (rawData[i] == 'l'
                            && rawData[i + 1] == 'd'
                            && rawData[i + 2] == 'a'
                            && rawData[i + 3] == 'p'
                            && rawData[i + 4] == ':'))
                    {
                        var bytes = new List<byte>();
                        for (var j = i; j < rawLength; j++)
                        {
                            if ((rawData[j - 4] == '.'
                                 && rawData[j - 3] == 'c'
                                 && rawData[j - 2] == 'r'
                                 && rawData[j - 1] == 'l')
                                || (rawData[j] == 'b'
                                    && rawData[j + 1] == 'a'
                                    && rawData[j + 2] == 's'
                                    && rawData[j + 3] == 'e'
                                    ))
                            {
                                i = j;
                                break;
                            }


                            if (rawData[j] < 0x20 || rawData[j] > 0x7E)
                            {
                                i = j;
                                break;
                            }

                            bytes.Add(rawData[j]);
                        }
                        var uri = Encoding.ASCII.GetString(bytes.ToArray());

                        if (IsValidUri(uri) && uri.StartsWith("http://") && uri.EndsWith(".crl"))
                            Urls.Add(uri);

                        if (uri.StartsWith("ldap:", StringComparison.InvariantCulture))
                        {
                            uri = "ldap://" + uri.Split('/')[2];
                            Urls.Add(uri);
                        }
                    }
                }
            }

            private bool IsValidUri(string uri)
            {
                Uri uriResult;
                return Uri.TryCreate(uri, UriKind.Absolute, out uriResult)
                       && (uriResult.Scheme == Uri.UriSchemeHttp
                           || uriResult.Scheme == Uri.UriSchemeHttps);
            }


            /// <summary>
            ///     Create a string representation of all CRL in
            ///     the list.
            /// </summary>
            /// <returns>CRL URLs.</returns>
            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendLine("CRL URLs:");
                foreach (var url in Urls)
                    sb.AppendFormat("\t{0}\n", url);
                return sb.ToString();
            }
        }
    }
}