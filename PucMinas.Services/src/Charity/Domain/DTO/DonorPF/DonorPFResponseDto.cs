using Newtonsoft.Json;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.DonorPF
{
    public class DonorPFResponseDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]        
        public string Name { get; set; }

        [JsonProperty("cpf")]
        public string CPF { get; set; }

        [JsonProperty("birthday")]     
        public DateTime Birthday { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }
}
