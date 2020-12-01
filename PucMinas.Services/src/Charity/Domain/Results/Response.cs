using Newtonsoft.Json;

namespace PucMinas.Services.Charity.Domain.Results
{
    public class Response<T>
    {
        [JsonProperty("data", Order = 1)]
        public T Data { get; set; }
    }
}
