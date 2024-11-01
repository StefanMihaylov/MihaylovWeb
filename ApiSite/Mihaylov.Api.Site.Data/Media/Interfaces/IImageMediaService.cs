namespace Mihaylov.Api.Site.Data.Media.Interfaces
{
    public interface IImageMediaService : IBaseMediaService
    {
        bool IsImageFile(string fileExtension);
    }
}