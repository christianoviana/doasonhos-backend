using Newtonsoft.Json;
using PucMinas.Services.Charity.Domain.DTO.Role;
using System;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.DTO.User
{
    public class UserResponseDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("login")]    
        public string Login { get; set; }

        [JsonProperty("active")]
        public bool IsActive { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
        
        [JsonProperty("roles")]
        public List<RoleDto> Roles { get; set; }
    }
}
