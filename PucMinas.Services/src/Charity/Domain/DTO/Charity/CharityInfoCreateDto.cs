using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PucMinas.Services.Charity.Domain.DTO.Charity
{
    public class CharityInfoCreateDto
    {
        [BindProperty(Name = "nickname")]
        public string Nickname { get; set; }
        [BindProperty(Name = "about")]
        [Required(ErrorMessage = "The field about cannot be null or empty", AllowEmptyStrings = false)]
        public string About { get; set; }
        [BindProperty(Name = "goal")]
        [Required(ErrorMessage = "The field goal cannot be null or empty", AllowEmptyStrings = false)]
        public string Goal { get; set; }

        [BindProperty(Name = "manager_description")]
        public string ManagerDescription { get; set; }
        [BindProperty(Name = "transparency_description")]
        public string TransparencyDescription { get; set; }

        [BindProperty(Name = "vision")]
        [Required(ErrorMessage = "The field vision cannot be null or empty", AllowEmptyStrings = false)]
        public string Vision { get; set; }
        [BindProperty(Name = "mission")]
        [Required(ErrorMessage = "The field mission cannot be null or empty", AllowEmptyStrings = false)]
        public string Mission { get; set; }
        [BindProperty(Name = "value")]
        [Required(ErrorMessage = "The field value cannot be null or empty", AllowEmptyStrings = false)]
        public string Value { get; set; }

        [BindProperty(Name = "picture")]
        public IFormFile Picture { get; set; }

        [BindProperty(Name = "image01")]
        public IFormFile Photo01 { get; set; }
        [BindProperty(Name = "title_image01")]
        public string TitlePhoto01 { get; set; }

        [BindProperty(Name = "image02")]
        public IFormFile Photo02 { get; set; }
        [BindProperty(Name = "title_image02")]
        public string TitlePhoto02 { get; set; }

        [BindProperty(Name = "site")]
        public string SiteUrl { get; set; }
    }
}
