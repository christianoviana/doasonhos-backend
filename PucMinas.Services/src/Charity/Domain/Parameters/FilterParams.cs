using Newtonsoft.Json;

namespace PucMinas.Services.Charity.Domain.Parameters
{
    public class FilterParams
    {
        [JsonProperty("term")]
        public string Term { get; set; }

        public FilterParams()
        {
            this.Term = string.Empty;
        }
    }
}
