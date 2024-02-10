using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Users.Data.Interfaces;
using Mihaylov.Users.Models.Responses;

namespace Mihaylov.Users.Server.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("/api/[controller]/[action]")]    
    public class ExternalController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IExternalRepository _repository;

        public ExternalController(ILoggerFactory loggerFactory, IExternalRepository repository)
        {
            _logger = loggerFactory.CreateLogger(GetType().Name);
            _repository = repository;
        }

        [HttpGet(Name = "ExternalGetExternalSchemes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SchemeModel>))]
        public async Task<IActionResult> GetExternalSchemes()
        {
            var response = await _repository.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false);

            return Ok(response);
        }
    }
}
