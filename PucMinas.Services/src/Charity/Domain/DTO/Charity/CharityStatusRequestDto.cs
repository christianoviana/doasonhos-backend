using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityStatusRequestDto
    {
        [JsonProperty("active")]
        [Required(ErrorMessage = "The field active cannot be null or empty", AllowEmptyStrings = false)]
        public bool Active { get; set; }          
    }
}
