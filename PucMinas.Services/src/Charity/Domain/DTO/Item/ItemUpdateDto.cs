using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Item
{
    public class ItemUpdateDto
    {
        [Required(ErrorMessage = "The field name cannot be null or empty", AllowEmptyStrings = false)]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field description cannot be null or empty", AllowEmptyStrings = false)]
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("activated")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "The field group_id cannot be null or empty")]
        [JsonProperty("group_id")]
        public Guid GroupId { get; set; }
    }
}
