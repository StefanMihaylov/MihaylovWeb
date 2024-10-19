using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Hubs;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Data.Models;
using Mihaylov.Common.Generic.Extensions;

namespace Mihaylov.Api.Site.Data.Helpers
{
    public class SiteHelper : ISiteHelper
    {
        private readonly ILogger _logger;

        private readonly ICollectionManager _collectionManager;
        private readonly IPersonsManager _personsManager;
        private readonly IPersonsWriter _personsWriter;

        private readonly HttpClient _client;
        private readonly string _url;
        private readonly DateTime _baseDate;

        public SiteHelper(IOptions<SiteOptions> options,
            ICollectionManager collectionManager, ILoggerFactory loggerFactory,
            IPersonsManager personsManager, IPersonsWriter personsWriter, IHttpClientFactory factory)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);

            _url = options.Value.SiteUrl;
            _baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            _client = factory.CreateClient("SiteHelper");

            _collectionManager = collectionManager;
            _personsManager = personsManager;
            _personsWriter = personsWriter;
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

        public async Task FillNewPersonAsync(Person person, string username)
        {
            var account = person.Accounts.First(a => a.Username == username);

            var personInfo = await GetPersonInfoAsync(username, (a) => { }).ConfigureAwait(false);
            if (personInfo.IsDeleted)
            {
                account.StatusId = 3;
                return;
            }

            account.CreateDate = personInfo.CreateDate;
            account.LastOnlineDate = personInfo.LastOnlineDate;

            if (personInfo.Age.HasValue)
            {
                person.DateOfBirthType = DateOfBirthType.YearCalculated;
                person.DateOfBirth = person.DateOfBirth.GetBirthDate(personInfo.Age, true, true);
            }

            if (!string.IsNullOrEmpty(personInfo.City))
            {
                person.Location ??= new PersonLocation();
                person.Location.City = personInfo.City;
            }

            if (personInfo.CountryId.HasValue)
            {
                person.CountryId = personInfo.CountryId;
            }

            if (personInfo.EthnicityId.HasValue)
            {
                person.EthnicityId = personInfo.EthnicityId;
            }

            if (personInfo.OrientationId.HasValue)
            {
                person.OrientationId = personInfo.OrientationId;
            }

            if (!string.IsNullOrEmpty(personInfo.Comments))
            {
                person.Comments += personInfo.Comments;
            }
        }

        public async Task UpdateAccountsAsync(int? batchSize, int delay, Action<UpdateProgressBarModel> progress)
        {
            var accounts = await _personsManager.GetAllAccountsForUpdateAsync(batchSize).ConfigureAwait(false);

            const int SUB_STEPS = 3;
            const int TOTAL_STEPS = SUB_STEPS + 2;
            var totalCount = accounts.Accounts.Count();

            int count = 0;
            foreach (var account in accounts.Accounts)
            {
                count++;
                var countPers = count.GetPersentage(totalCount);

                var personInfo = await GetPersonInfoAsync(account.Username, (p) => progress(new UpdateProgressBarModel(p.GetPersentage(TOTAL_STEPS), countPers, totalCount))).ConfigureAwait(false);

                if (personInfo.IsDeleted)
                {
                    account.StatusId = 3;
                }
                else
                {
                    account.LastOnlineDate = personInfo.LastOnlineDate;
                    if (account.LastOnlineDate.HasValue)
                    {
                        var inactiveYears = account.LastOnlineDate.Value.GetAge();
                        if (inactiveYears >= 5)
                        {
                            account.StatusId = 7;
                        }
                    }
                }

                var single = SUB_STEPS;

                await _personsWriter.AddOrUpdateAccountAsync(account, null).ConfigureAwait(false);
                progress(new UpdateProgressBarModel((++single).GetPersentage(TOTAL_STEPS), countPers, totalCount));

                if (delay > 0)
                {
                    Thread.Sleep(delay);
                }

                progress(new UpdateProgressBarModel((++single).GetPersentage(TOTAL_STEPS), countPers, totalCount));
            }
        }

        private async Task<PersonInfo> GetPersonInfoAsync(string username, Action<int> progress)
        {
            int stepNumber = 0;
            progress(stepNumber);

            _logger.LogDebug($"Helper: Get person by name: {username}");

            var result = new PersonInfo()
            {
                Username = username,
                IsDeleted = false,
                Comments = string.Empty,
            };

            var isDeleted = await TestUserAsync(username);
            if (isDeleted)
            {
                result.IsDeleted = true;
                // 
                return result;
            }

            progress(++stepNumber);

            var url = $"{_url}/rest/v1.0/profile/{username}/info";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var responseMessage = await _client.SendAsync(request).ConfigureAwait(false);
            var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            progress(++stepNumber);

            if (!responseMessage.IsSuccessStatusCode)
            {
                _logger.LogError($"Invalid response for username: {username}");
                //
                return result;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var personSiteInfo = JsonSerializer.Deserialize<PersonInfoModel>(responseString, options);

            result.Age = personSiteInfo.Age;
            result.City = personSiteInfo.City;
            result.CreateDate = _baseDate.AddMilliseconds(personSiteInfo.CreationDate).Date;
            result.LastOnlineDate = _baseDate.AddMilliseconds(personSiteInfo.LastBroadcast);

            if (!string.IsNullOrEmpty(personSiteInfo.CountryId))
            {
                var countries = await _collectionManager.GetAllCountriesAsync().ConfigureAwait(false);
                var country = countries.Where(c => c.TwoLetterCode != null)
                                       .Where(c => c.TwoLetterCode.Equals(personSiteInfo.CountryId, StringComparison.OrdinalIgnoreCase))
                                       .FirstOrDefault();

                if (country != null)
                {
                    result.CountryId = country.Id;
                }
                else
                {
                    result.Comments += $" Country: {personSiteInfo.CountryId},";
                }
            }

            if (!string.IsNullOrEmpty(personSiteInfo.Ethnicity))
            {
                var ethnicities = await _collectionManager.GetAllEthnicitiesAsync().ConfigureAwait(false);
                var ethnicity = ethnicities.Where(c => c.Name.Equals(personSiteInfo.Ethnicity, StringComparison.OrdinalIgnoreCase) ||
                                                       c.OtherNames?.Contains(personSiteInfo.Ethnicity, StringComparison.OrdinalIgnoreCase) == true)
                                           .FirstOrDefault();

                if (ethnicity != null)
                {
                    result.EthnicityId = ethnicity.Id;
                }
                else
                {
                    result.Comments += $" Ethnicity: {personSiteInfo.Ethnicity},";
                }
            }

            if (!string.IsNullOrEmpty(personSiteInfo.SexPreference) &&
                !personSiteInfo.SexPreference.Equals("Unknown", StringComparison.OrdinalIgnoreCase))
            {
                var orientations = await _collectionManager.GetAllOrientationsAsync().ConfigureAwait(false);
                var orientation = orientations.Where(c => c.Name.Equals(personSiteInfo.SexPreference, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (orientation != null)
                {
                    result.OrientationId = orientation.Id;
                }
                else
                {
                    result.Comments += $" Orientation: {personSiteInfo.SexPreference},";
                }
            }

            progress(++stepNumber);

            return result;
        }

        private async Task<bool> TestUserAsync(string username)
        {
            var url = $"{_url}/{username}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var responseMessage = await _client.SendAsync(request).ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                return false;
            }
            else if (responseMessage.StatusCode == HttpStatusCode.Redirect &&
                    responseMessage.Headers.Location.ToString() == "/error-no-profile")
            {
                return true;
            }

            return false;
        }
    }
}
