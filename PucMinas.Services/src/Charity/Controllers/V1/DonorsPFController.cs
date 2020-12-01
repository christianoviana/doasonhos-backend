using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Domain.DTO.DonorPF;
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
    public class DonorsPFController : ControllerBase
    {
        private DonorPFApplication DonorPFapplication { get; set; }
        private UserApplication UserApplication { get; set; }

        public DonorsPFController(DonorPFApplication donorPFApplication,
                                 UserApplication userApplication)
        {
            this.DonorPFapplication = donorPFApplication;
            this.UserApplication = userApplication;
        }

        // GET: api/<controller>
        [Authorize("GetDonorsPF")]
        [ResponseWithLinks]
        [HttpGet(Name = "GetDonorsPF")]
        public async Task<ActionResult<PagedResponse<DonorPFResponseDto>>> GetDonorsPF([FromQuery] PaginationParams paginationParams)
        {
            PagedResponse<DonorPFResponseDto> pagedResponse = await DonorPFapplication.GetAllDonorsPF(paginationParams);

            return Ok(pagedResponse);
        }

        // GET api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("GetDonorPFById")]
        [HttpGet("{id}", Name = "GetDonorPFById")]
        public async Task<ActionResult<DonorPFResponseDto>> GetDonorPFById(Guid id)
        { 
            DonorPFResponseDto donorDto = await DonorPFapplication.GetDonorPF(i => i.Id.Equals(id));

            if (donorDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O doador PF, {id}, não foi encontrado.");
                return NotFound(error);
            }

            return Ok(donorDto);
        }

        // POST api/<controller>  
        [AllowAnonymous]
        [HttpPost(Name = "CreateDonorPF")]
        public async Task<IActionResult> CreateDonorPF([FromBody]DonorPFCreateDto donorDto)
        {
            var userDto = await UserApplication.GetUserDto(u => u.Login.ToLower().Equals(donorDto.Login.ToLower()));

            if (userDto != null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O usuário, {donorDto.Login}, já existe.");
                return BadRequest(error);
            }

            // Check if the donor already exists
            bool hasDonor = await DonorPFapplication.ExistDonorPF((d) => d.CPF.Equals(donorDto.CPF.ToLower()));

            if (hasDonor)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O doador PF, {donorDto.CPF}, já existe.");
                return BadRequest(error);
            }

            var donorId = await DonorPFapplication.CreateDonorPF(donorDto);

            return CreatedAtRoute("GetDonorPFById", new { id = donorId }, null);                                   
        }

        // PUT api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("UpdateDonorPF")]
        [HttpPut("{id}", Name = "UpdateDonorPF")]
        public async Task<ActionResult> UpdateDonorPF(Guid id, [FromBody]DonorPFUpdateDto donorDto)
        {
            // Check if the donor already exists
            var donorPF = await DonorPFapplication.GetDonorPF((d) => d.Id.Equals(id));

            if (donorPF == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O doador PF, {id}, não foi encontrado.");
                return BadRequest(error);
            }

            if (!donorPF.CPF.Equals(donorDto.CPF))
            {
                // Check if the donor already exists
                bool hasDonor = await DonorPFapplication.ExistDonorPF((d) => d.CPF.Equals(donorDto.CPF));

                if (hasDonor)
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O doador PF, {donorDto.CPF}, já existe.");
                    return BadRequest(error);
                }
            }
            
            await DonorPFapplication.UpdateDonorPF(donorPF, donorDto);
            
            return Ok();
        }

        // DELETE api/<controller>/51110be2-5bbf-4e97-bbf5-a042ddf5d8eb
        [Authorize("DeleteDonorPF")]
        [HttpDelete("{id}", Name = "DeleteDonorPF")]
        public async Task<ActionResult> DeleteDonorPF(Guid id)
        {
            // Check if the donor exists
            DonorPFResponseDto donorDto = await DonorPFapplication.GetDonorPF(i => i.Id.Equals(id));

            if (donorDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"O doador PF, {id}, não foi encontrado.");
                return NotFound(error);
            }

            await DonorPFapplication.DeleteDonorPF(donorDto);

            return NoContent();
        }
    }
}
