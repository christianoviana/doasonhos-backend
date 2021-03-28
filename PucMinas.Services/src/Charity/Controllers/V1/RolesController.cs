using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Domain.DTO.Role;
using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Domain.Results;
using PucMinas.Services.Charity.Domain.Results.Exceptions;
using PucMinas.Services.Charity.Filters;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Controllers.V1
{
    [Authorize("administrator")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private RoleApplication RoleApplication { get; set; }

        public RolesController(RoleApplication roleApplication)
        {
            this.RoleApplication = roleApplication;
        }

        // GET: api/<controller>
        [ResponseWithLinks]
        [HttpGet(Name = "GetRoles")]
        public async Task<ActionResult<PagedResponse<RoleDto>>> GetRoles([FromQuery] FilterParams filterParams, [FromQuery] PaginationParams paginationParams)
        {
            PagedResponse<RoleDto> pagedResponse = await RoleApplication.GetAllRoles(filterParams, paginationParams);

            return Ok(pagedResponse);
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [HttpGet("{id}", Name = "GetRoleById")]
        public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
        { 
            RoleDto roleDto = await RoleApplication.GetRole(r => r.Id.Equals(id));

            if (roleDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"A regra, {id}, não foi encontrada.");
                return NotFound(error);
            }

            return Ok(roleDto);
        }

        // POST api/<controller>
        [HttpPost(Name = "CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody]RoleDto roleDto)
        {
            // Check if the role already exists
            bool hasRole = await RoleApplication.ExistRole((r) => r.Name.ToLower().Trim().Equals(roleDto.Name.ToLower().Trim()));

            if (hasRole)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A regra, {roleDto.Name}, já existe.");
                return BadRequest(error);
            }

            var roleId = await RoleApplication.CreateRole(roleDto);

            return CreatedAtRoute("GetRoleById", new { id = roleId }, null);                                   
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [HttpPut("{id}", Name = "UpdateRole")]
        public async Task<ActionResult> UpdateRole(Guid id, [FromBody]RoleDto roleDto)
        {
            // Check if the role exists
            RoleDto role = await RoleApplication.GetRole(r => r.Id.Equals(id));

            if (role == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"A regra, {id}, não foi encontrada.");
                return NotFound(error);
            }

            if (role.Name != roleDto.Name)
            {
                // Check if the role already exists
                RoleDto roleByName = await RoleApplication.GetRole((r) => r.Name.ToLower().Trim().Equals(roleDto.Name.ToLower().Trim()));

                if (roleByName != null)
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A regra, {roleDto.Name}, já existe.");
                    return BadRequest(error);
                }
            }          

            roleDto.Id = id;
            await RoleApplication.UpdateRole(roleDto);

            return Ok();
        }

        // DELETE api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [HttpDelete("{id}", Name = "DeleteRole")]
        public async Task<ActionResult> DeleteRole(Guid id)
        {
            // Check if the role exists
            RoleDto role = await RoleApplication.GetRole(r => r.Id.Equals(id));

            if (role == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"A regra, {id}, não foi encontrada.");
                return NotFound(error);
            }
            
            await RoleApplication.DeleteRole(role);

            return NoContent();
        }
    }
}
