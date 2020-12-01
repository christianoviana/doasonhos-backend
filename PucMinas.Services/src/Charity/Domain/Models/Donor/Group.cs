using PucMinas.Services.Charity.Domain.Models.Commmon;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.Models.Donor
{
    public class Group : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<Item> Items { get; set; }
    }
}
