using Newtonsoft.Json;
using PucMinas.Services.Charity.Domain.DTO.Charity;
using System;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.DTO.Donation
{
    public class DonationResponseDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("total")]
        public double Total { get; set; }
        [JsonProperty("completed")]
        public bool Completed { get; set; }

        [JsonProperty("canceled")]
        public bool Canceled { get; set; }

        [JsonProperty("items")]
        public IEnumerable<DonationItemResponseDto> DonationItem { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("charitable_entity")]
        public CharityResponseDto CharitableEntity { get; set; }
    }
}
