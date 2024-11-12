using LetWeCook.Common.Results;
using LetWeCook.Web.Areas.Account.Models.ViewModels;
using LetWeCook.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = LetWeCook.Services.AuthenticationServices.IAuthenticationService;

namespace LetWeCook.Web.Areas.Account.Controllers
{
	[Area("Account")]
	public class AuthController : Controller
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly ILogger<AuthController> _logger;

		public AuthController(IAuthenticationService authenticationService, ILogger<AuthController> logger)
		{
			_authenticationService = authenticationService;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> Login()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ForgotPasswordConfirmation(string email)
		{
			ViewData["Email"] = email;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _authenticationService.SendPasswordResetEmailAsync(model.Email);
			if (result)
			{
				return RedirectToAction("ForgotPasswordConfirmation", "Auth", new { area = "Account", email = model.Email });
			}
			else
			{
				// Handle the case where sending email fails
				ModelState.AddModelError(string.Empty, "Failed to send reset password email. Please try again later.");
				return View(model);
			}
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LoginAsync(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _authenticationService.LoginUserAsync(model.Email, model.Password, false);

			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home", new { area = "" });
			}

			if (result.IsLockedOut)
			{
				ModelState.AddModelError(string.Empty, "This account has been locked out, please try again later.");
			}
			else if (result.IsNotAllowed)
			{
				ModelState.AddModelError(string.Empty, "Email not confirmed. Please check your email and confirm your account.");
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			}

			return View(model);
		}



		[HttpGet]
		public async Task<IActionResult> Register()
		{
			return View();
		}

		[HttpGet]
		public IActionResult EmailConfirmed()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			if (userId == null || code == null)
			{
				return View("EmailConfirmError");
			}

			Result result = await _authenticationService.ConfirmEmailAsync(userId, code);
			if (result.IsSuccess)
			{
				return View("EmailConfirmed");
			}
			else
			{
				ViewData["ErrorMessage"] = result.Exception?.Message;
				return View("EmailConfirmError");
			}
		}


		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _authenticationService.RegisterUserAsync(model.Username, model.Email, model.Password);


			if (result.Succeeded)
			{
				ViewData["Email"] = model.Email;
				return View("RegisterSuccess");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return View(model);

		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _authenticationService.LogUserOut();
			await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			return RedirectToAction("Index", "Home", new { area = "" });
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public IActionResult ExternalLogin(string provider, string? returnUrl)
		{
			AuthenticationProperties? properties = null;
			// Request a redirect to the external login provider with Challenge()
			// redirectUrl is the url that I should redirect the user after successfully authenticated user in "my app"
			// redirecrUrl in this method != google callback path in program.cs
			var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
			properties = _authenticationService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

			return Challenge(properties!, provider);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
		{
			if (remoteError != null)
			{
				_logger.LogError($"Error from external provider: {remoteError}");
				return RedirectToAction(nameof(Login));
			}

			var result = await _authenticationService.ExternalLoginAsync();

			if (result.IsSuccess)
			{
				// Redirect to the specified return URL or the home page if not specified
				return RedirectToLocal(returnUrl);
			}
			else
			{
				// Log the error message if any and redirect to the login page
				_logger.LogError(result.ErrorMessage);
				return RedirectToAction(nameof(Login));
			}
		}

		// Helper method to redirect to a local URL or default
		private IActionResult RedirectToLocal(string? returnUrl)
		{
			if (returnUrl == null)
			{
				return RedirectToAction("Index", "Home", new { area = "" });
			}
			return Redirect(returnUrl);
		}

		[HttpGet]
		public IActionResult ResetPassword(string token, string email)
		{
			if (token == null || email == null)
			{
				var requestId = HttpContext.TraceIdentifier;
				var errorViewModel = new ErrorViewModel
				{
					RequestId = requestId
				};
				return View("Error", errorViewModel);
			}
			ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel { Token = token, Email = email };
			return View(resetPasswordViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _authenticationService.ResetUserPasswordAsync(model.Email, model.Token, model.NewPassword);

			return RedirectToAction("ResetPasswordConfirmation", "Auth", new { area = "Account", success = result });
		}

		[HttpGet]
		public IActionResult ResetPasswordConfirmation(bool success)
		{
			ViewData["Success"] = success;
			return View();
		}
	}
}
