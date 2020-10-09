using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Site.Core.Interfaces;

namespace Mihaylov.Site.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonAdditionalInfoManager _additionalInfo;

        public PersonController(IPersonAdditionalInfoManager additionalInfo)
        {
            _additionalInfo = additionalInfo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountTypes()
        {
            var accountTypes = _additionalInfo.GetAllAccountTypes();
            return Ok(accountTypes);
        }
    }
}
