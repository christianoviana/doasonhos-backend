using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Role
{
    public class RoleDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "The field name cannot be null or empty", AllowEmptyStrings = false)]
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
