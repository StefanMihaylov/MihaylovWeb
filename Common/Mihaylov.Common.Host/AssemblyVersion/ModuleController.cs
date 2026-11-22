using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mihaylov.Common;

/// <summary>
/// API controller that exposes endpoints for retrieving module assembly information.
/// </summary>
[AdminAuthorize]
[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class ModuleController(IModuleAssemblyService moduleService) : ControllerBase
{
    /// <summary>
    /// Gets information about the module.
    /// </summary>
    /// <returns>An HTTP 200 (OK) response containing a ModuleInfo instance.</returns>
    [HttpGet(Name = IModuleInfoService.MODULE_ACTION_NAME)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModuleInfo))]
    public IActionResult GetInfo()
    {
        var info = moduleService.GetModuleInfo();

        return Ok(info);
    }

    /// <summary>
    /// Gets the module dependencies.
    /// </summary>
    /// <returns>An HTTP 200 response containing the module dependencies.</returns>
    [HttpGet(Name = IModuleInfoService.DEPENDENCIES_ACTION_NAME)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DependencyNodeFlat>))]
    public IActionResult GetDependencies()
    {
        var dependencies = moduleService.GetDependencies();
        var flatDependencies = dependencies.ToFlatModel();

        return Ok(flatDependencies);
    }
}
