using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Login
{
    public class LoginDto
    {
        [JsonProperty("login")]
        [Required(ErrorMessage = "The field login cannot be null or empty", AllowEmptyStrings = false)]      
        public string Login { get; set; }

        [JsonProperty("password")]
        [Required(ErrorMessage = "The field password cannot be null or empty", AllowEmptyStrings = false)]
        public string Password { get; set; }      
    }
}
