using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Users.Data.Interfaces;
using Mihaylov.Users.Models.Requests;
using Mihaylov.Users.Models.Responses;

namespace Mihaylov.Users.Server.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("/api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUsersRepository _repository;

        public LoginController(ILoggerFactory loggerFactory, IUsersRepository repository)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _repository = repository;
        }

        [HttpPost(Name = "LoginRegister")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        public async Task<IActionResult> Register(RegisterRequestModel request)
        {
            var response = await _repository.RegisterAsync(request).ConfigureAwait(false);

            return Ok(response);
        }

        [HttpPost(Name = "LoginLogin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            var response = await _repository.LoginAsync(request).ConfigureAwait(false);

            return Ok(response);
        }
    }
}
