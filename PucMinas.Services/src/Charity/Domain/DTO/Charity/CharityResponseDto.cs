using Newtonsoft.Json;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityResponseDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        [JsonProperty("representant_name")]
        public string RepresentantName { get; set; }

        [JsonProperty("cellphone")]
        public string CellPhone { get; set; }

        [JsonProperty("telephone")]
        public string Telephone { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("approver")]
        public string Approver { get; set; }

        [JsonProperty("approver_data")]
        public DateTime? ApproverData { get; set; }

        [JsonProperty("address")]
        public AddressDto Address { get; set; }

        [JsonProperty("information")]
        public CharityInfoResponseDto Information { get; set; }
    }
}
