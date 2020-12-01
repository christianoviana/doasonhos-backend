using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Models.Commmon;
using System;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.Models.Donor
{
    public class Item : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public string ImageUrl { get; set; }

        public IEnumerable<DonationItem> DonationItem { get; set; }
        public IEnumerable<CharitableInformationItem> CharitableInformationItem { get; set; }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }
    }
}
