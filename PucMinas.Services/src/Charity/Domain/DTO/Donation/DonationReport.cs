using Newtonsoft.Json;
using PucMinas.Services.Charity.Domain.DTO.Charity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Domain.DTO.Donation
{
    public class DonationCharitiesReportDto
    {
        [JsonProperty("total_donations_value")]
        public double TotalDonations { get; set; }

        [JsonProperty("items_value")]
        public double TotalItems { get; set; }

        [JsonProperty("donations_value")]
        public double TotalDonationsWithoutItems { get; set; }

        [JsonProperty("total_online_items")]
        public int OnlineItemsQuantity { get; set; }

        [JsonProperty("total_presential_items")]
        public int PresentialItemsQuantity { get; set; }

        [JsonProperty("online_items")]
        public IEnumerable<DonationItemReportDto> OnlineItems { get; set; }
        [JsonProperty("presential_items")]
        public IEnumerable<DonationItemReportDto> PresentialItems { get; set; }

    }

    public class DonationUsersReportDto
    {
        [JsonProperty("number_donors_pf")]
        public int NumberDonorsPF { get; set; }

        [JsonProperty("number_donors_pj")]
        public int NumberDonorsPJ { get; set; }

        [JsonProperty("number_external")]
        public int NumberExternal { get; set; }

        [JsonProperty("number_charities")]
        public int NumberCharities { get; set; }

        [JsonProperty("number_inactives")]
        public int NumberInactives { get; set; }
    }

    public class DonationItemReportDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}