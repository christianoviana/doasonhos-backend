using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityInfoItemDto
    {
        [JsonProperty("items")]
        [Required(ErrorMessage = "The field items cannot be null")]
        public List<Guid> items { get; set; }
    }
}
