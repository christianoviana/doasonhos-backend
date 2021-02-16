using System;

namespace PucMinas.Services.Charity.Domain.Models.Donor
{
    public class DonationItem
    {
        public Guid DonationId { get; set; }
        public Donation Donation { get; set; }

        public Guid ItemId { get; set; }
        public Item Item { get; set; }

        public double Quantity { get; set; }
        //public double Valor { get; set; }
    }
}
