using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Api.Site.Data.Media.Interfaces;
using Mihaylov.Common;
using Mihaylov.Common.Generic.Servises.Interfaces;

namespace Mihaylov.Api.Site.Data.Media.Services
{
    public abstract class BaseMediaService : IBaseMediaService
    {
        protected readonly PathConfig _config;
        protected readonly IFileWrapper _fileWrapper;

        public BaseMediaService(IFileWrapper fileWrapper, IOptions<PathConfig> settings)
        {
            _config = settings.Value;
            _fileWrapper = fileWrapper;

            if (!Directory.Exists(_config.TempPath))
            {
                Directory.CreateDirectory(_config.TempPath);
            }
        }

        public abstract MediaInfoSizeModel GetSize(string filePath, bool calculateChecksum, Action<int> progress);

        protected abstract MemoryStream GetThumbnail(string filePath, SizeModel size, string outputFile);

        protected abstract string ThumbnailExtension { get; }


        public byte[] GetThumbnail(string filePath, SizeModel inputSize, int thumbnailWidth)
        {
            var outputFile = $"{_config.TempPath}/{Guid.NewGuid()}.{ThumbnailExtension}";
            SizeModel size = CalculateSize(inputSize, thumbnailWidth);

            using var imageStream = GetThumbnail(filePath, size, outputFile);
            var bytes = imageStream.ToArray();

            _fileWrapper.DeleteFile(outputFile);

            return bytes;
        }

        protected SizeModel CalculateSize(SizeModel size, int thumbnailWidth)
        {
            if (size == null || size.Height == 0 || size.Width == 0)
            {
                return new SizeModel(thumbnailWidth, thumbnailWidth);
            }

            var ratio = (decimal)size.Height / size.Width;
            var resizedHeight = (int)Math.Round(thumbnailWidth * ratio);

            return new SizeModel(thumbnailWidth, resizedHeight);
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
