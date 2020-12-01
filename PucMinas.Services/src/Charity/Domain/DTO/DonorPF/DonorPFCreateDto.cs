using Newtonsoft.Json;
using PucMinas.Services.Charity.Filters;
using System;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.DonorPF
{
    public class DonorPFCreateDto
    {
        [JsonProperty("name")]
        [Required(ErrorMessage = "The field name cannot be null or empty", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [JsonProperty("cpf")]
        [Required(ErrorMessage = "The field cpf cannot be null or empty", AllowEmptyStrings = false)]
        public string CPF { get; set; }

        [JsonProperty("birthday")]
        [Required(ErrorMessage = "The field birthday cannot be null or empty")]
        [MinDateValidator(ErrorMessage = "The birthday date is invalid")]
        public DateTime Birthday { get; set; }

        [JsonProperty("genre")]
        [Required(ErrorMessage = "The field genre cannot be null or empty", AllowEmptyStrings = false)]
        public string Genre { get; set; }

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
