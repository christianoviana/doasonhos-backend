using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Domain.DTO.Group;
using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Domain.Results;
using PucMinas.Services.Charity.Domain.Results.Exceptions;
using PucMinas.Services.Charity.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private GroupApplication GroupApplication { get; set; }

        public GroupsController(GroupApplication groupApplication)
        {
            this.GroupApplication = groupApplication;
        }

        // GET: api/<controller>
        [Authorize("administrator")]
        [ResponseWithLinks]
        [HttpGet(Name = "GetGroups")]
        public async Task<ActionResult<PagedResponse<GroupResponseDto>>> GetGroups([FromQuery] PaginationParams paginationParams)
        {
            PagedResponse<GroupResponseDto> pagedResponse = await GroupApplication.GetAllGroups(paginationParams);

            return Ok(pagedResponse);
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("administrator")]
        [HttpGet("{id}", Name = "GetGroupById")]
        public async Task<ActionResult<GroupResponseDto>> GetGroupById(Guid id)
        { 
            GroupResponseDto groupDto = await GroupApplication.GetGroup(g => g.Id.Equals(id));

            if (groupDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O grupo, {id}, não foi encontrado.");
                return NotFound(error);
            }

            return Ok(groupDto);
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("administrator")]
        [HttpGet("{id}/items", Name = "GetGroupItemsById")]
        public async Task<ActionResult<GroupItemsResponseDto>> GetGroupItemsById(Guid id)
        {
            GroupItemsResponseDto groupItemsDto = await GroupApplication.GetGroupItems(g => g.Id.Equals(id));

            if (groupItemsDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O grupo, {id}, não foi encontrado.");
                return NotFound(error);
            }

            return Ok(groupItemsDto);
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("GetGroupItems")]
        [HttpGet("items", Name = "GetGroupItems")]
        public async Task<ActionResult<IEnumerable<GroupItemsResponseDto>>> GetGroupItems()
        {
            IEnumerable<GroupItemsResponseDto> groupItemsDto = await GroupApplication.GetGroupItems();

            return Ok(groupItemsDto);
        }

        // POST api/<controller>
        [Authorize("administrator")]
        [HttpPost(Name = "CreateGroup")]
        public async Task<IActionResult> CreateGroup([FromBody]GroupRequestDto groupDto)
        {
            // Check if the group already exists
            bool hasGroup = await GroupApplication.ExistGroup((g) => g.Name.ToLower().Equals(groupDto.Name.ToLower()));

            if (hasGroup)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.Conflict, $"O grupo, {groupDto.Name}, já existe.");
                return Conflict(error);
            }
            
            var groupId = await GroupApplication.CreateGroup(groupDto);
            return CreatedAtRoute("GetGroupById", new { id = groupId }, null);                                
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("administrator")]
        [HttpPut("{id}", Name = "UpdateGroup")]
        public async Task<ActionResult> UpdateGroup(Guid id, [FromBody]GroupRequestDto groupDto)
        {
            // Check if the group exists
            GroupResponseDto group = await GroupApplication.GetGroup(g => g.Id.Equals(id));

            if (group == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O grupo, {id}, não foi encontrado.");
                return NotFound(error);
            }

            if (group.Name != groupDto.Name)
            {
                // Check if the group already exists
                GroupResponseDto groupByName = await GroupApplication.GetGroup((g) => g.Name.ToLower().Equals(groupDto.Name.ToLower()));

                if (groupByName != null)
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O grupo, {groupDto.Name}, já existe.");
                    return BadRequest(error);
                }
            }          

            await GroupApplication.UpdateGroup(id, groupDto);

            return Ok();
        }

        // DELETE api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("administrator")]
        [HttpDelete("{id}", Name = "DeleteGroup")]
        public async Task<ActionResult> DeleteGroup(Guid id)
        {
            // Check if the group exists
            GroupResponseDto groupDto = await GroupApplication.GetGroup(r => r.Id.Equals(id));

            if (groupDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O grupo, {id}, não foi encontrado.");
                return NotFound(error);
            }
            
            await GroupApplication.DeleteGroup(groupDto);

            return NoContent();
        }
    }
}
