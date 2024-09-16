using Microsoft.AspNetCore.Mvc;
using Mihaylov.Api.Site.Contracts.Managers;

namespace Mihaylov.Api.Site.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ICollectionsManager _additionalInfo;

        public PersonController(ICollectionsManager additionalInfo)
        {
            _additionalInfo = additionalInfo;
        }

        [HttpGet]
        public IActionResult GetAccountTypes()
        {
            var accountTypes = _additionalInfo.GetAllAccountTypes();
            return Ok(accountTypes);
        }
    }
}
