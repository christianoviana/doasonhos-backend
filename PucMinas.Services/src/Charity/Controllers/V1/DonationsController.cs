using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Domain.DTO.Donation;
using PucMinas.Services.Charity.Domain.DTO.Item;
using PucMinas.Services.Charity.Domain.Parameters;
using PucMinas.Services.Charity.Domain.Results;
using PucMinas.Services.Charity.Domain.Results.Exceptions;

namespace PucMinas.Services.Charity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private DonationApplication DonationApplication { get; set; }
        private ItemApplication ItemApplication { get; set; }
        private CharitableEntityApplication CharitableEntityApplication { get; set; }
        private UserApplication UserApplication { get; set; }

        public IEmailSender EmailSender { get; set; }


        public DonationsController(DonationApplication donationApplication, 
                                   ItemApplication itemApplication,
                                   CharitableEntityApplication charitableEntityApplication,
                                   IEmailSender emailSender,   
                                   UserApplication userApplication)
        {
            this.DonationApplication = donationApplication;
            this.ItemApplication = itemApplication;
            this.CharitableEntityApplication = charitableEntityApplication;
            this.UserApplication = userApplication;
            this.EmailSender = emailSender;
        }

        // GET api/<controller>
        [Authorize("GetDonationById")]
        [HttpGet("{id}", Name = "GetDonationById")]
        public async Task<ActionResult> GetDonationById([FromRoute] Guid id)
        {
            var donation = await DonationApplication.GetDonation(d => d.Id.Equals(id));

            if (donation == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"A doação, ${id}, não foi encontrada.");
                return NotFound(error);
            }

            return Ok(donation);
        }

        // GET api/<controller>
        [Authorize("CancelDonationById")]
        [HttpPut("{id}/cancel", Name = "CancelDonationById")]
        public async Task<ActionResult> CancelDonationById([FromRoute] Guid id)
        {
            var donation = await DonationApplication.GetDonation(d => d.Id.Equals(id));

            if (donation == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"A doação, ${id}, não foi encontrada.");
                return NotFound(error);
            }

            await DonationApplication.CancelDonation(id);

            return Ok();
        }

        // GET api/<controller>
        [Authorize("ApproveDonationById")]
        [HttpPut("{id}/approve", Name = "ApproveDonationById")]
        public async Task<ActionResult> ApproveDonationById([FromRoute] Guid id)
        {
            var donation = await DonationApplication.GetDonation(d => d.Id.Equals(id));

            if (donation == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.NotFound, $"A doação, ${id}, não foi encontrada.");
                return NotFound(error);
            }

            await DonationApplication.ApproveDonation(id);

            return Ok();
        }


        // GET api/<controller>
        [Authorize("GetCharityDonationById")]
        [HttpGet("charities/{id}", Name = "GetCharityDonationById")]
        public async Task<ActionResult> GetCharityDonationById([FromRoute] Guid id, [FromQuery] PaginationParams paginationParams)
        {
            PagedResponse<DonationResponseDto> pagedResponse = await DonationApplication.GetAllDonation(d => d.CharitableEntityId.Equals(id), paginationParams);

            return Ok(pagedResponse);
        }

        // GET api/<controller>
        [Authorize("GetDonorsDonationById")]
        [HttpGet("donors/{id}", Name = "GetDonorsDonationById")]
        public async Task<ActionResult<PagedResponse<DonationResponseDto>>> GetDonorsDonationById([FromRoute] Guid id, [FromQuery] PaginationParams paginationParams)
        {
            PagedResponse<DonationResponseDto> pagedResponse = await DonationApplication.GetAllDonation(d => d.UserId.Equals(id), paginationParams);
                      
            return Ok(pagedResponse);
        }

        // POST api/<controller>
        [Authorize("CreateDonationOnline")]
        [HttpPost("online", Name = "CreateDonationOnline")]
        public async Task<ActionResult> CreateDonationOnline([FromBody]DonationCreateDto donationCreateDto)
        {
            if (donationCreateDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O parâmetro, DonationCreate, não pode ser nulo.");
                return BadRequest(error);
            }

            // Check if charity exists
            var charity = await CharitableEntityApplication.GetCharity((c) => c.Id.Equals(donationCreateDto.CharitableEntityId), true);

            if (charity == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A entidade beneficente, {donationCreateDto.CharitableEntityId}, não foi encontrada.");
                return NotFound(error);
            }

            // Check if user exists
            var user = await UserApplication.GetUser((u) => u.Id.Equals(donationCreateDto.UserId));
         
            if (user == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O usuário, {donationCreateDto.UserId}, não foi encontrado.");
                return NotFound(error);
            }

            if (donationCreateDto?.DonationItem.Count() > 0)
            {              
                var lstItems = await ItemApplication.GetCharityItemIn(charity.Information.Id ,donationCreateDto.DonationItem.Select(e=>e.ItemId).ToList());

                if (lstItems.Count() != donationCreateDto.DonationItem.Count())
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"Existem itens inválidos.");
                    return BadRequest(error);
                }
            }

            var donationId= await DonationApplication.CreateDonation(donationCreateDto, true);

            try
            {
                // Sent an email
                var donor = await UserApplication.GetUserOwnerDto(donationCreateDto.UserId);
                string message = string.Format(" <h1>Olá, {0}</h1><h2>Doação online realizada com sucesso no valor total de {1} reais para a entidade {2} ({3}).</h2>", donor.Name, donationCreateDto.Total.ToString("C"), charity.Name, charity.Cnpj);

                await this.EmailSender.SendEmailAsync(user.Login, $"Doa Sonhos - Doação Online", message);
            }
            catch (Exception)
            {
            }         

            return CreatedAtRoute("GetDonationById", new { id = donationId }, null);
        }

        // POST api/<controller>
        [Authorize("CreateDonationPresential")]
        [HttpPost("presential", Name = "CreateDonationPresential")]
        public async Task<ActionResult> CreateDonationPresencial([FromBody]DonationCreateDto donationCreateDto)
        {
            if (donationCreateDto == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O parâmetro, DonationCreate, não pode ser nulo.");
                return BadRequest(error);
            }
                      
            // Check if the charity exists
            var charity = await CharitableEntityApplication.GetCharity((c) => c.Id.Equals(donationCreateDto.CharitableEntityId), true);

            if (charity == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A entidade beneficente, {donationCreateDto.CharitableEntityId}, não foi encontrada.");
                return NotFound(error);
            }

            // Check if user exists
            var user = await UserApplication.GetUser((u) => u.Id.Equals(donationCreateDto.UserId));

            if (user == null)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O usuário, {donationCreateDto.UserId}, não foi encontrado.");
                return NotFound(error);
            }

            if (donationCreateDto.Total != 0)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O valor total não pode ser diferente de zero para doações presenciais.");
                return BadRequest(error);
            }

            if (donationCreateDto?.DonationItem.Count() > 0)
            {
                var lstItems = await ItemApplication.GetCharityItemIn(charity.Information.Id, donationCreateDto.DonationItem.Select(e => e.ItemId).ToList());

                if (lstItems.Count() != donationCreateDto.DonationItem.Count())
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"Existem itens inválidos.");
                    return BadRequest(error);
                }
            }
            else
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"É necessário pelo menos um item válido para doações presenciais.");
                return BadRequest(error);
            }

            var donationId = await DonationApplication.CreateDonation(donationCreateDto, false);

            try
            {             
                // Sent an email
                var donor = await UserApplication.GetUserOwnerDto(donationCreateDto.UserId);
                string message = string.Format(" <h2>Olá, {0}</h2><h3>Parabéns! Você realizou uma doação presencial para a entidade {1} ({2}).</h3>", donor.Name, charity.Name, charity.Cnpj);

                await this.EmailSender.SendEmailAsync(user.Login, $"Doa Sonhos - Doação Presencial", message);
            }
            catch (Exception){}

            return CreatedAtRoute("GetDonationById", new { id = donationId }, null);
        }    

    }
}
