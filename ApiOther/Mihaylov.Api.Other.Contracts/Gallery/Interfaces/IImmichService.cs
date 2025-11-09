using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Mihaylov.Api.Other.Contracts.Gallery.Models;

namespace Mihaylov.Api.Other.Contracts.Gallery.Interfaces
{
    public interface IImmichService
    {
        Task<IEnumerable<AlbumModel>> GetAlbumsAsync();

        Task<Stream> GetThumbnailAsync(string id);
    }
}