using Newtonsoft.Json;
using PucMinas.Services.Charity.Domain.DTO.Group;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.Item
{
    public class ItemResponseDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("image_url")]
        public string ImageURL { get; set; }

        [JsonProperty("group")]
        public GroupResponseDto Group { get; set; }
    }
}
