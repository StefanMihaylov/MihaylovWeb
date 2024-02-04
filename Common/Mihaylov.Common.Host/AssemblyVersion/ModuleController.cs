using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Common.Host.Abstract.AssemblyVersion;
using Mihaylov.Common.Host.AssemblyVersion.Models;

namespace Mihaylov.Common.Host.AssemblyVersion
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleAssemblyService _moduleService;

        public ModuleController(IModuleAssemblyService moduleService)
        {
            _moduleService = moduleService;
        }

        [HttpGet(Name = "ModuleGetInfo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModuleInfo))]
        public IActionResult GetInfo()
        {
            var info = _moduleService.GetModuleInfo();

            return Ok(info);
        }
    }
}
