using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class AddressDto
    {
        [JsonProperty("city")]
        [Required(ErrorMessage = "The field city cannot be null or empty", AllowEmptyStrings = false)]
        public string City { get; set; }

        [JsonProperty("state")]
        [Required(ErrorMessage = "The field state cannot be null or empty", AllowEmptyStrings = false)]
        public string State { get; set; }

        [JsonProperty("country")]
        [Required(ErrorMessage = "The field country cannot be null or empty", AllowEmptyStrings = false)]
        public string Country { get; set; }

        [JsonProperty("cep")]
        [Required(ErrorMessage = "The field cep cannot be null or empty", AllowEmptyStrings = false)]
        public string CEP { get; set; }

        [JsonProperty("address_name")]
        [Required(ErrorMessage = "The field address_name cannot be null or empty", AllowEmptyStrings = false)]
        public string AddressName { get; set; }

        [JsonProperty("district")]
        [Required(ErrorMessage = "The field district cannot be null or empty", AllowEmptyStrings = false)]
        public string District { get; set; }

        [JsonProperty("number")]
        [Required(ErrorMessage = "The field number cannot be null or empty", AllowEmptyStrings = false)]
        public string Number { get; set; }

        [JsonProperty("complement")]
        public string Complement { get; set; }
    }
}
