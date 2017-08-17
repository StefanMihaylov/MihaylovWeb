using System;
using System.Linq;

using CsQuery;
using Mihaylov.Data.Models.Repositories;
using Mihaylov.Core.Interfaces;
using Ninject.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;

namespace Mihaylov.Core.Helpers
{
    public class SiteHelper : ISiteHelper
    {
        private readonly string url;
        private readonly ICountriesManager countriesManager;
        private readonly IEthnicitiesManager ethnicitiesManager;
        private readonly IOrientationsManager orientationsManager;
        private readonly IUnitsManager unitsManager;
        private readonly IAnswerTypesManager answerTypesManager;
        private readonly ICountriesWriter countriesWriter;
        private readonly IPersonsProvider personsProvider;
        private readonly IPersonsWriter personsWriter;
        private readonly ILogger logger;

        private readonly string systemUnit;

        public SiteHelper(string url, ICountriesManager countriesManager, ICountriesWriter countriesWriter,
            IEthnicitiesManager ethnicitiesManager, IOrientationsManager orientationsManager,
            IUnitsManager unitsManager, IAnswerTypesManager answerTypesManager, ILogger logger,
            IPersonsProvider personsProvider, IPersonsWriter personsWriter)
        {
            this.url = url;
            this.countriesManager = countriesManager;
            this.ethnicitiesManager = ethnicitiesManager;
            this.orientationsManager = orientationsManager;
            this.unitsManager = unitsManager;
            this.answerTypesManager = answerTypesManager;
            this.countriesWriter = countriesWriter;
            this.logger = logger;
            this.personsProvider = personsProvider;
            this.personsWriter = personsWriter;

            systemUnit = this.unitsManager.GetAllUnits().FirstOrDefault(u => u.ConversionRate == 1m)?.Name;
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
                AnswerType answerType = this.answerTypesManager.GetById(person.AnswerTypeId);
                person.AnswerType = answerType.Name;
            }

            if (person.Answer.HasValue)
            {
                Unit unit = this.unitsManager.GetById(person.AnswerUnitId.Value);

                person.AnswerUnit = unit.Name;
                person.AnswerConverted = person.Answer.Value * unit.ConversionRate;
                person.AnswerConvertedUnit = systemUnit;
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

        public void UpdatePersons()
        {
            var persons = this.personsProvider.GetAll();

            foreach (var person in persons)
            {
                if (!person.IsAccountDisabled)
                {
                    var updatedPerson = this.GetUserInfo(person.Username);
                    this.personsWriter.Update(updatedPerson);
                }
            }
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            return this.unitsManager.GetAllUnits();
        }

        public IEnumerable<AnswerType> GetAllAnswerTypes()
        {
            return this.answerTypesManager.GetAllAnswerTypes();
        }

        #region Private Methods

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

            Country countryDTO = this.countriesManager.GetByName(country);
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
