using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Site.Core;
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
        private readonly ILogger logger;

        public SiteHelper(IOptions<SiteCoreOptions> options, ICountriesWriter countriesWriter,
            IPersonAdditionalInfoManager personAdditionalManager, ILoggerFactory loggerFactory,
            IPersonsRepository personsRepository, IPersonsManager personsManager,
            IPersonsWriter personsWriter, ICsQueryWrapper csQueryWrapper)
        {
            this.url = options.Value.SiteUrl;
            this.personAdditionalManager = personAdditionalManager;
            this.countriesWriter = countriesWriter;
            this.logger = loggerFactory.CreateLogger(this.GetType().Name);
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

        public async Task<Person> GetUserInfoAsync(string username)
        {
            try
            {
                this.logger.LogDebug($"Helper: Get person by name: {username}");

                Person person = this.csQueryWrapper.GetInfo(this.url, username);

                Ethnicity ethnicityDTO = GetEthnisityType(person.Ethnicity);
                Orientation orientationDTO = GetOrientationType(person.Orientation);
                Country countryDTO = await GetCountry(person.Country).ConfigureAwait(false);

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
                this.logger.LogError(ex, $"Error in Site helper, username: {username}, url: {this.url}");
                throw;
            }
        }

        public async Task<int> UpdatePersonsAsync()
        {
            var persons = await this.personsRepository.GetAllForUpdateAsync().ConfigureAwait(false);

            foreach (var person in persons)
            {
                var updatedPerson = await this.GetUserInfoAsync(person.Username).ConfigureAwait(false);

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

               await this.personsWriter.AddOrUpdateAsync(person).ConfigureAwait(false);
            }

            return persons.Count();
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

        public async Task<PersonExtended> GetPersonByNameAsync(string userName)
        {
            Person person = this.personsManager.GetByName(userName);
            if (person == null)
            {
                person = await this.GetUserInfoAsync(userName).ConfigureAwait(false);
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

        private async Task<Country> GetCountry(string country)
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
                countryDTO = await this.countriesWriter.AddAsync(country);
            }

            return countryDTO;
        }

        #endregion
    }
}
