using Newtonsoft.Json;
using PucMinas.Services.Charity.Filters;
using System;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.DonorPF
{
    public class DonorPFUpdateDto
    {
        [JsonProperty("name")]
        [Required(ErrorMessage = "The field name cannot be null or empty", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [JsonProperty("cpf")]
        [Required(ErrorMessage = "The field cpf cannot be null or empty", AllowEmptyStrings = false)]
        public string CPF { get; set; }

        [JsonProperty("birthday")]
        [MinDateValidator(ErrorMessage = "The birthday date is invalid")]
        [Required(ErrorMessage = "The field birthday cannot be null or empty")]
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
    }
}
