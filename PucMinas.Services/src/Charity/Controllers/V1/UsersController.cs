using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Domain.DTO.Role;
using PucMinas.Services.Charity.Domain.DTO.User;
using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Domain.Results;
using PucMinas.Services.Charity.Domain.Results.Exceptions;
using PucMinas.Services.Charity.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Controllers.V1
{
    [Authorize("administrator")]  
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {      
        private RoleApplication RoleApplication { get; set; }
        private UserApplication UserApplication { get; set; }

        public UsersController(RoleApplication roleApplication,                            
                              UserApplication userApplication)
        {          
            this.RoleApplication = roleApplication;
            this.UserApplication = userApplication;
        }

        // GET: api/<controller>
        [ResponseWithLinks]
        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult<PagedResponse<UserResponseDto>>> GetUsers([FromQuery] FilterParams filterParams, [FromQuery] PaginationParams paginationParams)
        {
            var pagedResponse = await UserApplication.GetAllUsers(filterParams, paginationParams);

            return Ok(pagedResponse);
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(Guid id)
        {
            var userDto = await UserApplication.GetUserDto(u => u.Id == id);

            if (userDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O usuário, {id}, não foi encontrado.");
                return NotFound(error);
            }

            return Ok(userDto);
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [HttpGet("{id}/owner", Name = "GetUserOwner")]
        public async Task<ActionResult<UserOwnerDto>> GetUserOwner(Guid id)
        {
            var userOwnerDto = await UserApplication.GetUserOwnerDto(id);

            if (userOwnerDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O usuário, {id}, não foi encontrado.");
                return NotFound(error);
            }

            return Ok(userOwnerDto);
        }

        // POST api/<controller>
        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody]UserCreateDto userDto)
        {
            var userResponse = await UserApplication.GetUser(u => u.Login.ToLower().Trim() == userDto.Login.ToLower().Trim(), false);
                      
            if (userResponse != null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O usuário, {userDto.Login}, já existe.");
                return BadRequest(error);
            }

            IEnumerable<RoleDto> lstRoles = null;
            RoleDto role = null;

            if (userDto.Roles?.Count > 0)
            {
                var distinctRoles = userDto.Roles.Distinct();

                if (distinctRoles.Count() != userDto.Roles.Count())
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"Não é possível cadastrar regras duplicadas.");
                    return BadRequest(error);
                }

                lstRoles = await RoleApplication.GetRoleIn(userDto.Roles);

                if ((lstRoles != null) && (lstRoles.Count() != userDto.Roles.Count()))
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"Existem regras inválidas.");
                    return BadRequest(error);
                }               
            }
            else
            {
                role = await RoleApplication.GetRole(r=>r.Name.ToUpper().Equals(userDto.type.ToUpper()));
                userDto.Roles = new List<Guid>() { role.Id };
            }

            var Id = await UserApplication.CreateUser(userDto);

            return CreatedAtRoute("GetUserById", new { id = Id }, null);
           
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [HttpPut("{id}", Name = "UpdateUser")]
        public async Task<ActionResult> UpdateUser(Guid id, [FromBody]UserUpdateDto userDto)
        {
            // Check if the user exists
            var user = await UserApplication.GetUser(u => u.Id == id, false);

            if (user == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O usuário, {id}, não foi encontrado.");
                return NotFound(error);
            }

            if (user.Type == LoginType.ADMINISTRATOR)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"Não foi possível atualizar o usuário.");
                return BadRequest(error);
            }

            if (user.Login.ToLower() != userDto.Login.ToLower())
            {
                var user_login_check = await UserApplication.GetUserDto(u => u.Login.ToLower() == userDto.Login.ToLower(), false);

                if (user_login_check != null)
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O usuário, {userDto.Login}, já existe.");
                    return BadRequest(error);
                }
            }

            await UserApplication.UpdateUser(id, userDto);

            return Ok();
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [HttpPut("{id}/roles", Name = "UpdateUserRole")]
        public async Task<ActionResult> UpdateUserRole(Guid id, [FromBody]UserRoleUpdateDto roleDto)
        {           

            // Check if the user exists
            var user = await UserApplication.GetUser(u => u.Id == id, false);

            if (user == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O usuário, {id}, não foi encontrado.");
                return NotFound(error);
            }
                      
            IEnumerable<RoleDto> lstRoles = null;

            if (roleDto.Roles.Count > 0)
            {
                var distinctRoles = roleDto.Roles.Distinct();

                if (distinctRoles.Count() != roleDto.Roles.Count())
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"Não é possível cadastrar regras duplicadas.");
                    return BadRequest(error);
                }

                lstRoles = await RoleApplication.GetRoleIn(roleDto.Roles);

                if (lstRoles.Count() != roleDto.Roles.Count())
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"Existem regras inválidas.");
                    return BadRequest(error);
                }              
            }

            await UserApplication.UpdateAllUserRoles(user, lstRoles);

            return Ok();
        }

        // DELETE api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            // Check if the user exists
            User user = await UserApplication.GetUser(u => u.Id.Equals(id));

            if (user == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O usuário, {id}, não foi encontrado.");
                return NotFound(error);
            }

            if (user.Type == LoginType.ADMINISTRATOR)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"Não foi possível apagar o usuário.");
                return BadRequest(error);
            }

            await UserApplication.DeleteUser(user);

            return NoContent();
        }
    }
}
