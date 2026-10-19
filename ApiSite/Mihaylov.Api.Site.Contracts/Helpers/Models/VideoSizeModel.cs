using System;

namespace Mihaylov.Api.Site.Contracts.Helpers.Models
{
    public class VideoSizeModel
    {
        public SizeModel Size { get; set; }

        public TimeSpan Duration { get; set; }

        public VideoSizeModel()
        {            
        }

        public VideoSizeModel(SizeModel size, TimeSpan duration)
        {
            Size = size;
            Duration = duration;
        }
    }
}
