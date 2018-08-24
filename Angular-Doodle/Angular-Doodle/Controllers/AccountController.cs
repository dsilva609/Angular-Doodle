using Angular_Doodle.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Angular_Doodle.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[Route("Register")]
		public async Task<IActionResult> Register(RegisterModel registration)
		{
			if ((string.IsNullOrWhiteSpace(registration.Password) || string.IsNullOrWhiteSpace(registration.PasswordConfirm)) || registration.Password != registration.PasswordConfirm) return Unauthorized();

			var newUser = new ApplicationUser
			{
				Email = registration.Email,
				UserName = registration.Email,
				DisplayName = registration.DisplayName
			};

			var result = await _userManager.CreateAsync(newUser, registration.Password);

			return Ok(result.Succeeded);
		}
	}
}