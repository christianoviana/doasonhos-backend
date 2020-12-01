using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityUpdateDto
    {
        [JsonProperty("name")]
        [Required(ErrorMessage = "The field name cannot be null or empty", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [JsonProperty("cnpj")]
        [Required(ErrorMessage = "The field cnpj cannot be null or empty", AllowEmptyStrings = false)]
        public string Cnpj { get; set; }

        [JsonProperty("representant_name")]
        [Required(ErrorMessage = "The field representant_name cannot be null or empty", AllowEmptyStrings = false)]
        public string RepresentantName { get; set; }

        [JsonProperty("cellphone")]
        [Required(ErrorMessage = "The field cellphone cannot be null or empty", AllowEmptyStrings = false)]
        public string CellPhone { get; set; }

        [JsonProperty("telephone")]
        [Required(ErrorMessage = "The field telephone cannot be null or empty", AllowEmptyStrings = false)]
        public string Telephone { get; set; }
                           
        [JsonProperty("address")]
        [Required(ErrorMessage = "The field address cannot be null")]
        public AddressDto Address { get; set; }        
    }
}
