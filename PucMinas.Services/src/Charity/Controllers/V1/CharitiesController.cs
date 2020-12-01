using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Domain.DTO.Charity;
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
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CharitiesController : ControllerBase
    {
        private CharitableEntityApplication CharitableEntityApplication { get; set; }
        private CharitableInformationApplication CharitableInformationApplication { get; set; }
        private UserApplication UserApplication { get; set; }
        private ItemApplication ItemApplication { get; set; }

        public CharitiesController(CharitableEntityApplication charitableEntityApplication,
                                    CharitableInformationApplication charitableInformationApplication,
                                    UserApplication userApplication,
                                    ItemApplication itemApplication)
        {
            this.CharitableEntityApplication = charitableEntityApplication;
            this.CharitableInformationApplication = charitableInformationApplication;
            this.UserApplication = userApplication;
            this.ItemApplication = itemApplication;
        }

        // GET: api/<controller>
        [AllowAnonymous]
        [ResponseWithLinks]
        [HttpGet(Name = "GetCharities")]
        public async Task<ActionResult<PagedResponse<CharityResponseDto>>> GetCharities([FromQuery] PaginationParams paginationParams)
        {
            PagedResponse<CharityResponseDto> pagedResponse = await CharitableEntityApplication.GetAllCharities(paginationParams, true);

            return Ok(pagedResponse);
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetCharityById")]
        public async Task<ActionResult<CharityResponseDto>> GetCharityById(Guid id)
        {
            CharityResponseDto charityDto = await CharitableEntityApplication.GetCharity(c => c.Id.Equals(id), true);

            if (charityDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"A entidade beneficente, {id}, não foi encontrada.");
                return NotFound(error);
            }

            return Ok(charityDto);
        }

        // POST api/<controller>  
        [AllowAnonymous]
        [HttpPost(Name = "CreateCharity")]
        public async Task<IActionResult> CreateCharity([FromBody]CharityCreateDto charityDto)
        {
            var userDto = await UserApplication.GetUserDto(u => u.Login.ToLower().Equals(charityDto.Login.ToLower()));

            if (userDto != null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O usuário, {charityDto.Login}, já existe.");
                return BadRequest(error);
            }

            // Check if the charity entity already exists
            bool hasCharity = await CharitableEntityApplication.ExistCharity((c) => c.Cnpj.Equals(charityDto.Cnpj));

            if (hasCharity)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A entidade beneficente, {charityDto.Cnpj}, já existe.");
                return BadRequest(error);
            }

            var charityId = await CharitableEntityApplication.CreateCharity(charityDto);

            return CreatedAtRoute("GetCharityById", new { id = charityId }, null);
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("UpdateCharity")]
        [HttpPut("{id}", Name = "UpdateCharity")]
        public async Task<ActionResult> UpdateCharity(Guid id, [FromBody]CharityUpdateDto charityDto)
        {
            // Check if the donor already exists
            var charity = await CharitableEntityApplication.GetCharity((c) => c.Id.Equals(id));

            if (charity == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A entidade beneficente, {id}, não foi encontrada.");
                return NotFound(error);
            }

            if (!charity.Cnpj.Equals(charityDto.Cnpj))
            {
                // Check if the charity entity already exists
                bool hasCharity = await CharitableEntityApplication.ExistCharity((c) => c.Cnpj.Equals(charityDto.Cnpj));

                if (hasCharity)
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A entidade beneficente, {charityDto.Cnpj}, já existe.");
                    return BadRequest(error);
                }
            }

            await CharitableEntityApplication.UpdateCharity(id, charityDto);

            return Ok();
        }

        // DELETE api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("DeleteCharity")]
        [HttpDelete("{id}", Name = "DeleteCharity")]
        public async Task<ActionResult> DeleteCharity(Guid id)
        {
            // Check if the donor exists
            CharityResponseDto charityDto = await CharitableEntityApplication.GetCharity(c => c.Id.Equals(id));

            if (charityDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"A entidade beneficente, {id}, não foi encontrada.");
                return NotFound(error);
            }

            await CharitableEntityApplication.DeleteCharity(id);

            return NoContent();
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb/item
        [AllowAnonymous]
        [HttpGet("{id}/item", Name = "GetCharityItem")]
        public async Task<ActionResult<CharityInfoItemResponseDto>> GetCharityItem(Guid id)
        {
            CharityResponseDto charityDto = await CharitableEntityApplication.GetCharity(c => c.Id.Equals(id), true);

            if (charityDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"A entidade beneficente, {id}, não foi encontrada.");
                return NotFound(error);
            }

            if (charityDto.Information == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"As infomações da entidade beneficente não foram cadastradas.");
                return BadRequest(error);
            }

            var charityInfoItems = await CharitableInformationApplication.GetCharityInfoItem(p => p.CharitableInformationId == charityDto.Information.Id);

            return Ok(charityInfoItems);
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb/item
        [Authorize("UpdateCharityInfoItem")]
        [HttpPut("{id}/item", Name = "UpdateCharityInfoItem")]
        public async Task<ActionResult> UpdateCharityInfoItem(Guid id, [FromBody]CharityInfoItemDto charityDto)
        {
            // Check if the donor already exists
            var charityResponse = await CharitableEntityApplication.GetCharity((c) => c.Id.Equals(id), true);

            if (charityResponse == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A entidade beneficente, {id}, não foi encontrada.");
                return NotFound(error);
            }

            if (charityResponse.Information == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"As infomações da entidade beneficente não foram cadastradas.");
                return BadRequest(error);
            }

            if (charityDto.items != null && charityDto.items.Count > 0)
            {
                var distinctItems = charityDto.items.Distinct();

                if (distinctItems.Count() != charityDto.items.Count())
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"Não é possível inserir itens duplicados.");
                    return BadRequest(error);
                }

                var lstItems = await ItemApplication.GetItemIn(charityDto.items);

                if (lstItems.Count() != charityDto.items.Count())
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"Existem itens inválidos.");
                    return BadRequest(error);
                }
            }

            await CharitableInformationApplication.UpdateCharityInfoItem(id, charityDto);

            return Ok();
        }
        
        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb/information
        [Authorize("UpdateCharityInfo")]
        [HttpPut("{id}/information", Name = "UpdateCharityInfo")]
        public async Task<ActionResult> UpdateCharityInfo(Guid id, [FromBody]CharityInfoUpdateDto charityDto)
        {
            // Check if the donor already exists
            var charityResponse = await CharitableEntityApplication.GetCharity((c) => c.Id.Equals(id), true);

            if (charityResponse == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A entidade beneficente, {id}, não foi encontrada.");
                return NotFound(error);
            }

            if (charityResponse.Information == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"As infomações da entidade beneficente não foram cadastradas.");
                return BadRequest(error);
            }          

            await CharitableInformationApplication.UpdateCharityInfo(id, charityDto, Request);

            return Ok();
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb/information/image
        [Authorize("UpdateCharityInfoImage")]
        [HttpPut("{id}/information/image", Name = "UpdateCharityInfoImage")]
        public async Task<ActionResult> UpdateCharityInfoImage(Guid id, [FromForm]CharityInfoImageUpdateDto charityDto)
        {
            // Check if the donor already exists
            var charityResponse = await CharitableEntityApplication.GetCharity((c) => c.Id.Equals(id), true);
            var imageName = charityDto.Name.ToLower();

            if (imageName != "picture" && imageName != "image01" && imageName != "image02")
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O nome da imagem deve ser picture, image01 ou image02");
                return BadRequest(error);
            }

            if (charityResponse == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A entidade beneficente, {id}, não foi encontrada.");
                return NotFound(error);
            }

            if (charityResponse.Information == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"As infomações da entidade beneficente não foram cadastradas.");
                return BadRequest(error);
            }

            await CharitableInformationApplication.UpdateCharityInfoImage(id, charityDto, charityResponse.Information, Request);

            return Ok();
        }

        // POST api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb/information
        [Authorize("CreateCharityInfo")]
        [HttpPost("{id}/information", Name = "CreateCharityInfo")]
        public async Task<ActionResult> CreateCharityInfo(Guid id, [FromForm]CharityInfoCreateDto charityDto)
        {
            // Check if the donor already exists
            var charityResponse = await CharitableEntityApplication.GetCharity((c) => c.Id.Equals(id), true);

            if (charityResponse == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A entidade beneficente, {id}, não foi encontrada.");
                return NotFound(error);
            }

            if (charityResponse.Information != null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"As informações da entidade beneficente já foram cadastradas.");
                return BadRequest(error);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(charityResponse.Approver) || charityResponse.Active == false)
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"Não foi possível cadastrar as informações da entidade benefiecente porque a entidade não foi aprovada.");
                    return BadRequest(error);
                }
            }         

            await CharitableInformationApplication.CreateCharityInfo(id, charityDto, Request);

            return CreatedAtRoute("GetCharityById", new { id }, null); ;
        }
    }
}
