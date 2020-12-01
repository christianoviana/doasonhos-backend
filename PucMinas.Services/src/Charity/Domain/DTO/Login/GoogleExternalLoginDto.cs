using Newtonsoft.Json;

namespace PucMinas.Services.Charity.Domain.DTO.Login
{
    public class GoogleExternalLoginDto
    {
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
    }
}
