using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Site.Core.Interfaces;

namespace Mihaylov.Site.Server.Controllers
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
