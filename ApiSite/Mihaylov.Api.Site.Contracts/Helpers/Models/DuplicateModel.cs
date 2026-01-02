using System.Collections.Generic;

namespace Mihaylov.Api.Site.Contracts.Helpers.Models
{
    public class DuplicateModel
    {
        public string Checksum {  get; set; }

        public ulong? Hash { get; set; }

        public double Similarity { get; set; }

        public int Count { get; set; }

        public IEnumerable<MediaInfoModel> List { get; set; }
    }
}
