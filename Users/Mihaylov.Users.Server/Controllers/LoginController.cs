using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Users.Data;
using Mihaylov.Users.Data.Repository;
using Mihaylov.Users.Models.Requests;

namespace Mihaylov.Users.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUsersRepository _repository;

        public LoginController(ILogger<LoginController> logger, IUsersRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequestModel request)
        {
            var response = await _repository.RegisterAsync(request).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            var response = await _repository.LoginAsync(request).ConfigureAwait(false);

            if (!response.Succeeded)
            {
                return Unauthorized();
            }

            return Ok(response);
        }
    }
}
