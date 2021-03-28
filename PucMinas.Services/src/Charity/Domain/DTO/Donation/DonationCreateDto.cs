using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Donation
{
    public class DonationCreateDto
    {
        [JsonProperty("total")]
        [Required(ErrorMessage = "The field total cannot be null or empty", AllowEmptyStrings = false)]
        public double Total { get; set; }
        
        [JsonProperty("items")]
        public IEnumerable<DonationitemRequestDto> DonationItem { get; set; }

        [JsonProperty("donor_id")]
        [Required(ErrorMessage = "The field donor_id cannot be null or empty", AllowEmptyStrings = false)]
        public Guid UserId { get; set; }

        [JsonProperty("charitable_entity_id")]
        [Required(ErrorMessage = "The field charitable_entity_id cannot be null or empty", AllowEmptyStrings = false)]
        public Guid CharitableEntityId { get; set; }
    }
}
