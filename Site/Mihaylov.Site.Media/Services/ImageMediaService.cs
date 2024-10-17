using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using Mihaylov.Site.Media.Interfaces;
using Mihaylov.Site.Media.Models;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Mihaylov.Site.Media.Services
{
    public class ImageMediaService : BaseMediaService, IImageMediaService, IBaseMediaService
    {
        private const string BMP = "bmp";
        private const string WEBP = "webp";

        private readonly IEnumerable<string> _imageExtensions;

        protected override string ThumbnailExtension => WEBP;

        public ImageMediaService(IFileWrapper fileWrapper, IOptions<FileWrapperConfig> settings)
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
                using var image = Image.FromFile(filePath);

                var result = new MediaInfoSizeModel()
                {
                    Height = image.Height,
                    Width = image.Width,
                    Lenght = null,
                };

                if (calculateChecksum)
                {
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

        public override void SaveThumbnail(string filePath, Size size, string outputFile)
        {
            switch (ThumbnailExtension)
            {
                case BMP:
                    {
                        using var image = Image.FromFile(filePath);

                        var thumbImage = image.GetThumbnailImage(size.Width, size.Height, () => false, IntPtr.Zero);
                        thumbImage.Save(outputFile);
                        break;
                    }

                case WEBP:
                    {
                        using var inStream = _fileWrapper.GetStreamFile(filePath);
                        using var image = SixLabors.ImageSharp.Image.Load(inStream);
                        image.Mutate(x => x.Resize(size.Width, size.Height));

                        using var outStream = new MemoryStream();
                        image.Save(outStream, new WebpEncoder());
                        outStream.Seek(0, SeekOrigin.Begin);

                        _fileWrapper.SaveStreamFile(outStream, outputFile);
                        break;
                    }

                default:
                    throw new Exception($"The extension {ThumbnailExtension} is not supported");
            }
        }


        private string GetImageChecksum(Image image, Action<int> progress)
        {
            using var stream = GetStream(image);

            // using var hasher = SHA256.Create();
            // var hashBytes = hasher.ComputeHash(stream);
            // string convertedHash = MakeHashString(hashBytes);

            var hash = GetCheckSum(stream, progress);

            return hash;
        }


        private Stream GetStream(Image image)
        {
            var stream = new MemoryStream();
            image.Save(stream, image.RawFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        private IEnumerable<string> GetImageExtensions()
        {
            var extensions = ImageCodecInfo.GetImageEncoders()
                                    .SelectMany(codec => codec.FilenameExtension
                                                             .ToLowerInvariant()
                                                             .Split(';'))
                                    .ToArray();

            return extensions;
        }
    }
}
