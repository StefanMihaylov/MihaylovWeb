using System;
using System.Linq;
using CsQuery;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Site.Core.CsQuery
{
    public class CsQueryWrapper : ICsQueryWrapper
    {
        public Person GetInfo(string url, string username)
        {
            CQ dom = CQ.CreateFromUrl($"{url}/{username}");
            CQ bioContainer = dom.Select("div.miniBio ul li");

            if (bioContainer.Length == 0)
            {
                if (dom.Select("#main-content").Length > 0)
                {
                    return new Person()
                    {
                        Username = username,
                        IsAccountDisabled = true,
                        Ethnicity = null,
                        EthnicityTypeId = 0,
                        Orientation = null,
                        OrientationTypeId = 0,
                        Country = null,
                        CountryId = 0,
                    };
                }
            }

            var attributes = bioContainer.Find("span.desc");

            string createdDate = GetAttributeValue(attributes, "Member since:");
            string lastBroadcastDate = GetAttributeValue(attributes, "Last Broadcast:");
            string ethnicity = GetAttributeValue(attributes, "Ethnicity:");
            string orientation = GetAttributeValue(attributes, "Orientation:");
            string age = GetAttributeValue(attributes, "Age:");
            string country = GetAttributeValue(attributes, "Location:");

            var person = new Person()
            {
                Username = username,
                CreateDate = ParseDate(createdDate),
                LastBroadcastDate = ParseDate(lastBroadcastDate),
                Age = int.Parse(age),
                Ethnicity = ethnicity,
                EthnicityTypeId = 0,
                Orientation = orientation,
                OrientationTypeId = 0,
                Country = country,
                CountryId = 0,
                IsAccountDisabled = false,
            };

            return person;
        }

        private DateTime ParseDate(string createdDate)
        {
            if (string.IsNullOrWhiteSpace(createdDate))
            {
                return default(DateTime);
            }

            return DateTime.Parse(createdDate);
        }

        private string GetAttributeValue(CQ attributes, string description)
        {
            var attribute = attributes.Where(l => l.InnerText == description)
                                      .FirstOrDefault();
            if (attribute == null)
            {
                return null;
            }

            string value = attribute.ParentNode
                                    .Cq()
                                    .Find(".field")
                                    .Text();
            return value;
        }
    }
}
