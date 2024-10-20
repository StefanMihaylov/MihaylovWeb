﻿using System;
using FFMpegCore;
using Microsoft.Extensions.Options;
using Mihaylov.Site.Media.Interfaces;
using Mihaylov.Site.Media.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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

                result.Width = videoaInfo.PrimaryVideoStream?.Height ?? 0;
                result.Height = videoaInfo.PrimaryVideoStream?.Width ?? 0;
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

        public override void SaveThumbnail(string filePath, System.Drawing.Size size, string outputFile)
        {
            try
            {
                FFMpeg.Snapshot(filePath, outputFile, size, null);
            }
            catch (Exception)
            {
                using var resultImage = new Image<Rgba32>(size.Width, size.Height);
                resultImage.SaveAsBmp(outputFile);
            }
        }
    }
}
