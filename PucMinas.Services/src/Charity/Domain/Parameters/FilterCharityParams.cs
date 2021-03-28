using Newtonsoft.Json;

namespace PucMinas.Services.Charity.Domain.Parameters
{
    public class FilterCharityParams : FilterParams
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        public FilterCharityParams()
        {
            this.Term = string.Empty;
            this.State = string.Empty;
            this.City = string.Empty;
        }
    }
}
