using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.DonorPJ
{
    public class DonorPJCreateDto
    {
        [JsonProperty("company_name")]
        [Required(ErrorMessage = "The field company_name cannot be null or empty", AllowEmptyStrings = false)]
        public string CompanyName { get; set; }

        [JsonProperty("contact_name")]
        [Required(ErrorMessage = "The field contact_name cannot be null or empty", AllowEmptyStrings = false)]
        public string ContactName { get; set; }

        [JsonProperty("cnpj")]
        [Required(ErrorMessage = "The field cnpj cannot be null or empty", AllowEmptyStrings = false)]
        public string CNPJ { get; set; }

        [JsonProperty("city")]
        [Required(ErrorMessage = "The field city cannot be null or empty", AllowEmptyStrings = false)]
        public string City { get; set; }

        [JsonProperty("state")]
        [Required(ErrorMessage = "The field state cannot be null or empty", AllowEmptyStrings = false)]
        public string State { get; set; }

        [JsonProperty("country")]
        [Required(ErrorMessage = "The field country cannot be null or empty", AllowEmptyStrings = false)]
        public string Country { get; set; }

        [JsonProperty("login")]
        [Required(ErrorMessage = "The field login cannot be null or empty", AllowEmptyStrings = false)]
        public string Login { get; set; }

        [JsonProperty("password")]
        [Required(ErrorMessage = "The field password cannot be null or empty", AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
