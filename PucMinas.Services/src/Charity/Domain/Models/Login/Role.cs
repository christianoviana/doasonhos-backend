using PucMinas.Services.Charity.Domain.Models.Commmon;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.Models.Login
{
    public class Role : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
