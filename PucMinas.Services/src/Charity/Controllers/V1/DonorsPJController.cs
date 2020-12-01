using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Domain.DTO.DonorPJ;
using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Domain.Results;
using PucMinas.Services.Charity.Domain.Results.Exceptions;
using PucMinas.Services.Charity.Filters;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DonorsPJController : ControllerBase
    {
        private DonorPJApplication DonorPJApplication { get; set; }
        private UserApplication UserApplication { get; set; }

        public DonorsPJController(DonorPJApplication donorPJApplication,
                                 UserApplication userApplication)
        {
            this.DonorPJApplication = donorPJApplication;
            this.UserApplication = userApplication;
        }

        // GET: api/<controller>
        [Authorize("GetDonorsPJ")]
        [ResponseWithLinks]
        [HttpGet(Name = "GetDonorsPJ")]
        public async Task<ActionResult<PagedResponse<DonorPJResponseDto>>> GetDonorsPJ([FromQuery] PaginationParams paginationParams)
        {
            PagedResponse<DonorPJResponseDto> pagedResponse = await DonorPJApplication.GetAllDonorsPJ(paginationParams);

            return Ok(pagedResponse);
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("GetDonorPJById")]
        [HttpGet("{id}", Name = "GetDonorPJById")]
        public async Task<ActionResult<DonorPJResponseDto>> GetDonorPJById(Guid id)
        { 
            DonorPJResponseDto donorDto = await DonorPJApplication.GetDonorPJ(i => i.Id.Equals(id));

            if (donorDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O doador PJ, {id}, não foi encontrado.");
                return NotFound(error);
            }

            return Ok(donorDto);
        }

        // POST api/<controller>  
        [AllowAnonymous]
        [HttpPost(Name = "CreateDonorPJ")]
        public async Task<IActionResult> CreateDonorPJ([FromBody]DonorPJCreateDto donorDto)
        {
            var userDto = await UserApplication.GetUserDto(u => u.Login.ToLower().Equals(donorDto.Login.ToLower()));

            if (userDto != null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O usuário, {donorDto.Login}, já existe.");
                return BadRequest(error);
            }

            // Check if the donor already exists
            bool hasDonor = await DonorPJApplication.ExistDonorPJ(d => d.CNPJ.Equals(donorDto.CNPJ) && d.CompanyName.ToLower().Equals(donorDto.CompanyName.ToLower()));

            if (hasDonor)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O doador PJ, {donorDto.CNPJ} - {donorDto.CompanyName}, já existe.");
                return BadRequest(error);
            }

            var donorId = await DonorPJApplication.CreateDonorPJ(donorDto);

            return CreatedAtRoute("GetDonorPJById", new { id = donorId }, null);                                   
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("UpdateDonorPJ")]
        [HttpPut("{id}", Name = "UpdateDonorPJ")]
        public async Task<ActionResult> UpdateDonorPJ(Guid id, [FromBody]DonorPJUpdateDto donorDto)
        {
            // Check if the donor already exists
            var donorPJ = await DonorPJApplication.GetDonorPJ((d) => d.Id.Equals(id));

            if (donorPJ == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O doador PJ, {id}, não foi encontrado.");
                return BadRequest(error);
            }

            if (!donorPJ.CNPJ.Equals(donorDto.CNPJ) || !donorPJ.CompanyName.ToLower().Equals(donorDto.CompanyName.ToLower()))
            {
                // Check if the donor already exists
                bool hasDonor = await DonorPJApplication.ExistDonorPJ(d => d.CNPJ.Equals(donorDto.CNPJ) && d.CompanyName.ToLower().Equals(donorDto.CompanyName.ToLower()));

                if (hasDonor)
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O doador PJ, {donorDto.CNPJ} - {donorDto.CompanyName}, já existe.");
                    return BadRequest(error);
                }
            }
            
            await DonorPJApplication.UpdateDonorPJ(donorPJ, donorDto);
            
            return Ok();
        }

        // DELETE api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("DeleteDonorPJ")]
        [HttpDelete("{id}", Name = "DeleteDonorPJ")]
        public async Task<ActionResult> DeleteDonorPJ(Guid id)
        {
            // Check if the donor exists
            DonorPJResponseDto donorDto = await DonorPJApplication.GetDonorPJ(i => i.Id.Equals(id));

            if (donorDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O doador PJ, {id}, não foi encontrado.");
                return NotFound(error);
            }

            await DonorPJApplication.DeleteDonorPJ(donorDto);

            return NoContent();
        }
    }
}
