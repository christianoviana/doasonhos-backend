using PucMinas.Services.Charity.Domain.Models.Commmon;
using PucMinas.Services.Charity.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.Models.Charitable
{
    public class CharitableInformation : Entity
    {
        public string Nickname { get; set; }
        public string About { get; set; }
        public string Goal { get; set; }

        public string ManagerDescription { get; set; }
        public string TransparencyDescription { get; set; }

        public string Vision { get; set; }
        public string Mission { get; set; }
        public string Value { get; set; }

        public string PicturePath { get; set; }
        public string PictureUrl { get; set; }
        
        public string SiteUrl { get; set; }
        public string Email { get; set; }

        public Image Photo01 { get; set; }
        public Image Photo02 { get; set; }

        public IEnumerable<CharitableInformationItem> CharitableInformationItem { get; set; }

        public Guid CharitableEntityId { get; set; }
        public CharitableEntity CharitableEntity { get; set; }
    }
}
