using Newtonsoft.Json;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.Donation
{
    public class DonationitemRequestDto
    {
        [JsonProperty("item_id")]
        public Guid ItemId { get; set; }
               
        [JsonProperty("item_quantity")]
        public int ItemQuantity { get; set; }
    }
}
