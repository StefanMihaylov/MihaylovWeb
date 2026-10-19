using System;
using System.IO;
using System.Security.Cryptography;
using Mihaylov.Api.Site.Contracts.Helpers.Models;
using Mihaylov.Api.Site.Data.Media.Interfaces;
using Mihaylov.Common;

namespace Mihaylov.Api.Site.Data.Media.Services
{
    public abstract class BaseMediaService : IBaseMediaService
    {
        public abstract MediaInfoSizeModel GetSize(string filePath, bool calculateChecksum, Action<int> progress);

        public byte[] GetThumbnail(string filePath, SizeModel inputSize, int thumbnailWidth)
        {
            SizeModel size = CalculateSize(inputSize, thumbnailWidth);
            var bytes = GetThumbnail(filePath, size);

            return bytes;
        }


        protected abstract byte[] GetThumbnail(string filePath, SizeModel inputSize);

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
            var hash = BitConverter.ToString(hasher.Hash).Replace("-", string.Empty).ToLower();

            return hash;
        }
    }
}
