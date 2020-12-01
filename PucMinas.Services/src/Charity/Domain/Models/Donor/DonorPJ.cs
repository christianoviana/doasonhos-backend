using PucMinas.Services.Charity.Domain.Models.Commmon;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Domain.ValueObject;
using System;

namespace PucMinas.Services.Charity.Domain.Models.Donor
{
    public class DonorPJ : Entity
    {
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string CNPJ { get; set; }
        public Address Address { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
