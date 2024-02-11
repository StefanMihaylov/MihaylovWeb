using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Common.Host.Authorization;
using Mihaylov.Users.Data.Database.Models;
using Mihaylov.Users.Data.Interfaces;
using Mihaylov.Users.Models.Requests;
using Mihaylov.Users.Models.Responses;

namespace Mihaylov.Users.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [JwtAuthorize()]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            this._usersRepository = usersRepository;
        }

        [HttpGet(Name = "UsersGetUsers")]
        [JwtAuthorize(Roles = UserConstants.AdminRole)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserModel>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUsers()
        {
            IEnumerable<UserModel> users = await this._usersRepository.GetUsersAsync().ConfigureAwait(false);
            return Ok(users);
        }

        [HttpGet(Name = "UsersGetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            if (!User.IsOwnId(userId))
            {
                return Unauthorized($"Incorrect UserId {userId}");
            }

            UserModel users = await this._usersRepository.GetUserAsync(userId.ToString()).ConfigureAwait(false);
            return Ok(users);
        }

        [HttpPut(Name = "UsersChangePassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if (!User.IsOwnId(request.UserId))
            {
                return Unauthorized($"Incorrect UserId {request.UserId}");
            }

            GenericResponse response = await this._usersRepository.ChangePasswordAsync(request).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPut(Name = "UsersUpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser(UpdateUserModel request)
        {
            if (!User.IsOwnId(request.UserId))
            {
                return Unauthorized($"Incorrect UserId {request.UserId}");
            }

            GenericResponse response = await this._usersRepository.UpdateUserAsync(request).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpDelete(Name = "UsersDeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (!User.IsOwnId(userId))
            {
                return Unauthorized($"Incorrect UserId {userId}");
            }

            GenericResponse response = await this._usersRepository.DeleteUserAsync(userId).ConfigureAwait(false);
            return Ok(response);
        }
    }
}