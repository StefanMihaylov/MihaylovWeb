using System.ComponentModel.DataAnnotations;
using Mihaylov.Common;

namespace Mihaylov.UI.Mvc.Site.Areas.Site.Models.Site
{
    public class MergeRequestModel : IQueryRequest
    {
        [Required]
        public long? From { get; set; }

        [Required]
        public long? To { get; set; }

        public MergeRequestModel()
        {
        }

        public MergeRequestModel(long? from, long? to)
        {
            From = from;
            To = to;
        }
    }
}
