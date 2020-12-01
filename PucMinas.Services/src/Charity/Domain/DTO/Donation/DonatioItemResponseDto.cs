using Newtonsoft.Json;

namespace PucMinas.Services.Charity.Domain.DTO.Donation
{
    public class DonatioItemResponseDto
    {
        [JsonProperty("item")]
        public DonationItemDto DonationItemDto { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
