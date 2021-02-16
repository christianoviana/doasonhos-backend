using Newtonsoft.Json;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.Approval
{
    public class ApprovalResponseDto
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("detail")]
        public string Detail { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
