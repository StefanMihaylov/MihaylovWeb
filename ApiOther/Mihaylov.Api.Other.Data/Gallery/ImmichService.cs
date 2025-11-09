using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Other.Contracts.Gallery.Interfaces;
using Mihaylov.Api.Other.Contracts.Gallery.Models;
using Mihaylov.Api.Other.Data.Gallery.Models;
using Mihaylov.Common.Generic.Servises;
using Mihaylov.Common.Generic.Servises.Models;

namespace Mihaylov.Api.Other.Data.Gallery
{
    public class ImmichService : HttpClientExtention, IImmichService
    {
        // documentation https://api.immich.app/introduction
        public const string IMMICH_CLIENT_NAME = "ImmichHttpClientName";

        private readonly ImmichConfig _config;

        public ImmichService(IHttpClientFactory factory, ILoggerFactory loggerFactory,
            IOptions<ImmichConfig> settings)
            : base(factory, loggerFactory, IMMICH_CLIENT_NAME,
                  new ApiConfig(settings.Value.BaseUrl, null, null, settings.Value.ApiKey))
        {
            _config = settings.Value;
        }

        public async Task<IEnumerable<AlbumModel>> GetAlbumsAsync()
        {
            var sharedlinksReponse = await GetSharedLinksAsync().ConfigureAwait(false);
            if (!sharedlinksReponse.IsSuccessful)
            {
                _logger.LogError($"GetSharedLinks failed. Error message: {sharedlinksReponse.ErrorMessage}");
                return new List<AlbumModel>();
            }

            var sharedLinks = sharedlinksReponse.Data
                                    .Where(s => s.Type.Equals("ALBUM", System.StringComparison.InvariantCultureIgnoreCase))
                                    .GroupBy(s => s.AlbumId)
                                    .Select(g => g.OrderByDescending(s => s.CreatedAt).First())
                                    .ToList();

            var albumsReponse = await GetSharedAlbumsAsync().ConfigureAwait(false);
            if (!albumsReponse.IsSuccessful)
            {
                _logger.LogError($"GetSharedAlbums failed. Error message: {albumsReponse.ErrorMessage}");
                return new List<AlbumModel>();
            }

            var albums = albumsReponse.Data.ToDictionary(a => a.Id, a => a);

            var result = new List<AlbumModel>();
            foreach (var link in sharedLinks)
            {
                albums.TryGetValue(link.AlbumId, out var album);

                result.Add(new AlbumModel()
                {
                    AlbumLink = $"{_config.BaseUrl}/share/{link.Key}",
                    Name = album?.AlbumName,
                    Description = album?.Description,
                    StartDate = album?.StartDate ?? DateTime.MinValue,
                    EndDate = album?.EndDate ?? DateTime.MinValue,
                    AssetCount = album?.AssetCount ?? 0,
                    ThumbnailAssetId = album?.AlbumThumbnailAssetId
                });
            }

            return result.OrderByDescending(a => a.StartDate);
        }
       
        public async Task<Stream> GetThumbnailAsync(string id)
        {
            var fileResponse = await DownloadFileAsync<string>($"/api/assets/{id}/thumbnail", null, "GetThumbnail").ConfigureAwait(false);
            if (!fileResponse.IsSuccessful)
            {
                _logger.LogError($" GetThumbnail failed. Error message: {fileResponse.ErrorMessage}");
                return null;
            }

            return fileResponse.Data;
        }

        private async Task<Response<IEnumerable<ImmichSharedLinkModel>>> GetSharedLinksAsync()
        {
            var response = await GetResponse<string, IEnumerable<ImmichSharedLinkResponse>, string, IEnumerable<ImmichSharedLinkModel>>(
                HttpMethod.Get, $"/api/shared-links", null, "SharedLinks",
                x =>
                {
                    var list = new List<ImmichSharedLinkModel>();
                    foreach (var item in x)
                    {
                        list.Add(new ImmichSharedLinkModel()
                        {
                            Key = item.Key,
                            Type = item.Type,
                            CreatedAt = item.CreatedAt,
                            AlbumId = item.Album?.Id,
                        });
                    }

                    return list;
                },
                e => e).ConfigureAwait(false);

            return response;
        }

        private async Task<Response<IEnumerable<ImmichAlbum>>> GetSharedAlbumsAsync()
        {
            var response = await GetResponse<string, IEnumerable<ImmichAlbum>, string, IEnumerable<ImmichAlbum>>(
                HttpMethod.Get, $"/api/albums?shared=true", null, "SharedAlbums",
                x => x, e => e).ConfigureAwait(false);

            return response;
        }
    }
}
