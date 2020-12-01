using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.User
{
    public class UserRoleUpdateDto
    {
        [JsonProperty("roles")]
        [Required(ErrorMessage = "The field roles cannot be null")]
        public List<Guid> Roles { get; set; }
    }
}
