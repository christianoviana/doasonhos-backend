using Newtonsoft.Json;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityStatusResponseDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("approver")]
        public string Approver { get; set; }

        [JsonProperty("approver_data")]
        public DateTime? ApproverData { get; set; }

        [JsonProperty("charity_information")]
        public bool HasCharityInformation { get; set; }
    }
}
