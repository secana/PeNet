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
using PeNet.Utilities;

namespace PeNet
{
    /// <summary>
    ///     This class represents a Portable Executable (PE) file and makes the different
    ///     header and properties accessible.
    /// </summary>
    public class PeFile
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

            if (!IsPEFile(buff))
                return false;

            try { peFile = new PeFile(buff); }
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
                               == (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_I386;
        /// <summary>
        ///     Access the IMAGE_DOS_HEADER of the PE file.
        /// </summary>
        public IMAGE_DOS_HEADER? ImageDosHeader => _nativeStructureParsers.ImageDosHeader;

        /// <summary>
        ///     Access the IMAGE_NT_HEADERS of the PE file.
        /// </summary>
        public IMAGE_NT_HEADERS? ImageNtHeaders => _nativeStructureParsers.ImageNtHeaders;

        /// <summary>
        ///     Access the IMAGE_SECTION_HEADERS of the PE file.
        /// </summary>
        public IMAGE_SECTION_HEADER[]? ImageSectionHeaders => _nativeStructureParsers.ImageSectionHeaders;

        /// <summary>
        ///     Access the IMAGE_EXPORT_DIRECTORY of the PE file.
        /// </summary>
        public IMAGE_EXPORT_DIRECTORY? ImageExportDirectory => _dataDirectoryParsers.ImageExportDirectories;

        /// <summary>
        ///     Access the IMAGE_IMPORT_DESCRIPTOR array of the PE file.
        /// </summary>
        public IMAGE_IMPORT_DESCRIPTOR[]? ImageImportDescriptors => _dataDirectoryParsers.ImageImportDescriptors;

        /// <summary>
        ///     Access the IMAGE_BASE_RELOCATION array of the PE file.
        /// </summary>
        public IMAGE_BASE_RELOCATION[]? ImageRelocationDirectory => _dataDirectoryParsers.ImageBaseRelocations;

        /// <summary>
        ///     Access the IMAGE_DEBUG_DIRECTORY of the PE file.
        /// </summary>
        public IMAGE_DEBUG_DIRECTORY[]? ImageDebugDirectory => _dataDirectoryParsers.ImageDebugDirectory;

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
        public IMAGE_RESOURCE_DIRECTORY? ImageResourceDirectory => _dataDirectoryParsers.ImageResourceDirectory;

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
        public IMAGE_BOUND_IMPORT_DESCRIPTOR? ImageBoundImportDescriptor => _dataDirectoryParsers.ImageBoundImportDescriptor;

        /// <summary>
        /// Access the IMAGE_TLS_DIRECTORY from the data directory.
        /// </summary>
        public IMAGE_TLS_DIRECTORY? ImageTlsDirectory => _dataDirectoryParsers.ImageTlsDirectory;

        /// <summary>
        /// Access the IMAGE_DELAY_IMPORT_DESCRIPTOR from the data directory.
        /// </summary>
        public IMAGE_DELAY_IMPORT_DESCRIPTOR? ImageDelayImportDescriptor => _dataDirectoryParsers.ImageDelayImportDescriptor;

        /// <summary>
        /// Access the IMAGE_LOAD_CONFIG_DIRECTORY from the data directory.
        /// </summary>
        public IMAGE_LOAD_CONFIG_DIRECTORY? ImageLoadConfigDirectory => _dataDirectoryParsers.ImageLoadConfigDirectory;

        /// <summary>
        /// Access the IMAGE_COR20_HEADER (COM Descriptor/CLI) from the data directory.
        /// </summary>
        public IMAGE_COR20_HEADER? ImageComDescriptor => _dataDirectoryParsers.ImageComDescriptor;

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
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="online">Check certificate chain online or off-line.</param>
        /// <returns>True of cert chain is valid and from a trusted CA.</returns>
        public bool IsValidCertChain(bool online)
        {
            return IsSigned && SignatureInformation.IsValidCertChain(PKCS7, online);
        }

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
            var fileType = ImageNtHeaders?.FileHeader.Machine switch
            {
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_I386 => "I386",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_I860 => "I860",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_R3000 => "R3000",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_R4000 => "R4000",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_R10000 => "R10000",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_WCEMIPSV2 => "WCEMIPSV2",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_OLDALPHA => "OLDALPHA",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ALPHA => "ALPHA",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH3 => "SH3",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH3DSP => "SH3DSP",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH3E => "SH3E",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH4 => "SH4",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH5 => "SH5",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ARM => "ARM",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_THUMB => "THUMB",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_AM33 => "M33",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_POWERPC => "POWERPC",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_POWERPCFP => "POWERPCFP",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_IA64 => "IA64",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_MIPS16 => "MIPS16",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_M68K => "M68K",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ALPHA64 => "ALPHA64",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_MIPSFPU => "MIPSFPU",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_MIPSFPU16 => "MIPSFPU16",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_TRICORE => "TRICORE",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_CEF => "CEF",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_EBC => "EBC",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_AMD64 => "AMD64",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_M32R => "M32R",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_CEE => "CEE",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ARM64 => "ARM64",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ARMNT => "ARMNT",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_TARGET_HOST => "TARGETHOST",
                _ => "UNKNOWN"
            };

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
    }
}