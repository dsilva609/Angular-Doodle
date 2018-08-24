using Angular_Doodle.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Angular_Doodle.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[Route("Login")]
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginModel login)
		{
			var signInSuccessful = await _signInManager.PasswordSignInAsync(login.Username, login.Password, true, false);
			if (signInSuccessful.Succeeded)
			{
				var user = await _userManager.FindByEmailAsync(login.Username);
				var token = BuildToken(user);

				return Ok(new { token = token });
			}

			return Unauthorized();
		}

		private string BuildToken(ApplicationUser user)
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
					new Claim(JwtRegisteredClaimNames.Sub, user.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				}),
				Expires = DateTime.Now.AddMinutes(5),
				NotBefore = DateTime.Now
			});

			return handler.WriteToken(token);
		}
	}
}