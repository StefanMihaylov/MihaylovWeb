using System.Collections.Generic;
using Mihaylov.Api.Site.Client;
using Mihaylov.Common.Generic.Extensions;
using Mihaylov.Web.Models.Configs;

namespace Mihaylov.Web.Models.Configs
{
    public static class ViewConstants
    {
        public static string Width100 = "width:105px";
        public static string Width120 = "width:120px";
        public static string Width150 = "width:150px";
        public static string Width200 = "width:200px";
        public static string Width500 = "width:500px";

        public static string NewButton = "bi-plus-circle";
        public static string MergeButton = "bi-union";
        public static string MergeClearButton = "bi-0-square";
        public static string EditButton = "bi-pencil-fill";
        public static string CloseButton = "bi-x-octagon";
        public static string SaveButton = "bi-floppy2";
        public static string ViewButton = "bi-binoculars";
        public static string SearchButton = "bi-search";
        public static string DeleteButton = "bi-eraser";

        public static string DefaultDropDownValue = "--- Select ---";
        public static string DefaultDropDownValueShort = "-----";
        public static string EdithorClass = "form-control";

        public static string DateFormat = "{0:yyyy.MM.dd}";
        public static string DateTimeFormat = "{0:yyyy.MM.dd HH:mm:ss}";
        public static string DateFormatSimple = "yyyy.MM.dd";
        public static string DateTimeFormatSimple = "yyyy.MM.dd HH:mm:ss";

        public static string ToQueryString(this IGridRequest request)
        {
            return GetParameters(request).ToQueryString();
        }

        private static IDictionary<string, string> GetParameters(IGridRequest request)
        {
            var dictionary = new Dictionary<string, string>();

            if (request.Page.HasValue)
            {
                dictionary.Add(nameof(request.Page), request.Page.Value.ToString());
            }

            if (!string.IsNullOrEmpty(request.AccountName))
            {
                dictionary.Add(nameof(request.AccountName), request.AccountName);
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                dictionary.Add(nameof(request.Name), request.Name);
            }

            if (request.StatusId.HasValue)
            {
                dictionary.Add(nameof(request.StatusId), request.StatusId.Value.ToString());
            }

            if (request.AccountTypeId.HasValue)
            {
                dictionary.Add(nameof(request.AccountTypeId), request.AccountTypeId.Value.ToString());
            }

            if (request.PersonId.HasValue)
            {
                dictionary.Add(nameof(request.PersonId), request.PersonId.Value.ToString());
            }

            if (request.IsNewPerson.HasValue)
            {
                dictionary.Add(nameof(request.IsNewPerson), request.IsNewPerson.Value.ToString());
            }

            return dictionary;
        }
    }
}
