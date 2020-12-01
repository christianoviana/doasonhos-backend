using Newtonsoft.Json;
using PucMinas.Services.Charity.Domain.DTO.Item;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityInfoItemResponseDto
    {
        [JsonProperty("items")]
        public List<ItemResponseDto> items { get; set; }
    }
}
