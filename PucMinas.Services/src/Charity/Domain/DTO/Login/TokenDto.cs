using Newtonsoft.Json;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.Login
{
    public class TokenDto
    {
        public TokenDto(string token, DateTime issuedAt, DateTime expires)
        {
            this.Token = token;
            this.IssuedAt = issuedAt;
            this.Expires = expires;
        }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("created")]
        public DateTime IssuedAt { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }
    }
}
