namespace Mihaylov.Site.Media.Interfaces
{
    public interface IImageMediaService : IBaseMediaService
    {
        bool IsImageFile(string fileExtension);
    }
}