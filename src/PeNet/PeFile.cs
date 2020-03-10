using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using PeNet.Authenticode;
using PeNet.FileParser;
using PeNet.ImpHash;
using PeNet.Structures;

namespace PeNet
{
    /// <summary>
    ///     This class represents a Portable Executable (PE) file and makes the different
    ///     header and properties accessible.
    /// </summary>
    public class PeFile : IDisposable
    {
        private readonly DataDirectoryParsers _dataDirectoryParsers;
        private readonly NativeStructureParsers _nativeStructureParsers;
        private readonly DotNetStructureParsers _dotNetStructureParsers;
        private readonly AuthenticodeParser _authenticodeParser;


        /// <summary>
        ///     The PE binary .
        /// </summary>
        public IRawFile RawFile { get; }

        private string? _impHash;
        private string? _md5;
        private string? _sha1;
        private string? _sha256;
        private NetGuids? _netGuids;


        public PeFile(IRawFile peFile)
        {
            RawFile = peFile;

            _nativeStructureParsers = new NativeStructureParsers(RawFile);

            _dataDirectoryParsers = new DataDirectoryParsers(
                RawFile,
                ImageNtHeaders?.OptionalHeader?.DataDirectory,
                ImageSectionHeaders,
                Is32Bit
                );

            _dotNetStructureParsers = new DotNetStructureParsers(
                RawFile,
                ImageComDescriptor,
                ImageSectionHeaders
                );

            _authenticodeParser = new AuthenticodeParser(this);
        }

        /// <summary>
        ///     Create a new PeFile object.
        /// </summary>
        /// <param name="buff">A PE file a byte array.</param>
        public PeFile(byte[] buff)
            : this(new BufferFile(buff))
        { }

        /// <summary>
        ///     Create a new PeFile object.
        /// </summary>
        /// <param name="peFile">Path to a PE file.</param>
        public PeFile(string peFile)
            : this(new BufferFile(File.ReadAllBytes(peFile)))
        { }

        /// <summary>
        ///     Create a new PeFile object.
        /// </summary>
        /// <param name="peFile">Stream containing a PE file.</param>
        public PeFile(Stream peFile)
            : this(new StreamFile(peFile))
        { }

        /// <summary>
        /// Try to parse the PE file.
        /// </summary>
        /// <param name="file">Path to a possible PE file.</param>
        /// <param name="peFile">Parsed PE file or Null.</param>
        /// <returns>True if parable PE file and false if not.</returns>
        public static bool TryParse(string file, out PeFile? peFile)
        {
            return TryParse(File.ReadAllBytes(file), out peFile);
        }

        /// <summary>
        /// Try to parse the PE file.
        /// </summary>
        /// <param name="buf">Buffer containing a possible PE file.</param>
        /// <param name="peFile">Parsed PE file or Null.</param>
        /// <returns>True if parable PE file and false if not.</returns>
        public static bool TryParse(byte[] buf, out PeFile? peFile)
        {
            peFile = null;

            if (!IsPEFile(buf))
                return false;

            try { peFile = new PeFile(buf); }
            catch { return false; }

            return true;
        }

        /// <summary>
        ///     Returns true if the DLL flag in the
        ///     File Header is set.
        /// </summary>
        public bool IsDLL
            =>
                (ImageNtHeaders?.FileHeader.Characteristics
                 & (ushort)Constants.FileHeaderCharacteristics.IMAGE_FILE_DLL) > 0;

        /// <summary>
        ///     Returns true if the Executable flag in the
        ///     File Header is set.
        /// </summary>
        public bool IsEXE
            =>
                (ImageNtHeaders?.FileHeader.Characteristics &
                 (ushort)Constants.FileHeaderCharacteristics.IMAGE_FILE_EXECUTABLE_IMAGE) > 0;

        /// <summary>
        ///     Returns true if the PE file is a system driver
        ///     based on the Subsytem = 0x1 value in the Optional Header.
        /// </summary>
        public bool IsDriver => ImageNtHeaders?.OptionalHeader.Subsystem ==
                                (ushort)Constants.OptionalHeaderSubsystem.IMAGE_SUBSYSTEM_NATIVE
                                && ImportedFunctions.FirstOrDefault(i => i.DLL == "ntoskrnl.exe") != null;

