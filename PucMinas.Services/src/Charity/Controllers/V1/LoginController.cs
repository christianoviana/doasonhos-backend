using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Domain.DTO.Login;
using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Interfaces;
using PucMinas.Services.Charity.Domain.Results.Exceptions;
using PucMinas.Services.Charity.Extensions;
using System;
using System.Net;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace PucMinas.Services.Charity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private UserApplication UserApplication { get; set; }

        public IJwtService JwtService { get; set; }

        public LoginController(IJwtService JwtService,
                               UserApplication userApplication)
        {
            this.JwtService = JwtService;
            this.UserApplication = userApplication;
        }

        // POST api/<controller>/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<TokenDto>> Authenticate([FromBody]LoginDto userDto)
        {
            if (userDto == null || String.IsNullOrWhiteSpace(userDto.Login) || String.IsNullOrWhiteSpace(userDto.Password))
            {
                return BadRequest(new ErrorMessage((int)HttpStatusCode.BadRequest, "O usuário e a senha devem ser informados."));
            }

            var user = await UserApplication.GetUser(u => (u.Login.ToLower().Equals(userDto.Login.ToLower()) && u.Password.Equals(userDto.Password.ToSHA512()) && u.IsActive == true && u.Type != LoginType.EXTERNAL));
          
            if (user == null)
                return Unauthorized(new ErrorMessage((int) HttpStatusCode.Unauthorized, "Usuário ou senha inválidos."));
                      
            var token = JwtService.CreateToken(user);
                     
            return Ok(token);
        }

        [HttpPost("authenticate/google")]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TokenDto>> GoogleLogin([FromBody] GoogleExternalLoginDto userDto)
        {
            Payload payload;
            try
            {
                payload = await ValidateAsync(userDto.IdToken, new ValidationSettings
                {
                    Audience = new[] { "1055013042266-bgqj135lt6tq6vfinq1qev5ao7h1tkae.apps.googleusercontent.com" }
                });
                // It is important to add your ClientId as an audience in order to make sure
                // that the token is for your application!

                if (payload == null || string.IsNullOrWhiteSpace(payload.Email))
                {
                    return BadRequest(new ErrorMessage((int)HttpStatusCode.BadRequest, "As informações do usuário ou email são inválidas."));
                }

                var user = await UserApplication.GetUser(u => (u.Login.ToLower().Equals(payload.Email.ToLower())));
                
                if (user == null)
                {
                    user = await UserApplication.CreateExternalUser(payload.Email);
                }
                else if (!user.IsActive)
                {
                    return Unauthorized(new ErrorMessage((int)HttpStatusCode.Unauthorized, "O usuário está inativo. Por favor contate o administrador do sistema."));
                }
                              
                var token = JwtService.CreateToken(user);
                return Ok(token);
            }
            catch
            {
                // Invalid token
                return Unauthorized(new ErrorMessage((int)HttpStatusCode.Unauthorized, "O token de acesso é inválido."));
            }
        }
    }
}
