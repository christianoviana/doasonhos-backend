using Newtonsoft.Json;

namespace PucMinas.Services.Charity.Domain.DTO.Donation
{
    public class DonationItemResponseDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
