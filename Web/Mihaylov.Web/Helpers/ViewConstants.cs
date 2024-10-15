using System.Collections.Generic;
using System.Linq;
using Mihaylov.Api.Site.Client;

namespace Mihaylov.Web.Helpers
{
    public static class ViewConstants
    {
        public static string Width100 = "width:100px";
        public static string Width120 = "width:120px";
        public static string Width200 = "width:200px";
        public static string Width500 = "width:500px";

        public static string DefaultValue = "-- Not Selected --";
        public static string DefaultValueShort = "-----";
        public static string EdithorClass = "form-control";

        public static string DateFormat = "{0:yyyy.MM.dd}";
        public static string DateTimeFormat = "{0:yyyy.MM.dd HH:mm:ss}";
        public static string DateFormatSimple = "yyyy.MM.dd";
        public static string DateTimeFormatSimple = "yyyy.MM.dd HH:mm:ss";


        public static string ToQueryString(this IGridRequest request)
        {
            var dic = new Dictionary<string, string>();

            AddParameters(request, dic);

            var result = string.Join("&", dic.Select(kv => $"{kv.Key.ToLower()}={kv.Value}"));

            if (dic.Count > 0)
            {
                result = $"?{result}";
            }

            return result;
        }

        private static void AddParameters(IGridRequest request, IDictionary<string, string> dic)
        {
            if (request.Page.HasValue)
            {
                dic.Add(nameof(request.Page), request.Page.Value.ToString());
            }

            if (!string.IsNullOrEmpty(request.AccountName))
            {
                dic.Add(nameof(request.AccountName), request.AccountName);
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                dic.Add(nameof(request.Name), request.Name);
            }

            if (request.StatusId.HasValue)
            {
                dic.Add(nameof(request.StatusId), request.StatusId.Value.ToString());
            }

            if (request.AccountTypeId.HasValue)
            {
                dic.Add(nameof(request.AccountTypeId), request.AccountTypeId.Value.ToString());
            }

            if (request.PersonId.HasValue)
            {
                dic.Add(nameof(request.PersonId), request.PersonId.Value.ToString());
            }

            if (request.IsNewPerson.HasValue)
            {
                dic.Add(nameof(request.IsNewPerson), request.IsNewPerson.Value.ToString());
            }
        }
    }
}
