﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mihaylov.Users.Data;
using Mihaylov.Users.Data.Repository;
using Mihaylov.Users.Data.Repository.Models;

namespace Mihaylov.Users.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]    
    [JwtAuthorize(Roles = UserConstants.AdminRole)]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            this._usersRepository = usersRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await this._usersRepository.GetUsersAsync();
            return Ok(users);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserRequest request)
        {
            var response = await this._usersRepository.AddRoleAsync(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var response = await this._usersRepository.DeleteUserAsync(id);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleModel))]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await this._usersRepository.GetRolesAsync();
            return Ok(roles);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddRole(CreateRoleRequest request)
        {
            var response = await this._usersRepository.AddRoleAsync(request);
            return Ok(response);
        }
    }
}