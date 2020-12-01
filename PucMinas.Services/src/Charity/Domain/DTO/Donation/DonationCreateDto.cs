using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.DTO.Donation
{
    public class DonationCreateDto
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("total")]
        public double Total { get; set; }
        [JsonProperty("completed")]
        public bool Completed { get; set; }

        [JsonProperty("items")]
        public IEnumerable<DonationitemRequestDto> DonationItem { get; set; }

        [JsonProperty("donor_id")]
        public string UserId { get; set; }
        [JsonProperty("donor_name")]
        public string UserName { get; set; }
        [JsonProperty("donor_login")]
        public string UserLogin { get; set; }

        [JsonProperty("charitable_entity_id")]
        public Guid CharitableEntityId { get; set; }
    }
}