        /// <summary>
        ///     Returns true if the PE file is signed. It
        ///     does not check if the signature is valid!
        /// </summary>
        public bool IsSigned => PKCS7 != null;

        /// <summary>
        ///     Returns true if the PE file signature is valid signed.
        /// </summary>
        public bool IsSignatureValid => Authenticode?.IsAuthenticodeValid ?? false;

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="filePath">Path to a PE file.</param>
        /// <param name="useOnlineCRL">Check certificate chain online or offline.</param>
        /// <returns>True if cert chain is valid and from a trusted CA.</returns>
        public bool IsValidCertChain(bool useOnlineCRL)
        {
            if (Authenticode?.SigningCertificate == null)
                return false;

            return IsValidCertChain(Authenticode.SigningCertificate, new TimeSpan(0,0,0,10), useOnlineCRL);
        }

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="cert">X509 Certificate</param>
        /// <param name="urlRetrievalTimeout">Timeout to validate the certificate online.</param>
        /// <param name="useOnlineCRL">If true, uses online certificate revocation lists, else on the local CRL.</param>
        /// <param name="excludeRoot">True if the root certificate should not be validated. False if the whole chain should be validated.</param>
        /// <returns>True if cert chain is valid and from a trusted CA.</returns>
        public bool IsValidCertChain(X509Certificate2? cert, TimeSpan urlRetrievalTimeout, bool useOnlineCRL = true, bool excludeRoot = true)
        {
            var chain = new X509Chain
            {
                ChainPolicy =
                {
                    RevocationFlag      = excludeRoot ? X509RevocationFlag.ExcludeRoot : X509RevocationFlag.EntireChain,
                    RevocationMode      = useOnlineCRL ? X509RevocationMode.Online : X509RevocationMode.Offline,
                    UrlRetrievalTimeout = urlRetrievalTimeout,
                    VerificationFlags   = X509VerificationFlags.NoFlag
                }
            };
            return chain.Build(cert);
        }

        /// <summary>
        /// Information about a possible Authenticode binary signature.
        /// </summary>
        public AuthenticodeInfo? Authenticode => _authenticodeParser.ParseTarget();

        /// <summary>
        ///     Returns true if the PE file is x64.
        /// </summary>
        public bool Is64Bit => !Is32Bit;

        /// <summary>
        ///     Returns true if the PE file is x32.
        /// </summary>
        public bool Is32Bit => ImageNtHeaders?.FileHeader.Machine
                               == MachineType.I386;
        /// <summary>
        ///     Access the IMAGE_DOS_HEADER of the PE file.
        /// </summary>
        public ImageDosHeader? ImageDosHeader => _nativeStructureParsers.ImageDosHeader;

        /// <summary>
        ///     Access the IMAGE_NT_HEADERS of the PE file.
        /// </summary>
        public ImageNtHeaders? ImageNtHeaders => _nativeStructureParsers.ImageNtHeaders;

        /// <summary>
        ///     Access the IMAGE_SECTION_HEADERS of the PE file.
        /// </summary>
        public ImageSectionHeader[]? ImageSectionHeaders => _nativeStructureParsers.ImageSectionHeaders;

        /// <summary>
        ///     Access the IMAGE_EXPORT_DIRECTORY of the PE file.
        /// </summary>
        public ImageExportDirectory? ImageExportDirectory => _dataDirectoryParsers.ImageExportDirectories;

        /// <summary>
        ///     Access the IMAGE_IMPORT_DESCRIPTOR array of the PE file.
        /// </summary>
        public ImageImportDescriptor[]? ImageImportDescriptors => _dataDirectoryParsers.ImageImportDescriptors;

        /// <summary>
        ///     Access the IMAGE_BASE_RELOCATION array of the PE file.
        /// </summary>
        public ImageBaseRelocation[]? ImageRelocationDirectory => _dataDirectoryParsers.ImageBaseRelocations;

        /// <summary>
        ///     Access the IMAGE_DEBUG_DIRECTORY of the PE file.
        /// </summary>
        public ImageDebugDirectory[]? ImageDebugDirectory => _dataDirectoryParsers.ImageDebugDirectory;

