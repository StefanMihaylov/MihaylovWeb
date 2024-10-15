using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Mihaylov.Common.Host.Abstract.AssemblyVersion;

namespace Mihaylov.Api.Site.Client
{
    public partial interface ISiteApiClient
    {
        public void AddToken(string token);

        Task<PersonGrid> PersonsAsync(IGridRequest request, int? pageSize);
    }

    public partial class SiteApiClient : ISiteApiClient
    {
        public const string SITE_API_CLIENT_NAME = "SiteApiClient";

        public SiteApiClient(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory.CreateClient(SITE_API_CLIENT_NAME))
        {
            _baseUrl = _httpClient.BaseAddress.AbsoluteUri;
        }

        public void AddToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public Task<PersonGrid> PersonsAsync(IGridRequest request, int? pageSize)
        {
            return PersonsAsync(request.AccountTypeId, request.StatusId, request.Name,
                request.AccountName, request.AccountNameExact, request.Page, pageSize);
        }
    }

    public partial class ModuleInfo : IModuleInfo
    {
    }

    public partial class GridRequest : IGridRequest
    {
        public long? PersonId => null;

        public bool? IsNewPerson => null;
    }


    public partial class PersonDetail
    {
        public string FullName
        {
            get
            {
                var otherName = string.Empty;
                if (!string.IsNullOrEmpty(OtherNames))
                {
                    otherName = $"({this.OtherNames})";
                }

                return $"{FirstName} {LastName} {otherName}";
            }
        }
    }

    public partial class Person
    {
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue || !DateOfBirthType.HasValue)
                {
                    return null;
                }

                var age = DateOfBirth.Value.GetAge();

                return age;
            }
        }

        public string AgeGrid
        {
            get
            {
                if (!DateOfBirth.HasValue || !DateOfBirthType.HasValue)
                {
                    return string.Empty;
                }

                var birthDate = DateOfBirth.Value;
                var age = Age;

                return DateOfBirthType switch
                {
                    Client.DateOfBirthType.Full => $"{age} ({birthDate.ToString("dd.MM.yyyy")})",
                    Client.DateOfBirthType.YearAndMonth => $"{age} (**.{birthDate.ToString("MM.yyyy")})",
                    Client.DateOfBirthType.YearOnly => $"{age} (**.**.{birthDate.ToString("yyyy")})",
                    Client.DateOfBirthType.YearCalculated => $"{age}",
                    _ => string.Empty,
                };
            }
        }
    }

    public partial class Account
    {
        public int? AccountAge
        {
            get
            {
                if (!CreateDate.HasValue)
                {
                    return null;
                }

                return (int)DateTime.UtcNow.Subtract(CreateDate.Value).TotalDays;
            }
            set
            {
                if (value.HasValue)
                {
                    CreateDate = DateTime.UtcNow.AddDays(-value.Value);
                }
            }
        }
    }

    public partial class QuizAnswer
    {
        public string ValueDisplay
        {
            get
            {
                if (!Value.HasValue && !ConvertedValue.HasValue)
                {
                    return string.Empty;
                }

                if (!ConvertedValue.HasValue)
                {
                    return $"{Value.Value:0.##}";
                }
                else if (Value.Value == ConvertedValue.Value)
                {
                    return $"{Value.Value:0.##}";
                }
                else
                {
                    return $"{Value.Value} ({ConvertedValue.Value:0.##} {ConvertedUnit})";
                }
            }
        }
    }

    public partial class PersonStatistics
    {
        public IEnumerable<CountPairs> CountDictionary
        {
            get
            {
                var result = new List<CountPairs>();

                var answerCount = Answers.Sum(s => s.Value);
                var accountCount = AccountTypes.Sum(a => a.Value);
                var stateCount = States.Sum(a => a.Value);

                result.Add(new CountPairs("Acounts", $"{stateCount} ({accountCount})"));
                result.Add(new CountPairs("People", $"{TotalPersonCount}"));
                result.Add(new CountPairs(string.Empty, string.Empty));

                result.Add(new CountPairs("Average", $"{Average:0.00} ({Min:0.0} ÷ {Max:0.0})"));

                foreach (var answer in Answers)
                {
                    result.Add(new CountPairs(answer.Key ? "Answered" : "I Don't Know", $"{answer.Value} ({(decimal)answer.Value / answerCount:P2})"));
                }

                result.Add(new CountPairs("Answers", $"{answerCount}"));

                result.Add(new CountPairs(string.Empty, string.Empty));

                foreach (var types in AccountTypes)
                {
                    result.Add(new CountPairs(types.Key, $"{types.Value} ({(decimal)types.Value / accountCount:P2})"));
                }

                result.Add(new CountPairs(string.Empty, string.Empty));

                foreach (var status in States)
                {
                    result.Add(new CountPairs(status.Key, $"{status.Value} ({(decimal)status.Value / stateCount:P2})"));
                }

                return result;
            }
        }
    }

    public class CountPairs
    {
        public string Key { get; private set; }

        public string Value { get; private set; }

        public CountPairs(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    public interface IGridRequest
    {
        public int? AccountTypeId { get; }

        public int? StatusId { get; }

        public string Name { get; }

        public string AccountName { get; }

        public string AccountNameExact { get; }

        public int? Page { get; set; }

        public long? PersonId { get; }

        public bool? IsNewPerson { get; }
    }
}
