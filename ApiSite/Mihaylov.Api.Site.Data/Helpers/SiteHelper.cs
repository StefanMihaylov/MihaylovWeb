using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Data.Models;

namespace Mihaylov.Api.Site.Data.Helpers
{
    public class SiteHelper : ISiteHelper
    {
        private readonly string url;

        private readonly ILogger _logger;
        private readonly ICollectionManager _collectionManager;
        private readonly IPersonsManager _personsManager;
        private readonly IPersonsWriter personsWriter;
        private readonly ICsQueryWrapper csQueryWrapper;


        public SiteHelper(IOptions<SiteOptions> options, 
            ICollectionManager collectionManager, ILoggerFactory loggerFactory,
            IPersonsManager personsManager,
            IPersonsWriter personsWriter, ICsQueryWrapper csQueryWrapper)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            url = options.Value.SiteUrl;
            _collectionManager = collectionManager;            
            _personsManager = personsManager;
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
            //if (person.AskDate == null)
            //{
            //    person.AskDate = DateTime.UtcNow;
            //}

            //if (person.Answer.HasValue)
            //{
            //    Unit unit = this.personAdditionalManager.GetUnitById(person.AnswerUnitTypeId.Value);

            //    person.AnswerUnit = unit.Name;
            //    person.AnswerConverted = person.Answer.Value * unit.ConversionRate;
            //    person.AnswerConvertedUnit = this.GetSystemUnit();
            //}
            //else
            //{
            //    person.AnswerUnit = null;
            //    person.AnswerUnitTypeId = null;
            //    person.AnswerConverted = null;
            //}
        }

        public async Task<Person> GetUserInfoAsync(string username)
        {
            try
            {
                _logger.LogDebug($"Helper: Get person by name: {username}");

                Person person = this.csQueryWrapper.GetInfo(this.url, username);

                Ethnicity ethnicityDTO = await GetEthnisityTypeAsync(person.Ethnicity).ConfigureAwait(false);
                Orientation orientationDTO = await GetOrientationType(person.Orientation).ConfigureAwait(false);
                Country countryDTO = await GetCountry(person.Country).ConfigureAwait(false);

                person.Ethnicity = ethnicityDTO.Name;
                person.EthnicityId = ethnicityDTO.Id;
                person.Orientation = orientationDTO.Name;
                person.OrientationId = orientationDTO.Id;
                person.Country = countryDTO.Name;
                person.CountryId = countryDTO.Id;

                return person;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Site helper, username: {username}, url: {this.url}");
                throw;
            }
        }

        public async Task<int> UpdatePersonsAsync()
        {
            // var persons = await this.personsRepository.GetAllForUpdateAsync().ConfigureAwait(false);

            //foreach (var person in persons)
            //{
            //    var updatedPerson = await this.GetUserInfoAsync(person.Username).ConfigureAwait(false);

            //    if (!updatedPerson.IsAccountDisabled)
            //    {
            //        //if (updatedPerson.LastBroadcastDate > DateTime.MinValue)
            //        //{
            //        //    person.LastBroadcastDate = updatedPerson.LastBroadcastDate;
            //        //}

            //        if (updatedPerson.Age > 0)
            //        {
            //            person.Age = updatedPerson.Age;
            //        }

            //        if (updatedPerson.CountryId > 0)
            //        {
            //            person.CountryId = updatedPerson.CountryId;
            //        }
            //    }

            //    // person.IsAccountDisabled = updatedPerson.IsAccountDisabled;

            //    await this.personsWriter.AddOrUpdateAsync(person).ConfigureAwait(false);
            //}

            return 0; // persons.Count();
        }

        public async Task<PersonExtended> GetPersonByNameAsync(string userName)
        {
            Person person = this._personsManager.GetByName(userName);
            if (person == null)
            {
                person = await this.GetUserInfoAsync(userName).ConfigureAwait(false);
            }

            // var units = await personAdditionalManager.GetAllUnitsAsync().ConfigureAwait(false);    
            var states = await _collectionManager.GetAllAccountStatesAsync().ConfigureAwait(false);

            var personExtended = new PersonExtended(person)
            {
                AnswerUnits = null,
                AnswerTypes = states,
            };

            return personExtended;
        }

        private async Task<Orientation> GetOrientationType(string orientation)
        {
            if (string.IsNullOrWhiteSpace(orientation))
            {
                return null;
            }

            var orientations = await _collectionManager.GetAllOrientationsAsync().ConfigureAwait(false);
            Orientation orientationDTO = orientations.FirstOrDefault(o => o.Name.Equals(orientation, StringComparison.OrdinalIgnoreCase));
            return orientationDTO;
        }

        private async Task<Ethnicity> GetEthnisityTypeAsync(string ethnicity)
        {
            if (string.IsNullOrWhiteSpace(ethnicity))
            {
                return null;
            }

            ethnicity = ethnicity.Replace(" ", string.Empty);
            int index = ethnicity.IndexOf("/");
            if (index >= 0)
            {
                ethnicity = ethnicity.Substring(0, index).Trim();
            }

            var ethnisities = await _collectionManager.GetAllEthnicitiesAsync().ConfigureAwait(false);
            Ethnicity ethnisityDTO = ethnisities.FirstOrDefault(o => o.Name.Equals(ethnicity, StringComparison.OrdinalIgnoreCase));
            
            return ethnisityDTO;
        }

        private async Task<Country> GetCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                country = null;
            }

            int index = country.LastIndexOf(",");
            if (index >= 0)
            {
                country = country.Substring(index + 1);
            }

            Country countryDTO = await this._collectionManager.GetCountryByNameAsync(country).ConfigureAwait(false);
            if (countryDTO == null)
            {
                countryDTO = null; // add validation or add to database
            }

            return countryDTO;
        }
    }
}
