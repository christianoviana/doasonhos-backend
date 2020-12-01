using Newtonsoft.Json;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.Group
{
    public class GroupResponseDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
