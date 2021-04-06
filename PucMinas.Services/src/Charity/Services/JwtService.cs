using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PucMinas.Services.Charity.Configuration;
using PucMinas.Services.Charity.Domain.DTO.Login;
using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Interfaces;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Extensions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PucMinas.Services.Charity.Services
{
    public class JwtService : IJwtService
    {
        private JwtSettings JwtSettings { get; set; }

        public JwtService(IOptionsMonitor<JwtSettings> JwtSettings)
        {
            this.JwtSettings = JwtSettings.CurrentValue;
        }

        public TokenDto CreateToken(User user)
        {
            // Header + Payload (Claims) + Signature
            List<Claim> lstClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Login.ToString()),
                new Claim("user_type", user.Type.ToString()),
            };

            switch (user.Type)
            {
                case LoginType.DONOR_PF:
                    lstClaims.Add(new Claim("owner_name", user.DonorPF.Name));
                    lstClaims.Add(new Claim("owner_id", user.DonorPF.Id.ToString()));
                    break;
                case LoginType.DONOR_PJ:
                    lstClaims.Add(new Claim("owner_name", user.DonorPJ.CompanyName));
                    lstClaims.Add(new Claim("owner_id", user.DonorPJ.Id.ToString()));
                    break;
                case LoginType.CHARITABLE_ENTITY:
                    lstClaims.Add(new Claim("owner_name", user.CharitableEntity.Name));
                    lstClaims.Add(new Claim("owner_id", user.CharitableEntity.Id.ToString()));
                    break;
            }

            if (user.UserRoles != null && user.UserRoles.Count() > 0)
            {
                string roles = string.Empty;

                foreach (var userRole in user.UserRoles)
                {
                    roles += userRole.Role.Name.ToLower() + ",";
                }
                roles = roles.Remove(roles.LastIndexOf(',')).Trim();

                lstClaims.Add(new Claim(ClaimTypes.Role, roles));                          
            }         

            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key)), SecurityAlgorithms.HmacSha256);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = JwtSettings.Issuer,
                Audience = JwtSettings.Audience,
                Subject = new ClaimsIdentity(lstClaims),
                SigningCredentials = credentials,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(JwtSettings.ValidFor)
            };

            var securityToken = jwtTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtTokenHandler.WriteToken(securityToken);

            return new TokenDto(token, securityToken.ValidFrom.ToLocalTime(), securityToken.ValidTo.ToLocalTime());
        }
    }
}
