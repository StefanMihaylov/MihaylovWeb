using System;
using System.IO;
using FFMpegCore;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Api.Site.Data.Media.Interfaces;
using Mihaylov.Common.Generic.Servises.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;

namespace Mihaylov.Api.Site.Data.Media.Services
{
    public class VideoMediaService : BaseMediaService, IVideoMediaService, IBaseMediaService
    {
        protected override string ThumbnailExtension => "png";

        public VideoMediaService(IFileWrapper fileWrapper, IOptions<PathConfig> settings)
            : base(fileWrapper, settings)
        {
            GlobalFFOptions.Configure(options =>
            {
                options.BinaryFolder = _config.FFMpegPath;
                options.TemporaryFilesFolder = _config.TempPath;
            });
        }

        public override MediaInfoSizeModel GetSize(string filePath, bool calculateChecksum, Action<int> progress)
        {
            var result = new MediaInfoSizeModel();

            try
            {
                var videoaInfo = FFProbe.Analyse(filePath);

                var width = videoaInfo.PrimaryVideoStream?.Height ?? 0;
                var height = videoaInfo.PrimaryVideoStream?.Width ?? 0;

                result.Size = new SizeModel(width, height);
                result.Lenght = videoaInfo.Duration;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File {filePath} Length failed, Error: {ex.Message}.");
            }

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
                    Console.WriteLine($"File {filePath} checksum failed, Error: {ex.Message}.");
                }
            }
            else
            {
                progress(100);
            }

            return result;
        }

        protected override MemoryStream GetThumbnail(string filePath, SizeModel size, string outputFile)
        {
            try
            {
                FFMpeg.Snapshot(filePath, outputFile, size.Convert(), null);

               using var stream = _fileWrapper.GetStreamFile(outputFile);

                var outStream = new MemoryStream();
                stream.CopyTo(outStream);
                outStream.Seek(0, SeekOrigin.Begin);

                return outStream;
            }
            catch (Exception)
            {
                using var resultImage = new Image<Rgba32>(size.Width, size.Height);

                var outStream = new MemoryStream();
                resultImage.Save(outStream, new BmpEncoder());
                outStream.Seek(0, SeekOrigin.Begin);

                return outStream;
            }
        }
    }
}
