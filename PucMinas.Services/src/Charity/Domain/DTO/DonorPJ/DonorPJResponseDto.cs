using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.DonorPJ
{
    public class DonorPJResponseDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("company_name")]
        [Required(ErrorMessage = "The field company_name cannot be null or empty", AllowEmptyStrings = false)]
        public string CompanyName { get; set; }

        [JsonProperty("contact_name")]
        [Required(ErrorMessage = "The field contac_name cannot be null or empty", AllowEmptyStrings = false)]
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

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }
}
