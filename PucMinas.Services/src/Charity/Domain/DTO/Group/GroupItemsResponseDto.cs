using Newtonsoft.Json;
using PucMinas.Services.Charity.Domain.DTO.Item;
using System;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.DTO.Group
{
    public class GroupItemsResponseDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("items")]
        public IEnumerable<ItemDto> Items { get; set; }
    }
}