        /// <summary>
        ///     Access the exported functions as an array of parsed objects.
        /// </summary>
        public ExportFunction[]? ExportedFunctions => _dataDirectoryParsers.ExportFunctions;

        /// <summary>
        ///     Access the imported functions as an array of parsed objects.
        /// </summary>
        public ImportFunction[]? ImportedFunctions => _dataDirectoryParsers.ImportFunctions;

        /// <summary>
        ///     Access the IMAGE_RESOURCE_DIRECTORY of the PE file.
        /// </summary>
        public ImageResourceDirectory? ImageResourceDirectory => _dataDirectoryParsers.ImageResourceDirectory;

        /// <summary>
        ///     Access resources of the PE file.
        /// </summary>
        public Resources? Resources => _dataDirectoryParsers.Resources;

        /// <summary>
        ///     Access the array of RUNTIME_FUNCTION from the Exception header.
        /// </summary>
        public RUNTIME_FUNCTION[]? ExceptionDirectory => _dataDirectoryParsers.RuntimeFunctions;

        /// <summary>
        ///     Access the WIN_CERTIFICATE from the Security header.
        /// </summary>
        public WIN_CERTIFICATE? WinCertificate => _dataDirectoryParsers.WinCertificate;

        /// <summary>
        /// Access the IMAGE_BOUND_IMPORT_DESCRIPTOR form the data directory.
        /// </summary>
        public ImageBoundImportDescriptor? ImageBoundImportDescriptor => _dataDirectoryParsers.ImageBoundImportDescriptor;

        /// <summary>
        /// Access the IMAGE_TLS_DIRECTORY from the data directory.
        /// </summary>
        public ImageTlsDirectory? ImageTlsDirectory => _dataDirectoryParsers.ImageTlsDirectory;

        /// <summary>
        /// Access the IMAGE_DELAY_IMPORT_DESCRIPTOR from the data directory.
        /// </summary>
        public ImageDelayImportDescriptor? ImageDelayImportDescriptor => _dataDirectoryParsers.ImageDelayImportDescriptor;

        /// <summary>
        /// Access the IMAGE_LOAD_CONFIG_DIRECTORY from the data directory.
        /// </summary>
        public ImageLoadConfigDirectory? ImageLoadConfigDirectory => _dataDirectoryParsers.ImageLoadConfigDirectory;

        /// <summary>
        /// Access the IMAGE_COR20_HEADER (COM Descriptor/CLI) from the data directory.
        /// </summary>
        public ImageCor20Header? ImageComDescriptor => _dataDirectoryParsers.ImageComDescriptor;

        /// <summary>
        ///     Signing X509 certificate if the binary was signed with
        /// </summary>
        public X509Certificate2? PKCS7 => Authenticode?.SigningCertificate;

        /// <summary>
        ///     Access the METADATAHDR from the COM/CLI header.
        /// </summary>
        public METADATAHDR? MetaDataHdr => _dotNetStructureParsers.MetaDataHdr;

        /// <summary>
        /// Meta Data Stream #String.
        /// </summary>
        public IMETADATASTREAM_STRING? MetaDataStreamString => _dotNetStructureParsers.MetaDataStreamString;

        /// <summary>
        /// Meta Data Stream #US (User strings).
        /// </summary>
        public IMETADATASTREAM_US? MetaDataStreamUS => _dotNetStructureParsers.MetaDataStreamUS;

        /// <summary>
        /// Meta Data Stream #GUID.
        /// </summary>
        public IMETADATASTREAM_GUID? MetaDataStreamGUID => _dotNetStructureParsers.MetaDataStreamGUID;

        /// <summary>
        /// Meta Data Stream #Blob as an byte array.
        /// </summary>
        public byte[]? MetaDataStreamBlob => _dotNetStructureParsers.MetaDataStreamBlob;

        /// <summary>
        ///     Access the Meta Data Stream Tables Header from the list of
        ///     Meta Data Streams of the .Net header.
        /// </summary>
        public METADATATABLESHDR? MetaDataStreamTablesHeader => _dotNetStructureParsers.MetaDataStreamTablesHeader;

