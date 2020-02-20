using PeNet.Parser;
using PeNet.Structures;
using System.IO;

namespace PeNet
{
    internal class NativeStructureParsers
    {
        private readonly Stream _peFile;
        private readonly ImageDosHeaderParser _imageDosHeaderParser;
        private readonly ImageNtHeadersParser? _imageNtHeadersParser;
        private readonly ImageSectionHeadersParser? _imageSectionHeadersParser;

        internal NativeStructureParsers(Stream peFile)
        {
            _peFile = peFile;

            // Init all parsers
            _imageDosHeaderParser = InitImageDosHeaderParser();
            _imageNtHeadersParser = InitNtHeadersParser();
            _imageSectionHeadersParser = InitImageSectionHeadersParser();
        }

        public IMAGE_DOS_HEADER? ImageDosHeader => _imageDosHeaderParser.GetParserTarget();
        public IMAGE_NT_HEADERS? ImageNtHeaders => _imageNtHeadersParser?.GetParserTarget();
        public IMAGE_SECTION_HEADER[]? ImageSectionHeaders => _imageSectionHeadersParser?.GetParserTarget();


        private ImageSectionHeadersParser? InitImageSectionHeadersParser()
        {
            uint GetSecHeaderOffset()
            {
                var x = (uint)ImageNtHeaders!.FileHeader.SizeOfOptionalHeader + 0x18;
                return ImageDosHeader!.e_lfanew + x;
            }

            if (ImageNtHeaders is null || ImageDosHeader is null)
                return null;

            return new ImageSectionHeadersParser(
                _peFile, GetSecHeaderOffset(), 
                ImageNtHeaders.FileHeader.NumberOfSections, 
                ImageNtHeaders.OptionalHeader.ImageBase
                );
        }

        private ImageNtHeadersParser? InitNtHeadersParser()
        {
            if (ImageDosHeader is null)
                return null;

            return new ImageNtHeadersParser(_peFile, ImageDosHeader.e_lfanew);
        }

        private ImageDosHeaderParser InitImageDosHeaderParser()
        {
            return new ImageDosHeaderParser(_peFile, 0);
        }
    }
}