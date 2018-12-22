using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Mihaylov.Site.Core.CsQuery;
using Mihaylov.Site.Core.Interfaces;
using Mihaylov.Site.Core.Models;
using Mihaylov.Site.Data.Interfaces;
using Mihaylov.Site.Data.Models;

namespace Mihaylov.Core.Helpers.Site
{
    public class SiteHelper : ISiteHelper
    {
        private readonly string url;

        private readonly IPersonAdditionalInfoManager personAdditionalManager;
        private readonly ICountriesWriter countriesWriter;
        private readonly IPersonsRepository personsRepository;
        private readonly IPersonsManager personsManager;
        private readonly IPersonsWriter personsWriter;
        private readonly ICsQueryWrapper csQueryWrapper;
        private readonly ILog logger;

        public SiteHelper(string url, ICountriesWriter countriesWriter,
            IPersonAdditionalInfoManager personAdditionalManager, ILog logger,
            IPersonsRepository personsRepository, IPersonsManager personsManager,
            IPersonsWriter personsWriter, ICsQueryWrapper csQueryWrapper)
        {
            this.url = url;
            this.personAdditionalManager = personAdditionalManager;
            this.countriesWriter = countriesWriter;
            this.logger = logger;
            this.personsRepository = personsRepository;
            this.personsManager = personsManager;
            this.personsWriter = personsWriter;
            this.csQueryWrapper = csQueryWrapper;
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
                person.AskDate = DateTime.UtcNow;
            }

            if (person.Answer.HasValue)
            {
                Unit unit = this.personAdditionalManager.GetUnitById(person.AnswerUnitTypeId.Value);

                person.AnswerUnit = unit.Name;
                person.AnswerConverted = person.Answer.Value * unit.ConversionRate;
                person.AnswerConvertedUnit = this.GetSystemUnit();
            }
            else
            {
                person.AnswerUnit = null;
                person.AnswerUnitTypeId = null;
                person.AnswerConverted = null;
            }
        }

        public Person GetUserInfo(string username)
        {
            try
            {
                this.logger.Debug($"Helper: Get person by name: {username}");

                Person person = this.csQueryWrapper.GetInfo(this.url, username);

                Ethnicity ethnicityDTO = GetEthnisityType(person.Ethnicity);
                Orientation orientationDTO = GetOrientationType(person.Orientation);
                Country countryDTO = GetCountry(person.Country);

                person.Ethnicity = ethnicityDTO.Name;
                person.EthnicityTypeId = ethnicityDTO.Id;
                person.Orientation = orientationDTO.Name;
                person.OrientationTypeId = orientationDTO.Id;
                person.Country = countryDTO.Name;
                person.CountryId = countryDTO.Id;

                return person;
            }
            catch (Exception ex)
            {
                this.logger.Error($"Error in Site helper, username: {username}, url: {this.url}", ex);
                throw;
            }
        }

        public int UpdatePersons()
        {
            var persons = this.personsRepository.GetAll()
                .Where(p => p.IsAccountDisabled == false)
                .Where(p => p.UpdatedDate < DateTime.UtcNow.AddDays(-1))
                .ToList();

            foreach (var person in persons)
            {
                var updatedPerson = this.GetUserInfo(person.Username);

                if (!updatedPerson.IsAccountDisabled)
                {
                    if (updatedPerson.LastBroadcastDate > DateTime.MinValue)
                    {
                        person.LastBroadcastDate = updatedPerson.LastBroadcastDate;
                    }

                    if (updatedPerson.Age > 0)
                    {
                        person.Age = updatedPerson.Age;
                    }

                    if (updatedPerson.CountryId > 0)
                    {
                        person.CountryId = updatedPerson.CountryId;
                    }
                }

                person.IsAccountDisabled = updatedPerson.IsAccountDisabled;

                this.personsWriter.AddOrUpdate(person);
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

        public string GetSystemUnit()
        {
            return this.personAdditionalManager.GetAllUnits()
                                               .FirstOrDefault(u => u.ConversionRate == 1m)?.Name;
        }

        public PersonExtended GetPersonByName(string userName)
        {
            Person person = this.personsManager.GetByName(userName);
            if (person == null)
            {
                person = this.GetUserInfo(userName);
            }

            var personExtended = new PersonExtended(person)
            {
                AnswerUnits = this.GetAllUnits(),
                AnswerTypes = this.GetAllAnswerTypes()
            };

            return personExtended;
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

        #endregion
    }
}
