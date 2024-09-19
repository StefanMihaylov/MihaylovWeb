using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Contracts.Managers;
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
        private readonly ICollectionManager _additionalInfo;

        public PersonController(ICollectionManager additionalInfo)
        {
            _additionalInfo = additionalInfo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountTypes()
        {
            var accountTypes = await _additionalInfo.GetAllAccountTypesAsync();

            return Ok(accountTypes);
        }
    }
}
