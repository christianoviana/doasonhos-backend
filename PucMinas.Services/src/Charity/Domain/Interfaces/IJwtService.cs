using PucMinas.Services.Charity.Domain.DTO.Login;
using PucMinas.Services.Charity.Domain.Models.Login;

namespace PucMinas.Services.Charity.Domain.Interfaces
{
    public interface IJwtService
    {
        TokenDto CreateToken(User user);
    }
}
