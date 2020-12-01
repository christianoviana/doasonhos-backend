using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.User
{
    public class UserCreateDto
    {
        [JsonProperty("login")]
        [Required(ErrorMessage = "The field login cannot be null or empty", AllowEmptyStrings = false)]
        public string Login { get; set; }

        [JsonProperty("password")]
        [Required(ErrorMessage = "The field password cannot be null or empty", AllowEmptyStrings = false)]
        public string Password { get; set; }

        [JsonProperty("type")]
        [Required(ErrorMessage = "The field type cannot be null or empty")]
        public string type { get; set; }

        [JsonProperty("roles")]
        public List<Guid> Roles { get; set; }
    }
}
