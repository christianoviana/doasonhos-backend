using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Models.Commmon;
using System;

namespace PucMinas.Services.Charity.Domain.Models.Approvals
{
    public class Approval:Entity
    {     
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public int Status { get; set; }

        public Guid CharitableEntityId { get; set; }
        public CharitableEntity CharitableEntity { get; set; }
    }
}
