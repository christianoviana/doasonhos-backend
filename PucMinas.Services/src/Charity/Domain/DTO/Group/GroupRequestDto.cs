using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Group
{
    public class GroupRequestDto
    {
        [Required(ErrorMessage = "The field name cannot be null or empty", AllowEmptyStrings = false)]
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
