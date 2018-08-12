using Angular_Doodle.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Angular_Doodle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [Route("CreateToken")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateToken(LoginModel login)
        {
            if (login.Username == "Admin" && login.Password == "Pass")
            {
                var token = BuildToken();

                return Ok(new { token = token });
            }

            return Unauthorized();
        }

        private string BuildToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdfasdfasdfsadf"));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "https://localhost:44388",
                Audience = "https://localhost:44388",
                SigningCredentials = cred,
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "user@gmail.com"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.Now.AddMinutes(5),
                NotBefore = DateTime.Now
            });

            return handler.WriteToken(token);
        }
    }
}