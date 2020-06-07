using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Users.Data.Repository;
using Mihaylov.Users.Data.Repository.Helpers;
using Mihaylov.Users.Data.Repository.Models;

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

        public IActionResult Test()
        {
            return Ok("Works unauthorized");
        }

        [Authorize(AuthenticationSchemes = TokenHelper.AUTHENTICATION_SCHEME)]
       // [Authorize()]
        public IActionResult Test2()
        {
            return Ok("Works with authorization");
        }

        [Authorize(AuthenticationSchemes = TokenHelper.AUTHENTICATION_SCHEME, Roles = "Admin")]
        public IActionResult Test3()
        {
            return Ok("Works with authorization, Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestModel request)
        {
            var response = await _repository.RegisterAsync(request);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            var response = await _repository.LoginAsync(request);

            if (!response.Succeeded)
            {
                return Unauthorized();
            }

            return Ok(response);
        }
    }
}
