using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Extensions;
using Mihaylov.Api.Site.Models;

namespace Mihaylov.Api.Site.Controllers
{
    //[JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]

    public class CollectionController : ControllerBase
    {
        private readonly ICollectionManager _manager;
        private readonly ICollectionWriter _writer;

        public CollectionController(ICollectionManager manager, ICollectionWriter writer)
        {
            _manager = manager;
            _writer = writer;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Country>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Countries()
        {
            IEnumerable<Country> countries = await _manager.GetAllCountriesAsync().ConfigureAwait(false);
            return Ok(countries);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Country), StatusCodes.Status200OK)]
        public async Task<IActionResult> Country(string name)
        {
            Country country = await _manager.GetCountryByNameAsync(name).ConfigureAwait(false);
            return Ok(country);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CountryState>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CountryStates(int id)
        {
            IEnumerable<CountryState> states = await _manager.GetAllStatesByCountryIdAsync(id).ConfigureAwait(false);
            return Ok(states);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Ethnicity>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Ethnicities()
        {
            IEnumerable<Ethnicity> ethnicities = await _manager.GetAllEthnicitiesAsync().ConfigureAwait(false);
            return Ok(ethnicities);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Orientation>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Orientations()
        {
            IEnumerable<Orientation> orientations = await _manager.GetAllOrientationsAsync().ConfigureAwait(false);
            return Ok(orientations);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountType>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AccountTypes()
        {
            IEnumerable<AccountType> accountTypes = await _manager.GetAllAccountTypesAsync().ConfigureAwait(false);
            return Ok(accountTypes);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AccountType), StatusCodes.Status200OK)]
        public async Task<IActionResult> AccountType(AccountTypeModel input)
        {
            var model = new AccountType()
            {
                Id = input.Id ?? 0,
                Name = input.Name,
            };

            AccountType accountType = await _writer.AddAccountTypeAsync(model).ConfigureAwait(false);
            return Ok(accountType);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountStatus>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AccountStates()
        {
            IEnumerable<AccountStatus> accountStates = await _manager.GetAllAccountStatesAsync().ConfigureAwait(false);
            return Ok(accountStates);
        }
    }
}
