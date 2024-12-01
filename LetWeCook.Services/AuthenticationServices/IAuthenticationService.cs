using LetWeCook.Services.Exceptions;
using LetWeCook.Services.Models.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace LetWeCook.Services.AuthenticationServices
{
    /// <summary>
    /// Provides authentication-related functionalities for the LetWeCook application.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Registers a new user with the specified username, email, and password.
        /// </summary>
        /// <param name="username">The username of the user to register.</param>
        /// <param name="email">The email address of the user to register.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the registration.</returns>
        Task<IdentityResult> RegisterUserAsync(string username, string email, string password);

        /// <summary>
        /// Confirms the user's email address using the provided user ID and confirmation code.
        /// </summary>
        /// <param name="userId">The ID of the user to confirm.</param>
        /// <param name="code">The confirmation code sent to the user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ConfirmEmailAsync(string userId, string code);

        /// <summary>
        /// Confirms the user's email address using the provided user ID and confirmation code.
        /// </summary>
        /// <param name="userId">The ID of the user to confirm.</param>
        /// <param name="code">The confirmation code sent to the user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="UserNotFoundException">Thrown if a user with the specified ID is not found.</exception>
        /// <exception cref="EmailConfirmationException">Thrown if the email confirmation fails due to errors.</exception>
        Task<SignInResult> LoginUserAsync(string email, string password, bool rememberMe);

        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task LogUserOut();

        /// <summary>
        /// Configures the properties required for external authentication.
        /// </summary>
        /// <param name="provider">The name of the external authentication provider.</param>
        /// <param name="returnUrl">The URL to redirect to after authentication.</param>
        /// <returns>The configured <see cref="AuthenticationProperties"/>.</returns>
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, string? returnUrl);

        /// <summary>
        /// Handles the login process for external authentication.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the external login attempt.</returns>
        Task<ExternalLoginResult> ExternalLoginAsync();

        /// <summary>
        /// Sends a password reset email to the specified email address.
        /// </summary>
        /// <param name="email">The email address of the user requesting a password reset.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the email was sent successfully.</returns>
        Task<bool> SendPasswordResetEmailAsync(string email);

        /// <summary>
        /// Resets the user's password using the specified email, reset token, and new password.
        /// </summary>
        /// <param name="email">The email address of the user resetting their password.</param>
        /// <param name="token">The password reset token provided to the user.</param>
        /// <param name="newPassword">The new password to set for the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the password reset was successful.</returns>
        Task<bool> ResetUserPasswordAsync(string email, string token, string newPassword);
    }
}
