using System;

namespace PucMinas.Services.Charity.Domain.Models.Donor
{
    public class DonationItem
    {
        public Guid DonationId { get; set; }
        public Donation Donation { get; set; }

        public Guid ItemId { get; set; }
        public Item Item { get; set; }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
