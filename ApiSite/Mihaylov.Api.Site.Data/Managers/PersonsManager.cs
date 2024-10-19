using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;
using Mihaylov.Api.Site.Contracts.Repositories;

namespace Mihaylov.Api.Site.Data.Managers
{
    public class PersonsManager : IPersonsManager
    {
        private readonly IPersonsRepository _repository;
        private readonly ILogger _logger;

        public PersonsManager(IPersonsRepository personsRepository, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(this.GetType().Name);
            _repository = personsRepository;
        }

        public Task<Grid<Person>> GetAllPersonsAsync(GridRequest request)
        {
            return _repository.GetAllPersonsAsync(request);
        }

        public Task<Person> GetPersonAsync(long id)
        {
            return _repository.GetPersonAsync(id);
        }

        public Task<Account> GetAccountAsync(long id)
        {
            return _repository.GetAccountAsync(id);
        }

        public Task<UpdateAccounts> GetAllAccountsForUpdateAsync(int? batchSize)
        {
            return _repository.GetAllAccountsForUpdateAsync(batchSize);
        }

        public Task<PersonStatistics> GetStaticticsAsync()
        {
            return _repository.GetStaticticsAsync();
        }

        public async Task<PersonFormatedStatistics> GetFormatedStatisticsAsync()
        {
            var statistics = await GetStaticticsAsync().ConfigureAwait(false);

            var result = new List<PersonStatisticsPair>();

            var answerCount = statistics.Answers.Sum(s => s.Value);
            var accountCount = statistics.AccountTypes.Sum(a => a.Value);
            var stateCount = statistics.States.Sum(a => a.Value);

            result.Add(new PersonStatisticsPair("Acounts", $"{stateCount} ({accountCount})"));
            result.Add(new PersonStatisticsPair("People", $"{statistics.TotalPersonCount}"));
            result.Add(new PersonStatisticsPair(string.Empty, string.Empty));

            result.Add(new PersonStatisticsPair("Average", $"{statistics.Average:0.00} ({statistics.Min:0.0} ÷ {statistics.Max:0.0})"));

            foreach (var answer in statistics.Answers)
            {
                result.Add(new PersonStatisticsPair(answer.Key ? "Answered" : "I Don't Know", $"{answer.Value} ({(decimal)answer.Value / answerCount:P2})"));
            }

            result.Add(new PersonStatisticsPair("Answers", $"{answerCount}"));

            result.Add(new PersonStatisticsPair(string.Empty, string.Empty));

            foreach (var types in statistics.AccountTypes)
            {
                result.Add(new PersonStatisticsPair(types.Key, $"{types.Value} ({(decimal)types.Value / accountCount:P2})"));
            }

            result.Add(new PersonStatisticsPair(string.Empty, string.Empty));

            foreach (var status in statistics.States)
            {
                result.Add(new PersonStatisticsPair(status.Key, $"{status.Value} ({(decimal)status.Value / stateCount:P2})"));
            }

            return new PersonFormatedStatistics()
            {
                Data = result,
            };
        }
    }
}
