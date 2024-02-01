using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Mihaylov.Site.Media.Interfaces;
using Mihaylov.Site.Media.Models;

namespace Mihaylov.Site.Media.Services
{
    public abstract class BaseMediaService : IBaseMediaService
    {
        protected readonly FileWrapperConfig _config;
        protected readonly IFileWrapper _fileWrapper;

        public BaseMediaService(IFileWrapper fileWrapper, IOptions<FileWrapperConfig> settings)
        {
            _config = settings.Value;
            _fileWrapper = fileWrapper;

            if (!Directory.Exists(_config.TempPath))
            {
                Directory.CreateDirectory(_config.TempPath);
            }
        }

        public abstract MediaInfoSizeModel GetSize(string filePath, bool calculateChecksum, Action<int> progress);

        public abstract void SaveThumbnail(string filePath, Size size, string outputFile);

        protected abstract string ThumbnailExtension { get; }

        public byte[] GetThumbnail(string filePath, int width, int height, int thumbnailWidth)
        {
            var outputFile = $"{_config.TempPath}/{Guid.NewGuid()}.{ThumbnailExtension}";
            Size size = CalculateSize(width, height, thumbnailWidth);

            SaveThumbnail(filePath, size, outputFile);

            var bytes = _fileWrapper.ReadFile(outputFile);
            _fileWrapper.DeleteFile(outputFile);

            return bytes;
        }

        protected Size CalculateSize(int width, int height, int thumbnailWidth)
        {
            if (height == 0 || width == 0)
            {
                return new Size(thumbnailWidth, thumbnailWidth);
            }

            var ratio = (decimal)height / width;
            var resizedHeight = (int)Math.Round(thumbnailWidth * ratio);

            return new Size(thumbnailWidth, resizedHeight);
        }

        protected string GetCheckSum(Stream stream, Action<int> progress)
        {
            stream.Seek(0, SeekOrigin.Begin);

            byte[] buffer;
            int bytesRead;
            long totalBytesRead = 0;
            long size = stream.Length;

            using var hasher = SHA256.Create();

            do
            {
                buffer = new byte[4096];
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                totalBytesRead += bytesRead;

                hasher.TransformBlock(buffer, 0, bytesRead, null, 0);

                progress(totalBytesRead.GetPersentage(size));

            } while (bytesRead != 0);

            hasher.TransformFinalBlock(buffer, 0, 0);
            var hash = MakeHashString(hasher.Hash);

            return hash;
        }

        private string MakeHashString(byte[] bytes)
        {
            var hash = BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();

            return hash;
        }
    }
}
