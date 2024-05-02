using API_livechat.DTO;
using API_livechat.Models;
using API_livechat.Services;
using API_livechat.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_livechat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        #region service
        private readonly UserlService _service;

        public AuthController(UserlService service)
        {
            _service = service;
        }
        #endregion

        private object? CreateToken(UserlDTO userDTO)
        {
            List<Claim> claimsList = new List<Claim>()
                {
                new Claim(JwtRegisteredClaimNames.Sub, userDTO.User),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            //Creazione dell'algoritmo di cifratura e stabilire la chiave
            var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("supersecret_key_supersecret_key_supersecret_key")
                    );
            //Creazione credenziali di cifratura
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenJwt = new JwtSecurityToken(
                issuer: "mjserver",
                audience: "Users",
                claims: claimsList,
                expires: DateTime.Now.AddHours(1),  //scadenza del token per il login ogni ora
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenJwt);

            return token;
        }

        [HttpPost("login")]
        public IActionResult LoginProcedure(UserlDTO userDTO)
        {
            if (_service.CheckUser(userDTO))
            {
                return Ok(CreateToken(userDTO));
            }
            
            return Unauthorized();
        }


    }
}
