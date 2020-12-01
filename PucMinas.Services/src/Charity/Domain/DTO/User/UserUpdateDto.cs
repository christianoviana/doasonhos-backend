using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.User
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "The field login cannot be null or empty", AllowEmptyStrings = false)]
        [JsonProperty("login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "The field active cannot be null")]
        [JsonProperty("active")]
        public bool IsActive { get; set; }
    }
}
