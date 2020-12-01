using PucMinas.Services.Charity.Domain.Models.Commmon;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Domain.ValueObject;
using System;

namespace PucMinas.Services.Charity.Domain.Models.Donor
{
    public class DonorPF : Entity
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public DateTime Birthday { get; set; }
        public string Genre { get; set; }
        public Address Address { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
