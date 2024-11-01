using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Api.Site.Data.Media.Interfaces;
using Mihaylov.Common.Generic.Servises.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Mihaylov.Api.Site.Data.Media.Services
{
    public class ImageMediaService : BaseMediaService, IImageMediaService, IBaseMediaService
    {
        private const string BMP = "bmp";
        private const string WEBP = "webp";
        private const string PNG = "png";

        private readonly IEnumerable<string> _imageExtensions;

        protected override string ThumbnailExtension => WEBP;

        public ImageMediaService(IFileWrapper fileWrapper, IOptions<PathConfig> settings)
            : base(fileWrapper, settings)
        {
            _imageExtensions = GetImageExtensions();
        }

        public bool IsImageFile(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                return false;
            }

            return _imageExtensions.Contains($"*{fileExtension.ToLowerInvariant()}");
        }

        public override MediaInfoSizeModel GetSize(string filePath, bool calculateChecksum, Action<int> progress)
        {
            try
            {
                ImageInfo imageInfo = Image.Identify(filePath);

                var result = new MediaInfoSizeModel()
                {
                    Size = new SizeModel(imageInfo.Width, imageInfo.Height),
                    Lenght = null,
                };

                if (calculateChecksum)
                {
                    using var image = Image.Load(filePath);
                    var checksum = GetImageChecksum(image, progress);
                    result.Checksum = checksum;
                }
                else
                {
                    progress(100);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File {filePath} checksum failed, Error: {ex.Message}.");
            }

            return null;
        }

        protected override MemoryStream GetThumbnail(string filePath, SizeModel size, string outputFile)
        {
            IImageEncoder imageEncoder;

            switch (ThumbnailExtension)
            {
                case BMP:
                    {
                        imageEncoder = new BmpEncoder();
                        break;
                    }

                case WEBP:
                    {
                        imageEncoder = new WebpEncoder();
                        break;
                    }

                case PNG:
                    {
                        imageEncoder = new PngEncoder();
                        break;
                    }

                default:
                    throw new Exception($"The extension {ThumbnailExtension} is not supported");
            }

            using var image = Image.Load(filePath);
            image.Mutate(x => x.Resize(size.Width, size.Height));

            var outStream = new MemoryStream();
            image.Save(outStream, imageEncoder);
            outStream.Seek(0, SeekOrigin.Begin);

            return outStream;
        }

        private string GetImageChecksum(Image image, Action<int> progress)
        {
            using var stream = new MemoryStream();
            image.Save(stream, image.Metadata.DecodedImageFormat);
            stream.Seek(0, SeekOrigin.Begin);

            var hash = base.GetCheckSum(stream, progress);

            return hash;
        }

        private IEnumerable<string> GetImageExtensions()
        {
           var extensions = Configuration.Default
                                .ImageFormats
                                .SelectMany(x => x.FileExtensions)
                                .ToList();

            return extensions;
        }
    }
}
