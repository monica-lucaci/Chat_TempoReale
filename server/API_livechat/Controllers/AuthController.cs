using System;
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
        private readonly UserService _service;

        public AuthController(UserService service)
        {
            _service = service;
        }
        #endregion

        private object? CreateToken(UserLogin usL)
        {
            List<Claim> claimsList = new List<Claim>()
                {
                new Claim(JwtRegisteredClaimNames.Sub, usL.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserRole", "USER"),
                new Claim("Username", usL.Username)
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
        public IActionResult LoginProcedure(UserLoginDTO userLDTO)
        {
            if (_service.CheckUserLog(userLDTO))
            {
                UserLogin usL = _service.ConvertToUserLogin(userLDTO);
                return Ok(CreateToken(usL));
            }

            return Unauthorized();
        }


    }
}
