using System.Collections.Generic;
using Mihaylov.Api.Site.Client;
using Mihaylov.Common.Generic.Extensions;
using Mihaylov.Web.Models.Configs;

namespace Mihaylov.Web.Models.Configs
{
    public static class ViewConstants
    {
        public const string Width100 = "width:105px";
        public const string Width120 = "width:120px";
        public const string Width150 = "width:150px";
        public const string Width200 = "width:200px";
        public const string Width500 = "width:500px";

        public const string NewButton = "bi-plus-circle";
        public const string MergeButton = "bi-union";
        public const string MergeClearButton = "bi-0-square";
        public const string EditButton = "bi-pencil-fill";
        public const string CloseButton = "bi-x-octagon";
        public const string SaveButton = "bi-floppy2";
        public const string ViewButton = "bi-binoculars";
        public const string SearchButton = "bi-search";
        public const string DeleteButton = "bi-eraser";
        public const string ListButton = "bi-list-ul";

        public const string DefaultDropDownValue = "--- Select ---";
        public const string DefaultDropDownValueShort = "-----";
        public const string EdithorClass = "form-control";

        public const string DateFormat = "{0:yyyy.MM.dd}";
        public const string DateTimeFormat = "{0:yyyy.MM.dd HH:mm:ss}";
        public const string DateFormatSimple = "yyyy.MM.dd";
        public const string DateTimeFormatSimple = "yyyy.MM.dd HH:mm:ss";

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
