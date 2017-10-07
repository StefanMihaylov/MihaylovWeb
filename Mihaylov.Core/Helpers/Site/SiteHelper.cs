using System;
using System.Linq;

using CsQuery;
using Mihaylov.Data.Models.Site;
using Mihaylov.Core.Interfaces.Site;
using Ninject.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;

namespace Mihaylov.Core.Helpers.Site
{
    public class SiteHelper : ISiteHelper
    {
        private readonly string url;
        private readonly IPersonAdditionalInfoManager personAdditionalManager;
        private readonly ICountriesWriter countriesWriter;
        private readonly IPersonsProvider personsProvider;
        private readonly IPersonsWriter personsWriter;
        private readonly ILogger logger;

        public SiteHelper(string url, ICountriesWriter countriesWriter,
            IPersonAdditionalInfoManager personAdditionalManager, ILogger logger,
            IPersonsProvider personsProvider, IPersonsWriter personsWriter)
        {
            this.url = url;
            this.personAdditionalManager = personAdditionalManager;
            this.countriesWriter = countriesWriter;
            this.logger = logger;
            this.personsProvider = personsProvider;
            this.personsWriter = personsWriter;
        }

        public string SystemUnit
        {
            get
            {
                return this.personAdditionalManager.GetAllUnits()
                                                   .FirstOrDefault(u => u.ConversionRate == 1m)?.Name;
            }
        }

        public string GetUserName(string url)
        {
            if (url == null)
            {
                return null;
            }

            int index = url.LastIndexOf('/');
            if (index >= 0)
            {
                string username = url.Substring(index + 1);
                return username.Trim();
            }
            else
            {
                return url.Trim();
            }
        }

        public void AddAdditionalInfo(Person person)
        {
            if (person.AskDate == null)
            {
                person.AskDate = DateTime.Now;
            }

            if (string.IsNullOrWhiteSpace(person.AnswerType))
            {
                AnswerType answerType = this.personAdditionalManager.GetAnswerTypeById(person.AnswerTypeId);
                person.AnswerType = answerType.Name;
            }

            if (person.Answer.HasValue)
            {
                Unit unit = this.personAdditionalManager.GetUnitById(person.AnswerUnitId.Value);

                person.AnswerUnit = unit.Name;
                person.AnswerConverted = person.Answer.Value * unit.ConversionRate;
                person.AnswerConvertedUnit = this.SystemUnit;
            }
            else
            {
                person.AnswerUnit = null;
                person.AnswerUnitId = null;
                person.AnswerConverted = null;
            }
        }

        public Person GetUserInfo(string username)
        {
            try
            {
                this.logger.Debug($"Helper: Get person by name: {username}");

                var dom = CQ.CreateFromUrl($"{this.url}/{username}");
                var bioContainer = dom.Select("div.miniBio ul li");

                if (bioContainer.Length == 0)
                {
                    if (dom.Select("#main-content").Length > 0)
                    {
                        Ethnicity ethnicityDtoNull = GetEthnisityType(null);
                        Orientation orientationDtoNull = GetOrientationType(null);
                        Country countryDtoNull = GetCountry(null);

                        return new Person()
                        {
                            Username = username,
                            IsAccountDisabled = true,
                            Ethnicity = ethnicityDtoNull.Name,
                            EthnicityId = ethnicityDtoNull.Id,
                            Orientation = orientationDtoNull.Name,
                            OrientationId = orientationDtoNull.Id,
                            Country = countryDtoNull.Name,
                            CountryId = countryDtoNull.Id,
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
                this.logger.ErrorException($"Error in Site helper, username: {username}, url: {this.url}", ex);
                throw;
            }
        }

        public int UpdatePersons()
        {
            var persons = this.personsProvider.GetAll()
                .Where(p => p.IsAccountDisabled == false)
                .Where(p => p.UpdatedDate < DateTime.Now.AddDays(-1))
                .ToList();

            foreach (var person in persons)
            {
                if (!person.IsAccountDisabled)
                {
                    var updatedPerson = this.GetUserInfo(person.Username);
                    this.personsWriter.Update(updatedPerson);
                }
            }

            return persons.Count;
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            return this.personAdditionalManager.GetAllUnits();
        }

        public IEnumerable<AnswerType> GetAllAnswerTypes()
        {
            return this.personAdditionalManager.GetAllAnswerTypes();
        }

        #region Private Methods

        private Orientation GetOrientationType(string orientation)
        {
            if (string.IsNullOrWhiteSpace(orientation))
            {
                orientation = "Unknown";
            }

            Orientation orientationDTO = this.personAdditionalManager.GetOrientationByName(orientation);
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

            Ethnicity ethnisityDTO = this.personAdditionalManager.GetEthnicityByName(ethnicity);
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

            Country countryDTO = this.personAdditionalManager.GetCountryByName(country);
            if (countryDTO == null)
            {
                countryDTO = this.countriesWriter.Add(country);
            }

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

        #endregion
    }
}
