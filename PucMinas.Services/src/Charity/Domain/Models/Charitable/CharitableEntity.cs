 using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Models.Approvals;
using PucMinas.Services.Charity.Domain.Models.Commmon;
using PucMinas.Services.Charity.Domain.Models.Donor;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.Models.Charitable
{
    public class CharitableEntity : Entity
    {
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public string RepresentantName { get; set; }
        public ContactNumber ContactNumber { get; set; }
        public bool IsActive { get; set; }
        public ApproverStatus Status { get; set; }
        public string Approver { get; set; }
        public DateTime? ApproverData { get; set; }

        public Address Address { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public IEnumerable<Donation> Donations { get; set; }
        public CharitableInformation CharitableInformation { get; set; }
        public IEnumerable<Approval> Approvals { get; set; }
    }
}