        /// <summary>
        ///     The SHA-256 hash sum of the binary.
        /// </summary>
        public string SHA256 
            => _sha256 ??= ComputeHash(RawFile, new SHA256Managed().ComputeHash);

        /// <summary>
        ///     The SHA-1 hash sum of the binary.
        /// </summary>
        public string SHA1 
            => _sha1 ??= ComputeHash(RawFile, new SHA1Managed().ComputeHash);

        /// <summary>
        ///     The MD5 of hash sum of the binary.
        /// </summary>
        public string MD5 
            => _md5 ??= ComputeHash(RawFile, new MD5CryptoServiceProvider().ComputeHash);

        /// <summary>
        ///     The Import Hash of the binary if any imports are
        ///     given else null;
        /// </summary>
        public string? ImpHash 
            => _impHash ??= new ImportHash(ImportedFunctions)?.ImpHash;

        /// <summary>
        ///     The Version ID of each module
        ///     if the PE is a CLR assembly.
        /// </summary>
        public List<Guid> ClrModuleVersionIds 
            => (_netGuids ??= new NetGuids(this)).ModuleVersionIds;

        /// <summary>
        ///     The COM TypeLib ID of the assembly, if specified,
        ///     and if the PE is a CLR assembly.
        /// </summary>
        public string ClrComTypeLibId 
            => (_netGuids ??= new NetGuids(this)).ComTypeLibId;

        /// <summary>
        ///     Returns the file size in bytes.
        /// </summary>
        public long FileSize => RawFile.Length;

        /// <summary>
        ///     Get an object which holds information about
        ///     the Certificate Revocation Lists of the signing
        ///     certificate if any is present.
        /// </summary>
        /// <returns>Certificate Revocation List information or null if binary is not signed.</returns>
        public CrlUrlList? GetCrlUrlList()
        {
            if (PKCS7 == null)
                return null;

            try
            {
                return new CrlUrlList(PKCS7);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Tests if a file is a PE file based on the MZ
        ///     header. It is not checked if the PE file is correct
        ///     in all other parts.
        /// </summary>
        /// <param name="file">Path to a possible PE file.</param>
        /// <returns>True if the MZ header is set.</returns>
        public static bool IsPEFile(string file)
        {
            var buffer = new byte[2];

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                fs.Read(buffer, 0, buffer.Length);
            }

            return IsPEFile(buffer);
        }

        /// <summary>
        ///     Tests is a buffer is a PE file based on the MZ
        ///     header. It is not checked if the PE file is correct
        ///     in all other parts.
        /// </summary>
        /// <param name="buf">Byte array containing a possible PE file.</param>
        /// <returns>True if the MZ header is set.</returns>
        public static bool IsPEFile(byte[] buf)
        {
            if (buf.Length < 2)
                return false;

            return buf[1] == 0x5a && buf[0] == 0x4d; // MZ Header
        }

        /// <summary>
        ///     Returns if the PE file is a EXE, DLL and which architecture
        ///     is used (32/64).
        ///     Architectures: "I386", "AMD64", ...
        ///     DllOrExe: "DLL", "EXE", "UNKNOWN"
        /// </summary>
        /// <returns>
        ///     A string "architecture_dllOrExe".
        ///     E.g. "AMD64_DLL", "ALPHA_EXE"
        /// </returns>
        public string GetFileType()
        {
            var fileType = ImageNtHeaders?.FileHeader.MachineResolved;

            if ((ImageNtHeaders?.FileHeader.Characteristics 
                 & (ushort)Constants.FileHeaderCharacteristics.IMAGE_FILE_DLL) != 0)
                fileType += "_DLL";
            else if ((ImageNtHeaders?.FileHeader.Characteristics 
                      & (ushort)Constants.FileHeaderCharacteristics.IMAGE_FILE_EXECUTABLE_IMAGE) != 0)
                fileType += "_EXE";
            else
                fileType += "_UNKNOWN";

            return fileType;
        }

        private string ComputeHash(IRawFile peFile, Func<Stream, byte[]> hashFunction)
        {
            var sBuilder = new StringBuilder();
            var hash = hashFunction.Invoke(peFile.ToStream());

            foreach (var t in hash)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }

        public void Dispose()
        {
            RawFile.Dispose();
        }
    }
}