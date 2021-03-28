using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Models.Commmon;
using PucMinas.Services.Charity.Domain.Models.Login;
using System;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.Models.Donor
{
    public class Donation : Entity
    {
        public DateTime Date { get; set; }
        public double Total { get; set; }

        public IEnumerable<DonationItem> DonationItem { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public bool Completed { get; set; }
        public bool Canceled { get; set; }

        public Guid CharitableEntityId { get; set; }
        public CharitableEntity CharitableEntity { get; set; }
    }
}
