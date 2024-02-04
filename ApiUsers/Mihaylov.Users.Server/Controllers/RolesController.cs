using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Common.Host.Authorization;
using Mihaylov.Users.Data.Interfaces;
using Mihaylov.Users.Models.Requests;
using Mihaylov.Users.Models.Responses;

namespace Mihaylov.Users.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    public class RolesController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public RolesController(IUsersRepository usersRepository)
        {
            this._usersRepository = usersRepository;
        }

        [HttpGet(Name = "RolesGetRoles")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoleModel>))]
        public async Task<IActionResult> GetRoles()
        {
            IEnumerable<RoleModel> roles = await this._usersRepository.GetRolesAsync().ConfigureAwait(false);
            return Ok(roles);
        }

        [HttpGet(Name = "RolesGetRoleById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleModel))]
        public async Task<IActionResult> GetRoleById(Guid roleId)
        {
            RoleModel role = await this._usersRepository.GetRoleByIdAsync(roleId).ConfigureAwait(false);
            return Ok(role);
        }

        [HttpPost(Name = "RolesAddRole")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddRole(CreateRoleRequest request)
        {
            GenericResponse response = await this._usersRepository.AddRoleAsync(request).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPut(Name = "RolesUpdateRole")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateRole(UpdateRoleRequest request)
        {
            GenericResponse response = await this._usersRepository.UpdateRoleAsync(request, UserConstants.AdminRole).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpPut(Name = "RolesAddRoleToUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserRequest request)
        {
            GenericResponse response = await this._usersRepository.AddRoleToUserAsync(request).ConfigureAwait(false);
            return Ok(response);
        }

        [HttpDelete(Name = "RolesDeleteRole")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            GenericResponse response = await this._usersRepository.DeleteRoleAsync(roleId).ConfigureAwait(false);
            return Ok(response);
        }
    }
}
