using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Users.Data;

namespace Mihaylov.Users.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]    
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Works unauthorized");
        }

        [HttpGet]
        [JwtAuthorize()]
        public IActionResult Test2()
        {
            return Ok("Works with authorization");
        }

        [HttpGet]
        [JwtAuthorize(Roles = UserConstants.AdminRole)]
        public IActionResult Test3()
        {
            return Ok("Works with authorization, Admin");
        }

        [HttpGet]
        [Authorize()]
        public IActionResult Test4()
        {
            return Ok("Works with authorization, no scheme");
        }
    }
}
