using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;
using Mihaylov.Api.Site.Contracts.Writers;
using Mihaylov.Api.Site.Extensions;
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

        public PersonController(IPersonsManager manager, IPersonsWriter writer)
        {
            _manager = manager;
            _writer = writer;
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
            Person person = await _manager.GetByIdAsync(id).ConfigureAwait(false);

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
            };

            Person person = await _writer.AddOrUpdateAsync(request).ConfigureAwait(false);

            return Ok(person);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PersonStatistics), StatusCodes.Status200OK)]
        public async Task<IActionResult> Statistics()
        {
            PersonStatistics statistics = await _manager.GetStaticticsAsync().ConfigureAwait(false);

            return Ok(statistics);
        }
    }
}
