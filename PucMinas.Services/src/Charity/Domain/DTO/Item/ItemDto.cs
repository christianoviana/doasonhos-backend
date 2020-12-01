using Newtonsoft.Json;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.Item
{
    public class ItemDto
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

    }
}
