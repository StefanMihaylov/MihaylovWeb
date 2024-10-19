using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Contracts.Helpers;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Extensions;
using Mihaylov.Api.Site.Hubs;
using Mihaylov.Api.Site.Models;
using Mihaylov.Common.Host.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Mihaylov.Api.Site.Controllers
{
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class PersonController : ControllerBase
    {
        private readonly IPersonsManager _manager;
        private readonly IPersonsWriter _writer;
        private readonly ICollectionManager _collectionManager;
        private readonly ISiteHelper _siteHelper;
        private readonly IProgressReporterFactory _progressReporter;

        public PersonController(IPersonsManager manager, IPersonsWriter writer, ICollectionManager collectionManager,
            ISiteHelper siteHelper, IProgressReporterFactory progressReporter)
        {
            _manager = manager;
            _writer = writer;
            _collectionManager = collectionManager;
            _siteHelper = siteHelper;
            _progressReporter = progressReporter;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Grid<Person>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Persons([FromQuery] GridRequest request)
        {
            Grid<Person> persons = await _manager.GetAllPersonsAsync(request).ConfigureAwait(false);

            return Ok(persons);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        public async Task<IActionResult> Person(long id)
        {
            Person person = await _manager.GetPersonAsync(id).ConfigureAwait(false);

            return Ok(person);
        }

        [HttpPost]
        [SwaggerOperation(OperationId = "AddPerson")]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        public async Task<IActionResult> Person(AddPersonModel input)
        {
            var request = new Person()
            {
                Id = input.Id ?? 0,
                Details = input.Details == null || input.Details.IsEmpty() ? null : new PersonDetail()
                {
                    Id = input.Id ?? 0,
                    FirstName = input.Details.FirstName,
                    MiddleName = input.Details.MiddleName,
                    LastName = input.Details.LastName,
                    OtherNames = input.Details.OtherNames,
                },
                DateOfBirth = input.DateOfBirth,
                DateOfBirthType = input.DateOfBirthType,
                CountryId = input.CountryId,
                Country = null,
                Location = input.Location == null || input.Location.IsEmpty() ? null : new PersonLocation()
                {
                    Id = input.Id ?? 0,
                    CountryStateId = input.Location.CountryStateId,
                    CountryState = null,
                    City = input.Location.City,
                    Region = input.Location.Region,
                    Details = input.Location.Details,
                },
                EthnicityId = input.EthnicityId,
                Ethnicity = null,
                OrientationId = input.OrientationId,
                Orientation = null,
                Comments = input.Comments,
                CreatedOn = input.CreatedOn,
            };

            Person person = await _writer.AddOrUpdatePersonAsync(request, input.Age).ConfigureAwait(false);

            return Ok(person);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        public async Task<IActionResult> NewPerson(NewPersonModel input)
        {
            input.AccountTypeId ??= 1;
            input.IsPreview ??= true;

            var accountTypes = await _collectionManager.GetAllAccountTypesAsync().ConfigureAwait(false);

            var person = new Person()
            {
                DateOfBirthType = DateOfBirthType.YearCalculated,
                Accounts = new List<Account>()
                {
                    new Account()
                    {
                        AccountType = accountTypes.First(a => a.Id == input.AccountTypeId).Name,
                        AccountTypeId = input.AccountTypeId.Value,
                        Username = input.Username,
                        AskDate = DateTime.UtcNow,
                    }
                }
            };

            if (input.AccountTypeId == 3)
            {
                await _siteHelper.FillNewPersonAsync(person, input.Username).ConfigureAwait(false);
            }

            var account = person.Accounts.First();
            if (account.StatusId.HasValue)
            {
                var accountStates = await _collectionManager.GetAllAccountStatesAsync().ConfigureAwait(false);
                account.Status = accountStates.First(s => s.Id == account.StatusId.Value).Name;
            }

            if (!input.IsPreview.Value)
            {
                var newPerson = await _writer.AddNewPersonAsync(person, person.Age).ConfigureAwait(false);

                return Ok(newPerson);
            }

            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        public async Task<IActionResult> MergePerson(PersonMerge input)
        {
            Person person = await _writer.MergePersonsAsync(input).ConfigureAwait(false);

            return Ok(person);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        public async Task<IActionResult> Account(long id)
        {
            Account account = await _manager.GetAccountAsync(id).ConfigureAwait(false);

            return Ok(account);
        }

        [HttpPost]
        [SwaggerOperation(OperationId = "AddAccount")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        public async Task<IActionResult> Account(AddAccountModel input)
        {
            var request = new Account()
            {
                Id = input.Id ?? 0,
                PersonId = input.PersonId,
                AccountTypeId = input.AccountTypeId.Value,
                AccountType = null,
                StatusId = input.StatusId,
                Status = null,
                Username = input.Username,
                DisplayName = input.DisplayName,
                Details = input.Details,
                AskDate = input.AskDate,
                CreateDate = input.CreateDate,
                LastOnlineDate = input.LastOnlineDate,
                ReconciledDate = input.ReconciledDate,
            };

            Account account = await _writer.AddOrUpdateAccountAsync(request, input.Age).ConfigureAwait(false);

            return Ok(account);
        }

        [HttpPost]
        [SwaggerOperation(OperationId = "UpdateAccounts")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public IActionResult Accounts([FromForm] string connectionId)
        {
            var progressReporter = _progressReporter.GetLoadingBarReporter(connectionId, "updateAccountsLoadingBar");

            _siteHelper.UpdateAccountsAsync(null, 500, progressReporter.Report).GetAwaiter().GetResult();

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PersonStatistics), StatusCodes.Status200OK)]
        public async Task<IActionResult> Statistics()
        {
            PersonStatistics statistics = await _manager.GetStaticticsAsync().ConfigureAwait(false);

            return Ok(statistics);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PersonFormatedStatistics), StatusCodes.Status200OK)]
        public async Task<IActionResult> FormatedStatistics()
        {
            PersonFormatedStatistics statistics = await _manager.GetFormatedStatisticsAsync().ConfigureAwait(false);

            return Ok(statistics);
        }
    }
}
