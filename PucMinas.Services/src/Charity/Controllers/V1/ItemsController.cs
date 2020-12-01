using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Domain.DTO.Item;
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
    public class ItemsController : ControllerBase
    {
        private ItemApplication ItemApplication { get; set; }
        private GroupApplication GroupApplication { get; set; }
        
        public ItemsController(ItemApplication itemApplication,
                              GroupApplication groupApplication)
        {
            this.ItemApplication = itemApplication;
            this.GroupApplication = groupApplication;
        }

        // GET: api/<controller>
        [ResponseWithLinks]
        [Authorize("items_read")]
        [HttpGet(Name = "GetItems")]
        public async Task<ActionResult<PagedResponse<ItemResponseDto>>> GetItems([FromQuery] PaginationParams paginationParams)
        {
            PagedResponse<ItemResponseDto> pagedResponse = await ItemApplication.GetAllItems(paginationParams);

            return Ok(pagedResponse);
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("items_read")]
        [HttpGet("{id}", Name = "GetItemById")]
        public async Task<ActionResult<ItemResponseDto>> GetItemById(Guid id)
        { 
            ItemResponseDto itemDto = await ItemApplication.GetItem(i => i.Id.Equals(id));

            if (itemDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O item, {id}, não foi encontrado.");
                return NotFound(error);
            }

            return Ok(itemDto);
        }

        // POST api/<controller>       
        [HttpPost(Name = "CreateItem")]
        public async Task<IActionResult> CreateItem([FromForm]ItemCreateDto itemDto)
        {
            var hasGroup = await GroupApplication.ExistGroup(g => g.Id.Equals(itemDto.GroupId));

            if (!hasGroup)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O grupo, {itemDto.GroupId}, não existe.");
                return BadRequest(error);
            }

            // Check if the item already exists
            bool hasItem = await ItemApplication.ExistItem((i) => i.Name.ToLower().Equals(itemDto.Name.ToLower()));

            if (hasItem)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O item, {itemDto.Name}, já existe.");
                return BadRequest(error);
            }
                    
            var itemId = await ItemApplication.CreateItem(itemDto, Request);

            return CreatedAtRoute("GetItemById", new { id = itemId }, null);                                   
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [HttpPut("{id}", Name = "UpdateItem")]        
        public async Task<ActionResult> UpdateItem(Guid id, [FromBody]ItemUpdateDto itemDto)
        {
            var hasGroup = await GroupApplication.ExistGroup(g => g.Id.Equals(itemDto.GroupId));

            if (!hasGroup)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O grupo, {itemDto.GroupId}, não existe.");
                return BadRequest(error);
            }

            // Check if the item exists
            ItemResponseDto item = await ItemApplication.GetItem(i => i.Id.Equals(id));

            if (item == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O item, {id}, não foi encontrado.");
                return NotFound(error);
            }

            if (item.Name != itemDto.Name)
            {
                // Check if the item already exists
                ItemResponseDto itemByName = await ItemApplication.GetItem((i) => i.Name.ToLower().Equals(itemDto.Name.ToLower()));

                if (itemByName != null)
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O item, {itemDto.Name}, já existe.");
                    return BadRequest(error);
                }
            }          
            
            await ItemApplication.UpdateItem(id, itemDto);

            return Ok();
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb/image
        [HttpPut("{id}/image", Name = "UpdateItemImage")]
        public async Task<ActionResult> UpdateItemImage(Guid id, [FromForm]IFormFile photo)
        {           
            // Check if the item exists
            ItemResponseDto item = await ItemApplication.GetItem(i => i.Id.Equals(id));

            if (item == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O item, {id}, não foi encontrado.");
                return NotFound(error);
            }
                      
            await ItemApplication.UpdateItemImage(id, photo, Request);

            return Ok();
        }

        // DELETE api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [HttpDelete("{id}", Name = "DeleteItem")]       
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            // Check if the item exists
            ItemResponseDto itemDto = await ItemApplication.GetItem(i => i.Id.Equals(id));

            if (itemDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O item, {id}, não foi encontrado.");
                return NotFound(error);
            }
            
            await ItemApplication.DeleteItem(itemDto);

            return NoContent();
        }
    }
}
