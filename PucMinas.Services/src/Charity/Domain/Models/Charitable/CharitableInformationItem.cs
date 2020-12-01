using PucMinas.Services.Charity.Domain.Models.Donor;
using System;

namespace PucMinas.Services.Charity.Domain.Models.Charitable
{
    public class CharitableInformationItem
    {
        public Guid CharitableInformationId { get; set; }
        public CharitableInformation CharitableInformation { get; set; }

        public Guid ItemId { get; set; }
        public Item Item { get; set; }
    }
}
