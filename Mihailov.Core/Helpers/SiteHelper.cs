using System;
using System.Linq;

using CsQuery;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Core.Interfaces;

namespace Mihaylov.Core.Helpers
{
    public class SiteHelper : ISiteHelper
    {
        private readonly string url;
        private readonly ICountriesManager countriesManager;
        private readonly IEthnicitiesManager ethnicitiesManager;
        private readonly IOrientationsManager orientationsManager;


        public SiteHelper(string url, ICountriesManager countriesManager, IEthnicitiesManager ethnicitiesManager,
            IOrientationsManager orientationsManager)
        {
            this.url = url;
            this.countriesManager = countriesManager;
            this.ethnicitiesManager = ethnicitiesManager;
            this.orientationsManager = orientationsManager;
        }

        public Person GetUserInfo(string username)
        {
            var dom = CQ.CreateFromUrl($"{this.url}/{username}");
            var bioContainer = dom.Select("div.miniBio ul li");

            if (bioContainer.Length == 0)
            {
                if (dom.Select("#main-content").Length > 0)
                {
                    Ethnicity ethnicityDTO = GetEthnisityType(null);
                    Orientation orientationDTO = GetOrientationType(null);
                    Country countryDTO = GetCountry(null);

                    return new Person()
                    {
                        Username = username,
                        IsAccountDisabled = true,
                        Ethnicity = ethnicityDTO.Name,
                        EthnicityId = ethnicityDTO.Id,
                        Orientation = orientationDTO.Name,
                        OrientationId = orientationDTO.Id,
                        Country = countryDTO.Name,
                        CountryId = countryDTO.Id,
                    };
                }
            }

            try
            {
                var attributes = bioContainer.Find("span.desc");

                string createdDate = GetAttributeValue(attributes, "Member since:");
                string lastBroadcastDate = GetAttributeValue(attributes, "Last Broadcast:");
                string ethnicity = GetAttributeValue(attributes, "Ethnicity:");
                string orientation = GetAttributeValue(attributes, "Orientation:");
                string age = GetAttributeValue(attributes, "Age:");
                string country = GetAttributeValue(attributes, "Location:");

                Ethnicity ethnicityDTO = GetEthnisityType(ethnicity);
                Orientation orientationDTO = GetOrientationType(orientation);
                Country countryDTO = GetCountry(country);

                var person = new Person()
                {
                    Username = username,
                    CreateDate = ParseDate(createdDate),
                    LastBroadcastDate = ParseDate(lastBroadcastDate),
                    Age = int.Parse(age),
                    Ethnicity = ethnicityDTO.Name,
                    EthnicityId = ethnicityDTO.Id,
                    Orientation = orientationDTO.Name,
                    OrientationId = orientationDTO.Id,
                    Country = countryDTO.Name,
                    CountryId = countryDTO.Id,
                    IsAccountDisabled = false,                     
                };

                return person;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Orientation GetOrientationType(string orientation)
        {
            if (string.IsNullOrWhiteSpace(orientation))
            {
                orientation = "Unknown";
            }

            Orientation orientationDTO = this.orientationsManager.GetByName(orientation);
            return orientationDTO;
        }

        private Ethnicity GetEthnisityType(string ethnicity)
        {
            if (string.IsNullOrWhiteSpace(ethnicity))
            {
                ethnicity = "Unknown";
            }

            ethnicity = ethnicity.Replace(" ", string.Empty);
            int index = ethnicity.IndexOf("/");
            if (index >= 0)
            {
                ethnicity = ethnicity.Substring(0, index).Trim();
            }

            Ethnicity ethnisityDTO = this.ethnicitiesManager.GetByName(ethnicity);
            return ethnisityDTO;
        }

        private Country GetCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                country = "Unknown";
            }

            int index = country.LastIndexOf(",");
            if (index >= 0)
            {
                country = country.Substring(index + 1);
            }

            Country countryDTO = this.countriesManager.GetByName(country.Trim());
            return countryDTO;
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
