using Newtonsoft.Json;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.User
{
    public class UserOwnerDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }
}
