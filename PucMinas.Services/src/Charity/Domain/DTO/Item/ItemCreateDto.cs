using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Item
{
    public class ItemCreateDto
    {
        [Required(ErrorMessage = "The field name cannot be null or empty", AllowEmptyStrings = false)]
        [BindProperty(Name = "name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field description cannot be null or empty", AllowEmptyStrings = false)]
        [BindProperty(Name = "description")]
        public string Description { get; set; }

        [BindProperty(Name = "price")]
        public string Price { get; set; }

        [BindProperty(Name = "photo")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "The field group_id cannot be null or empty")]
        [BindProperty(Name ="group_id")]
        public Guid GroupId { get; set; }
    }
}
