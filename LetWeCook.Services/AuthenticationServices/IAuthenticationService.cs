using LetWeCook.Common.Results;
using LetWeCook.Services.Models.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace LetWeCook.Services.AuthenticationServices
{
	public interface IAuthenticationService
	{
		Task<IdentityResult> RegisterUserAsync(string username, string email, string password);
		Task<Result> ConfirmEmailAsync(string userId, string code);
		Task<SignInResult> LoginUserAsync(string email, string password, bool rememberMe);
		Task LogUserOut();
		AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, string? returnUrl);
		Task<ExternalLoginResult> ExternalLoginAsync();
		Task<bool> SendPasswordResetEmailAsync(string email);
		Task<bool> ResetUserPasswordAsync(string email, string token, string newPassword);
	}
}
