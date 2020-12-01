using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityInfoUpdateDto
    {
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        [JsonProperty("about")]
        [Required(ErrorMessage = "The field about cannot be null or empty", AllowEmptyStrings = false)]
        public string About { get; set; }
        [JsonProperty("goal")]
        [Required(ErrorMessage = "The field goal cannot be null or empty", AllowEmptyStrings = false)]
        public string Goal { get; set; }

        [JsonProperty("manager_description")]
        public string ManagerDescription { get; set; }
        [JsonProperty("transparency_description")]
        public string TransparencyDescription { get; set; }

        [JsonProperty("vision")]
        [Required(ErrorMessage = "The field vision cannot be null or empty", AllowEmptyStrings = false)]
        public string Vision { get; set; }
        [JsonProperty("mission")]
        [Required(ErrorMessage = "The field mission cannot be null or empty", AllowEmptyStrings = false)]
        public string Mission { get; set; }
        [JsonProperty("value")]
        [Required(ErrorMessage = "The field value cannot be null or empty", AllowEmptyStrings = false)]
        public string Value { get; set; }

        [JsonProperty("site")]
        public string SiteUrl { get; set; }
    }
}
