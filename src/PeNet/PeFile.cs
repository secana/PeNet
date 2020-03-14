using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using PeNet.FileParser;
using PeNet.Header.Authenticode;
using PeNet.Header.ImpHash;
using PeNet.Header.Net;
using PeNet.Header.Pe;
using PeNet.Header.Resource;
using PeNet.HeaderParser.Authenticode;
using PeNet.HeaderParser.Net;
using PeNet.HeaderParser.Pe;

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
        /// <param name="buff">Buffer containing a possible PE file.</param>
        /// <param name="peFile">Parsed PE file or Null.</param>
        /// <returns>True if parable PE file and false if not.</returns>
        public static bool TryParse(byte[] buff, out PeFile? peFile)
        {
            peFile = null;

            if (!IsPeFile(buff))
                return false;

            try { peFile = new PeFile(buff); }
            catch { return false; }

            return true;
        }

        /// <summary>
        ///     Returns true if the DLL flag in the
        ///     File Header is set.
        /// </summary>
        public bool IsDll
            => ImageNtHeaders?.FileHeader?.Characteristics.HasFlag(FileCharacteristicsType.Dll) ?? false;


        /// <summary>
        ///     Returns true if the Executable flag in the
        ///     File Header is set.
        /// </summary>
        public bool IsExe
            => ImageNtHeaders?.FileHeader.Characteristics.HasFlag(FileCharacteristicsType.ExecutableImage) ?? false;

        /// <summary>
        ///     Returns true if the PE file is a system driver
        ///     based on the Subsytem = 0x1 value in the Optional Header.
        /// </summary>
        public bool IsDriver => ImageNtHeaders?.OptionalHeader.Subsystem == SubsystemType.Native
                                && ImportedFunctions.FirstOrDefault(i => i.DLL == "ntoskrnl.exe") != null;

        /// <summary>
        /// Returns true if the PE file is a .NET assembly.
        /// </summary>
        public bool IsDotNet => ImageComDescriptor != null;

        /// <summary>
        ///     Returns true if the PE file is signed. It
        ///     does not check if the signature is valid!
        /// </summary>
        public bool IsSigned => Pkcs7 != null;

        /// <summary>
        ///     Returns true if the PE file signature is valid signed.
        /// </summary>
        public bool HasValidSignature => Authenticode?.IsAuthenticodeValid ?? false;

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="useOnlineCrl">Check certificate chain online or offline.</param>
        /// <returns>True if cert chain is valid and from a trusted CA.</returns>
        public bool HasValidCertChain(bool useOnlineCrl)
            => Authenticode?.SigningCertificate != null 
                   && HasValidCertChain(Authenticode.SigningCertificate, new TimeSpan(0,0,0,10), useOnlineCrl);

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="cert">X509 Certificate</param>
        /// <param name="urlRetrievalTimeout">Timeout to validate the certificate online.</param>
        /// <param name="useOnlineCRL">If true, uses online certificate revocation lists, else on the local CRL.</param>
        /// <param name="excludeRoot">True if the root certificate should not be validated. False if the whole chain should be validated.</param>
        /// <returns>True if cert chain is valid and from a trusted CA.</returns>
        public bool HasValidCertChain(X509Certificate2? cert, TimeSpan urlRetrievalTimeout, bool useOnlineCRL = true, bool excludeRoot = true)
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
        ///     Access the ImageDosHeader of the PE file.
        /// </summary>
        public ImageDosHeader? ImageDosHeader => _nativeStructureParsers.ImageDosHeader;

        /// <summary>
        ///     Access the ImageNtHeaders of the PE file.
        /// </summary>
        public ImageNtHeaders? ImageNtHeaders => _nativeStructureParsers.ImageNtHeaders;

        /// <summary>
        ///     Access the ImageSectionHeader of the PE file.
        /// </summary>
        public ImageSectionHeader[]? ImageSectionHeaders => _nativeStructureParsers.ImageSectionHeaders;

        /// <summary>
        ///     Access the ImageExportDirectory of the PE file.
        /// </summary>
        public ImageExportDirectory? ImageExportDirectory => _dataDirectoryParsers.ImageExportDirectories;

        /// <summary>
        ///     Access the ImageImportDescriptor array of the PE file.
        /// </summary>
        public ImageImportDescriptor[]? ImageImportDescriptors => _dataDirectoryParsers.ImageImportDescriptors;

        /// <summary>
        ///     Access the ImageBaseRelocation array of the PE file.
        /// </summary>
        public ImageBaseRelocation[]? ImageRelocationDirectory => _dataDirectoryParsers.ImageBaseRelocations;

        /// <summary>
        ///     Access the ImageDebugDirectory of the PE file.
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
        ///     Access the ImageResourceDirectory of the PE file.
        /// </summary>
        public ImageResourceDirectory? ImageResourceDirectory => _dataDirectoryParsers.ImageResourceDirectory;

        /// <summary>
        ///     Access resources of the PE file.
        /// </summary>
        public Resources? Resources => _dataDirectoryParsers.Resources;

        /// <summary>
        ///     Access the array of RuntimeFunction from the Exception header.
        /// </summary>
        public RuntimeFunction[]? ExceptionDirectory => _dataDirectoryParsers.RuntimeFunctions;

        /// <summary>
        ///     Access the WinCertificate from the Security header.
        /// </summary>
        public WinCertificate? WinCertificate => _dataDirectoryParsers.WinCertificate;

        /// <summary>
        /// Access the IMAGE_BOUND_IMPORT_DESCRIPTOR form the data directory.
        /// </summary>
        public ImageBoundImportDescriptor? ImageBoundImportDescriptor => _dataDirectoryParsers.ImageBoundImportDescriptor;

        /// <summary>
        /// Access the IMAGE_TLS_DIRECTORY from the data directory.
        /// </summary>
        public ImageTlsDirectory? ImageTlsDirectory => _dataDirectoryParsers.ImageTlsDirectory;

        /// <summary>
        /// Access the ImageDelayImportDirectory from the data directory.
        /// </summary>
        public ImageDelayImportDescriptor? ImageDelayImportDescriptor => _dataDirectoryParsers.ImageDelayImportDescriptor;

        /// <summary>
        /// Access the ImageLoadConfigDirectory from the data directory.
        /// </summary>
        public ImageLoadConfigDirectory? ImageLoadConfigDirectory => _dataDirectoryParsers.ImageLoadConfigDirectory;

        /// <summary>
        /// Access the ImageCor20Header (COM Descriptor/CLI) from the data directory.
        /// </summary>
        public ImageCor20Header? ImageComDescriptor => _dataDirectoryParsers.ImageComDescriptor;

        /// <summary>
        ///     Signing X509 certificate if the binary was signed with
        /// </summary>
        public X509Certificate2? Pkcs7 => Authenticode?.SigningCertificate;

        /// <summary>
        ///     Access the MetaDataHdr from the COM/CLI header.
        /// </summary>
        public MetaDataHdr? MetaDataHdr => _dotNetStructureParsers.MetaDataHdr;

        /// <summary>
        /// Meta Data Stream #String.
        /// </summary>
        public MetaDataStreamString? MetaDataStreamString => _dotNetStructureParsers.MetaDataStreamString;

        /// <summary>
        /// Meta Data Stream #US (User strings).
        /// </summary>
        public MetaDataStreamUs? MetaDataStreamUs => _dotNetStructureParsers.MetaDataStreamUs;

        /// <summary>
        /// Meta Data Stream #GUID.
        /// </summary>
        public MetaDataStreamGuid? MetaDataStreamGuid => _dotNetStructureParsers.MetaDataStreamGuid;

        /// <summary>
        /// Meta Data Stream #Blob as an byte array.
        /// </summary>
        public byte[]? MetaDataStreamBlob => _dotNetStructureParsers.MetaDataStreamBlob;

        /// <summary>
        ///     Access the Meta Data Stream Tables Header from the list of
        ///     Meta Data Streams of the .Net header.
        /// </summary>
        public MetaDataTablesHdr? MetaDataStreamTablesHeader => _dotNetStructureParsers.MetaDataStreamTablesHeader;

        /// <summary>
        ///     The SHA-256 hash sum of the binary.
        /// </summary>
        public string Sha256 
            => _sha256 ??= ComputeHash(RawFile, new SHA256Managed().ComputeHash);

        /// <summary>
        ///     The SHA-1 hash sum of the binary.
        /// </summary>
        public string Sha1 
            => _sha1 ??= ComputeHash(RawFile, new SHA1Managed().ComputeHash);

        /// <summary>
        ///     The MD5 of hash sum of the binary.
        /// </summary>
        public string Md5 
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
            if (Pkcs7 == null)
                return null;

            try
            {
                return new CrlUrlList(Pkcs7);
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
        public static bool IsPeFile(string file)
        {
            var buffer = new byte[2];

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                fs.Read(buffer, 0, buffer.Length);
            }

            return IsPeFile(buffer);
        }

        /// <summary>
        ///     Tests is a buffer is a PE file based on the MZ
        ///     header. It is not checked if the PE file is correct
        ///     in all other parts.
        /// </summary>
        /// <param name="buf">Byte array containing a possible PE file.</param>
        /// <returns>True if the MZ header is set.</returns>
        public static bool IsPeFile(byte[] buf)
        {
            if (buf.Length < 2)
                return false;

            return buf[1] == 0x5a && buf[0] == 0x4d; // MZ Header
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