using System;
using System.IO;
using FFMpegCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Api.Site.Data.Media.Interfaces;
using Mihaylov.Common;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;

namespace Mihaylov.Api.Site.Data.Media.Services
{
    public class VideoMediaService : BaseMediaService, IVideoMediaService, IBaseMediaService
    {
        private readonly IFileWrapper _fileWrapper;
        private readonly ILogger _logger;
        private readonly string _tempPath;

        public VideoMediaService(IFileWrapper fileWrapper, ILogger<VideoMediaService> logger, IOptions<FFMpegPathConfig> settings)
        {
            _fileWrapper = fileWrapper;
            _logger = logger;

            _tempPath = fileWrapper.CreateDirectory(Path.GetTempPath(), "MihaylovMedia");

            GlobalFFOptions.Configure(options =>
            {
                options.BinaryFolder = settings.Value.FFMpegPath;
                options.TemporaryFilesFolder = _tempPath;
            });
        }

        public override MediaInfoSizeModel GetSize(string filePath, bool calculateChecksum, Action<int> progress)
        {
            var videoSize = GetVideoSize(filePath);

            var result = new MediaInfoSizeModel
            {
                Size = videoSize?.Size,
                Lenght = videoSize?.Duration,
                PerceptualHash = null,
            };

            if (calculateChecksum)
            {
                try
                {
                    using var stream = _fileWrapper.GetStreamFile(filePath);
                    var checksum = GetCheckSum(stream, p => progress(p));

                    result.Checksum = checksum;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "File {FilePath} checksum failed, Error: {Message}.", filePath, ex.Message);
                }
            }
            else
            {
                progress(100);
            }

            return result;
        }

        protected override byte[] GetThumbnail(string filePath, SizeModel size)
        {
            const string ThumbnailExtension = "png";
            var outputFile = $"{_tempPath}/{Guid.NewGuid()}.{ThumbnailExtension}";

            try
            {
                var newSize = new System.Drawing.Size(size.Width, size.Height);
                FFMpeg.Snapshot(filePath, outputFile, newSize, null);

                using var stream = _fileWrapper.GetStreamFile(outputFile);
                using var outStream = new MemoryStream();
                stream.CopyTo(outStream);
                outStream.Seek(0, SeekOrigin.Begin);

                var bytes = outStream.ToArray();
                return bytes;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Get video Thumbnail failed. Message: {Message}", ex.Message);

                using var resultImage = new Image<Rgba32>(size.Width, size.Height);

                using var outStream = new MemoryStream();
                resultImage.Save(outStream, new BmpEncoder());
                outStream.Seek(0, SeekOrigin.Begin);

                var bytes = outStream.ToArray();

                return bytes;
            }
            finally
            {
                _fileWrapper.DeleteFile(outputFile);
            }
        }

        private VideoSizeModel GetVideoSize(string filePath)
        {
            try
            {
                var videoaInfo = FFProbe.Analyse(filePath);

                var width = videoaInfo.PrimaryVideoStream?.Height ?? 0;
                var height = videoaInfo.PrimaryVideoStream?.Width ?? 0;

                var result = new VideoSizeModel(new SizeModel(width, height), videoaInfo.Duration);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"File {filePath} Length failed, Error: {ex.Message}.");
                return null;
            }
        }
    }
}
