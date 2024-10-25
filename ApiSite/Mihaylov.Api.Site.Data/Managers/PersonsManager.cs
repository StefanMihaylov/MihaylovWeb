using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;
using Mihaylov.Api.Site.Contracts.Repositories;

namespace Mihaylov.Api.Site.Data.Managers
{
    public class PersonsManager : BaseManager, IPersonsManager
    {
        private readonly IPersonsRepository _repository;
        private readonly ICollectionManager _collectionManager;

        public PersonsManager(IPersonsRepository personsRepository, ILoggerFactory loggerFactory,
            ICollectionManager collectionManager, IMemoryCache memoryCache) : base(loggerFactory, memoryCache)
        {
            _repository = personsRepository;
            _collectionManager = collectionManager;
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

        public async Task<PersonStatistics> GetStaticticsAsync()
        {
            var stats = await GetDataAsync(_repository.GetStaticticsAsync, typeof(PersonStatistics).Name).ConfigureAwait(false);

            return stats;
        }

        public async Task<PersonFormatedStatistics> GetFormatedStatisticsAsync()
        {
            PersonStatistics statistics = await GetStaticticsAsync().ConfigureAwait(false);

            var result = new List<PersonStatisticsPair>();

            var answerCount = statistics.Answers.Sum(s => s.Value);
            var accountCount = statistics.AccountTypes.Sum(a => a.Value);
            var stateCount = statistics.States.Sum(a => a.Value);
            var activeAccounts = statistics.ActiveAccounts.Sum(a => a.Value);

            //result.Add(new PersonStatisticsPair("Active Accounts", $"{activeAccounts} ({accountCount})"));
            //result.Add(new PersonStatisticsPair("Accounts", $"{statistics.TotalAccountCount}"));
            //result.Add(new PersonStatisticsPair("People", $"{statistics.TotalPersonCount}"));
            //result.Add(new PersonStatisticsPair(string.Empty, string.Empty));

            result.Add(new PersonStatisticsPair("Average", $"{statistics.Average:0.00} ({statistics.Min:0.0} ÷ {statistics.Max:0.0})"));

            foreach (var answer in statistics.Answers)
            {
                result.Add(new PersonStatisticsPair(answer.Key ? "Answered" : "I Don't Know", $"{answer.Value} ({(decimal)answer.Value / answerCount:P2})"));
            }

            result.Add(new PersonStatisticsPair("Answers", $"{answerCount}"));

            result.Add(new PersonStatisticsPair(string.Empty, string.Empty));


            //foreach (var types in statistics.AccountTypes)
            //{
            //    result.Add(new PersonStatisticsPair(types.Key, $"{types.Value} ({(decimal)types.Value / accountCount:P2})"));
            //}

            //result.Add(new PersonStatisticsPair(string.Empty, string.Empty));

            //foreach (var status in statistics.States)
            //{
            //    result.Add(new PersonStatisticsPair(status.Key, $"{status.Value} ({(decimal)status.Value / stateCount:P2})"));
            //}


            var accountStates = await _collectionManager.GetAllAccountStatesAsync().ConfigureAwait(false);
            var accounTypes = await _collectionManager.GetAllAccountTypesAsync().ConfigureAwait(false);

            var columns = 1 + 1 + accountStates.Count();
            var rows = 1 + accounTypes.Count();

            var additionalColumns = 2;
            var gridData = new List<List<string>>();
            var header = new List<string>
            {
                string.Empty,
                "Total",
                "Total Active",
            };

            foreach (var state in accountStates)
            {
                header.Add(state.Name);
            }

            header.Add("Not Asked");

            foreach (var type in accounTypes)
            {
                var row = new string[header.Count];
                row[0] = type.Name;

                gridData.Add(row.ToList());
            }

            foreach (var types in statistics.AccountTypes)
            {
                var row = types.TypeId - 1;
                if (row.HasValue && row < rows)
                {
                    gridData[row.Value][1] = $"{types.Value} ({(decimal)types.Value / accountCount:P2})";
                }
            }

            foreach (var types in statistics.ActiveAccounts)
            {
                var row = types.TypeId - 1;
                if (row.HasValue && row < rows)
                {
                    gridData[row.Value][2] = $"{types.Value} ({(decimal)types.Value / activeAccounts:P2})";
                }
            }

            foreach (var status in statistics.States)
            {
                var row = status.TypeId - 1;
                var column = status.Id ?? columns - 1;

                if (row.HasValue && row < rows)
                {
                    var count = statistics.AccountTypes.First(t => t.TypeId == status.TypeId).Value;
                    gridData[row.Value][column + additionalColumns] = $"{status.Value} ({(decimal)status.Value / count:P2})";
                }
            }

            var footer = new string[header.Count];

            footer[0] = string.Empty;
            footer[1] = $"{statistics.TotalAccountCount} in {statistics.TotalPersonCount}";
            footer[2] = $"{activeAccounts} ({(decimal)activeAccounts / statistics.TotalAccountCount:P2})";

            return new PersonFormatedStatistics()
            {
                Data = result,
                HeaderData = header,
                GridData = gridData,
                FooterData = footer,
            };
        }
    }
}
