using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityInfoImageUpdateDto
    {
        [BindProperty(Name = "image_name")]
        [Required(ErrorMessage = "The field image_name cannot be null or empty", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [BindProperty(Name = "image_file")]
        public IFormFile Photo { get; set; }
    }
}
