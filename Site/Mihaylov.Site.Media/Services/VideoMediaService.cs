using System;
using System.Drawing;
using FFMpegCore;
using Microsoft.Extensions.Options;
using Mihaylov.Site.Media.Interfaces;
using Mihaylov.Site.Media.Models;

namespace Mihaylov.Site.Media.Services
{
    public class VideoMediaService : BaseMediaService, IVideoMediaService, IBaseMediaService
    {
        protected override string ThumbnailExtension => "png";

        public VideoMediaService(IFileWrapper fileWrapper, IOptions<FileWrapperConfig> settings)
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

                result.Width = videoaInfo.PrimaryVideoStream?.Width ?? 0;
                result.Height = videoaInfo.PrimaryVideoStream?.Height ?? 0;
                result.Lenght = videoaInfo.Duration;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File {filePath} Lenght failed, Error: {ex.Message}.");
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

        public override void SaveThumbnail(string filePath, Size size, string outputFile)
        {
            FFMpeg.Snapshot(filePath, outputFile, size, null);
        }
    }
}
