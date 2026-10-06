using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mihaylov.Common;

/// <summary>
/// API controller that exposes endpoints for retrieving module assembly information.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class ModuleController(IModuleAssemblyService moduleService) : ControllerBase
{
    /// <summary>
    /// Gets information about the module.
    /// </summary>
    /// <returns>An HTTP 200 (OK) response containing a ModuleInfo instance.</returns>
    [HttpGet(Name = "ModuleGetInfo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModuleInfo))]
    public IActionResult GetInfo()
    {
        var info = moduleService.GetModuleInfo();

        return Ok(info);
    }
}
