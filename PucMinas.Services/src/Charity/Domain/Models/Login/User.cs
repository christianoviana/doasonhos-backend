using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Models.Commmon;
using PucMinas.Services.Charity.Domain.Models.Donor;
using System;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.Models.Login
{
    public class User : Entity
    {
        public User()
        {
        }

        public User(Guid id, string login, string password, LoginType type, Guid activationCode, bool isActive)
        {
            Id = id;
            Login = login;
            Password = password;
            Type = type;
            ActivationCode = activationCode;
            IsActive = isActive;
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public LoginType Type { get; set; }
        public Guid ActivationCode { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
        
        public DonorPF DonorPF { get; set; }
        public DonorPJ DonorPJ { get; set; }
        public CharitableEntity CharitableEntity { get; set; }
        public IEnumerable<Donation> Donations { get; set; }
    }
}
