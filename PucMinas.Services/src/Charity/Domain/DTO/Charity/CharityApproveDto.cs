using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityApproveDto
    {
        [JsonProperty("message")]
        [Required(ErrorMessage = "The field message cannot be null or empty", AllowEmptyStrings = false)]
        public string Message { get; set; }
        [JsonProperty("details")]
        public string Detail { get; set; }
        [JsonProperty("status")]
        [Required(ErrorMessage = "The field status cannot be null or empty", AllowEmptyStrings = false)]
        public string Status { get; set; }
        [JsonProperty("approver_name")]
        [Required(ErrorMessage = "The field approver_name cannot be null or empty", AllowEmptyStrings = false)]
        public string ApproverName { get; set; }
    }
}
