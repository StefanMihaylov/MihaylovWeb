using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Data.Models;

namespace Mihaylov.Api.Site.Data.Helpers
{
    public class SiteHelper : ISiteHelper
    {
        private readonly ILogger _logger;

        private readonly ICollectionManager _collectionManager;
        private readonly IPersonsManager _personsManager;

        private readonly HttpClient _client;
        private readonly string _url;
        private readonly DateTime _baseDate;

        public SiteHelper(IOptions<SiteOptions> options,
            ICollectionManager collectionManager, ILoggerFactory loggerFactory,
            IPersonsManager personsManager, ICsQueryWrapper csQueryWrapper, IHttpClientFactory factory)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);

            _url = options.Value.SiteUrl;
            _baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            _client = factory.CreateClient("SiteHelper");

            _collectionManager = collectionManager;
            _personsManager = personsManager;
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

                Person person = null; // _csQueryWrapper.GetInfo(_url, username);

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
                _logger.LogError(ex, $"Error in Site helper, username: {username}, url: {this._url}");
                throw;
            }
        }

        public async Task FillNewPersonAsync(Person person, string username)
        {
            var account = person.Accounts.First();

            var url = $"{_url}/rest/v1.0/profile/{username}/info";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var responseMessage = await _client.SendAsync(request).ConfigureAwait(false);
            var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                account.StatusId = 3;
            }
            else if (responseMessage.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var personInfo = JsonSerializer.Deserialize<PersonInfoModel>(responseString, options);

                person.DateOfBirth = GetBirthDate(personInfo.Age);

                if (!string.IsNullOrEmpty(personInfo.City))
                {
                    person.Location ??= new PersonLocation();
                    person.Location.City = personInfo.City;
                }

                if (!string.IsNullOrEmpty(personInfo.CountryId))
                {
                    var countries = await _collectionManager.GetAllCountriesAsync().ConfigureAwait(false);
                    var country = countries.Where(c => c.TwoLetterCode.Equals(personInfo.CountryId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    if (country != null)
                    {
                        person.CountryId = country.Id;
                    }
                    else
                    {
                        person.Comments += $" Country: {personInfo.CountryId},";
                    }
                }

                if (!string.IsNullOrEmpty(personInfo.Ethnicity))
                {
                    var ethnicities = await _collectionManager.GetAllEthnicitiesAsync().ConfigureAwait(false);
                    var ethnicity = ethnicities.Where(c => c.Name.Equals(personInfo.Ethnicity, StringComparison.OrdinalIgnoreCase) ||
                                                           c.OtherNames?.Contains(personInfo.Ethnicity, StringComparison.OrdinalIgnoreCase) == true)
                                               .FirstOrDefault();

                    if (ethnicity != null)
                    {
                        person.EthnicityId = ethnicity.Id;
                    }
                    else
                    {
                        person.Comments += $" Ethnicity: {personInfo.Ethnicity},";
                    }
                }

                if (!string.IsNullOrEmpty(personInfo.SexPreference))
                {
                    var orientations = await _collectionManager.GetAllOrientationsAsync().ConfigureAwait(false);
                    var orientation = orientations.Where(c => c.Name.Equals(personInfo.SexPreference, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    if (orientation != null)
                    {
                        person.OrientationId = orientation.Id;
                    }
                    else
                    {
                        person.Comments += $" Orientation: {personInfo.SexPreference},";
                    }
                }

                account.CreateDate = _baseDate.AddMilliseconds(personInfo.CreationDate).Date;
                account.LastOnlineDate = _baseDate.AddMilliseconds(personInfo.LastBroadcast);
            }
        }

        private DateTime GetBirthDate(int age)
        {
            return DateTime.UtcNow.Date.AddYears(-age).AddMonths(-6);
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

        //public async Task<PersonExtended> GetPersonByNameAsync(string userName)
        //{
        //    Person person = this._personsManager.GetByName(userName);
        //    if (person == null)
        //    {
        //        person = await this.GetUserInfoAsync(userName).ConfigureAwait(false);
        //    }

        //    // var units = await personAdditionalManager.GetAllUnitsAsync().ConfigureAwait(false);    
        //    var states = await _collectionManager.GetAllAccountStatesAsync().ConfigureAwait(false);

        //    var personExtended = new PersonExtended(person)
        //    {
        //        AnswerUnits = null,
        //        AnswerTypes = states,
        //    };

        //    return personExtended;
        //}

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
