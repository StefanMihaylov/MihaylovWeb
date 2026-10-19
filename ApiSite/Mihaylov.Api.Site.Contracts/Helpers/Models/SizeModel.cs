namespace Mihaylov.Api.Site.Contracts.Helpers.Models
{
    public class SizeModel
    {
        public int Width { get; set; }
        
        public int Height { get; set; }

        public SizeModel(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
