using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Contracts.Managers;
using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Api.Site.Contracts.Models.Base;
using Mihaylov.Api.Site.Extensions;

namespace Mihaylov.Api.Site.Controllers
{
    //[JwtAuthorize(Roles = UserConstants.AdminRole)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [TypeFilter(typeof(ErrorFilter))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public class PersonController : ControllerBase
    {
        private readonly IPersonsManager _managwr;

        public PersonController(IPersonsManager manager)
        {
            _managwr = manager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Grid<Person>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Persons([FromQuery]GridRequest request)
        {
            Grid<Person> persons = await _managwr.GetAllPersonsAsync(request).ConfigureAwait(false);

            return Ok(persons);
        }
    }
}
