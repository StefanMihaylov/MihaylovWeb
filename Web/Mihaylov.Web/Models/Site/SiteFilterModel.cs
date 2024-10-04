using System.Collections.Generic;
using System.Linq;

namespace Mihaylov.Web.Models.Site
{
    public class SiteFilterModel
    {
        public int? Page { get; set; }


        public string ToQueryString()
        {
            var dic = new Dictionary<string, string>();

            if (Page.HasValue)
            {
                dic.Add(nameof(Page), Page.Value.ToString());
            }



            var result = string.Join("&", dic.Select(kv => $"{kv.Key.ToLower()}={kv.Value}"));

            if (dic.Count > 0)
            {
                result = $"?{result}";
            }

            return result;
        }
    }
}
