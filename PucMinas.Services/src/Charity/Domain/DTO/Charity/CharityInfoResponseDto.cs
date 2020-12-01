using Newtonsoft.Json;
using System;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityInfoResponseDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        [JsonProperty("about")]
        public string About { get; set; }
        [JsonProperty("goal")]
        public string Goal { get; set; }

        [JsonProperty("manager_description")]
        public string ManagerDescription { get; set; }
        [JsonProperty("transparency_description")]
        public string TransparencyDescription { get; set; }

        [JsonProperty("vision")]
        public string Vision { get; set; }
        [JsonProperty("mission")]
        public string Mission { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        
        [JsonProperty("picture_url")]
        public string PictureUrl { get; set; }

        [JsonProperty("image01_url")]
        public string Photo01 { get; set; }
        [JsonProperty("title_image02")]
        public string TitlePhoto01 { get; set; }

        [JsonProperty("image02_url")]
        public string Photo02 { get; set; }
        [JsonProperty("title_image01")]
        public string TitlePhoto02 { get; set; }

        [JsonProperty("site_url")]
        public string SiteUrl { get; set; }
    }
}
