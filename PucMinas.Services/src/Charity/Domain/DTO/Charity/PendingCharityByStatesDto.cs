using Newtonsoft.Json;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class PendingCharityByStatesDto
    {
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("charities")]
        public IEnumerable<CharityResponseDto> charities { get; set; }
    }
}
