using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Domain.DTO.Donation;
using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Results.Exceptions;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private DonationApplication DonationApplication { get; set; }
        private UserApplication UserApplication { get; set; }


        public ReportsController(DonationApplication donationApplication,
                                 UserApplication userApplication)
        {
            this.DonationApplication = donationApplication;
            this.UserApplication = userApplication;
        }

        // GET api/<controller>
        [Authorize("GetUserReport")]
        [HttpGet("users", Name = "GetUserReport")]
        public async Task<ActionResult<DonationUsersReportDto>> GetUserReport(Guid id, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {           
            var numberDonorsPF = await this.UserApplication.GetCountAllUsers(u => u.Type == LoginType.DONOR_PF && u.IsActive == true);
            var numberDonorsPJ = await this.UserApplication.GetCountAllUsers(u => u.Type == LoginType.DONOR_PJ && u.IsActive == true);
            var numberExternal = await this.UserApplication.GetCountAllUsers(u => u.Type == LoginType.EXTERNAL && u.IsActive == true);
            var numberCharities = await this.UserApplication.GetCountAllUsers(u => u.Type == LoginType.CHARITABLE_ENTITY && u.IsActive == true);
            var numberInactives = await this.UserApplication.GetCountAllUsers(u => u.IsActive == false);

            DonationUsersReportDto donationUsersReportDto = new DonationUsersReportDto()
            {
                NumberCharities = numberCharities,
                NumberDonorsPF = numberDonorsPF,
                NumberDonorsPJ = numberDonorsPJ,
                NumberExternal = numberExternal,
                NumberInactives = numberInactives
            };

            return Ok(donationUsersReportDto);
        }

        // GET api/<controller>
        [Authorize("GetCharityReport")]
        [HttpGet("charities/{id}", Name = "GetCharityReport")]
        public async Task<ActionResult<DonationCharitiesReportDto>> GetCharityReport(Guid id, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from == null || from.Date.Equals(DateTime.MinValue))
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A data de início deve ser informada.");
                return BadRequest(error);
            }

            if (to != null && from.Date > to.Date)
            {
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"A data de início não pode ser maior do que a data final.");
                return BadRequest(error);
            }        

            if (to == null || to.Date.Equals(DateTime.MinValue))
            {
                to = DateTime.Today;
            }
            
            var user_type = this.User.Claims.Where(c => c.Type == "user_type").Select(c => c.Value).FirstOrDefault();

            if (user_type.ToUpper() == "CHARITABLE_ENTITY")
            {
                var ownerId = this.User.Claims.Where(c => c.Type == "owner_id").Select(c => c.Value).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(ownerId) || !ownerId.Equals(id.ToString()))
                {
                    ErrorMessage error = new ErrorMessage((int)HttpStatusCode.BadRequest, $"O id informado não pertence a endidade beneficente logada.");
                    return BadRequest(error);
                }
            }

            var filteredDonations = await this.DonationApplication.GetAllDonation(d => d.CharitableEntityId.Equals(id) && d.Completed == true && d.Date.Date >= from && d.Date.Date <= to);
            DonationCharitiesReportDto donationCharitiesReportDto = new DonationCharitiesReportDto();
            
            if (filteredDonations != null && filteredDonations.Count() > 0)
            {
                var allDonations = filteredDonations.Select(d => d.Total);
                var onlineItems = filteredDonations.Where(d => d.Total > 0).Select(d => d);

                var presentialItems = filteredDonations.Where(d => d.Total == 0).Select(d => d);


                var totalDonations = allDonations.Sum();
                var totalItems = onlineItems.Where(d => d.DonationItem != null && d.DonationItem.Count() > 0).SelectMany(d => d.DonationItem).Select(i => (i.Price * i.Quantity)).Sum();
                var totalDonationsWithoutItems = onlineItems.Where(d => d.DonationItem == null || d.DonationItem.Count() == 0).Select(d => d.Total).Sum();

                var presentialItemsQuantity = presentialItems.SelectMany(d => d.DonationItem).Select(i => i.Quantity).Sum();
                var onlineItemsQuantity = onlineItems.Where(d => d.DonationItem != null && d.DonationItem.Count() > 0).SelectMany(d => d.DonationItem).Select(i => i.Quantity).Sum();

                donationCharitiesReportDto.TotalItems = totalItems;
                donationCharitiesReportDto.TotalDonations = totalDonations;
                donationCharitiesReportDto.TotalDonationsWithoutItems = totalDonationsWithoutItems;
                donationCharitiesReportDto.OnlineItemsQuantity = onlineItemsQuantity;
                donationCharitiesReportDto.PresentialItemsQuantity = presentialItemsQuantity;
                donationCharitiesReportDto.PresentialItems = presentialItems.SelectMany(d => d.DonationItem).Select(d => new DonationItemReportDto() { Name = d.Name, Quantity = d.Quantity }).GroupBy(d => d.Name).Select(d => new DonationItemReportDto(){ Name = d.Key, Quantity = d.Select(di => di.Quantity).Sum() }).OrderByDescending(d => d.Quantity).Take(10);
                donationCharitiesReportDto.OnlineItems = onlineItems.Where(d => d.DonationItem != null && d.DonationItem.Count() > 0).SelectMany(d => d.DonationItem).Select(d => new DonationItemReportDto() { Name = d.Name, Quantity = d.Quantity }).GroupBy(d => d.Name).Select(d => new DonationItemReportDto() { Name = d.Key, Quantity = d.Select(di => di.Quantity).Sum() }).OrderByDescending(d => d.Quantity).Take(10);                              
            }


            return Ok(donationCharitiesReportDto);
        }

    }
}
