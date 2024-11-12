using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using LetWeCook.Services.Models.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace LetWeCook.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager, ILogger<AuthenticationService> logger)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IdentityResult> RegisterUserAsync(string username, string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateEmail",
                    Description = "Email is already taken."
                });
            }
            else
            {
                existingUser = await _userManager.FindByNameAsync(username);
                if (existingUser != null)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "DuplicateUsername",
                        Description = "Username is already taken."
                    });
                }
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = username,
                Email = email,
            };

            IdentityResult result;

            try
            {
                result = await _userManager.CreateAsync(user, password);

            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "Exception",
                    Description = $"An error occurred: {ex.Message}."
                });
            }

            if (result.Succeeded)
            {
                // Check if the email matches the admin email
                if (email.Equals("votrongtin882003@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
                    if (!roleResult.Succeeded)
                    {
                        return IdentityResult.Failed(roleResult.Errors.ToArray());
                    }
                }
                else
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, UserRole.User.ToString());
                    if (!roleResult.Succeeded)
                    {
                        return IdentityResult.Failed(roleResult.Errors.ToArray());
                    }
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                {
                    throw new InvalidOperationException("Cannot generate URL: HttpContext is not available.");
                }

                var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext
                {
                    HttpContext = httpContext,
                    RouteData = httpContext.GetRouteData(),
                    ActionDescriptor = new ControllerActionDescriptor()
                });

                var callbackUrl = urlHelper.Action(
                    "ConfirmEmail",
                    "Auth",
                    new { userId = user.Id, code = code },
                    protocol: httpContext.Request.Scheme
                );

                await _emailSender.SendEmailAsync(email, "Confirm your email",
                    $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");
            }

            return result;
        }

        public async Task<Result> ConfirmEmailAsync(string userId, string code)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Result.Failure("User not found", ErrorCode.UserNotFound);
                }

                var identityResult = await _userManager.ConfirmEmailAsync(user, code);

                if (identityResult.Succeeded)
                {
                    return Result.Success("Email confirmed successfully");
                }
                else
                {
                    // Return failure with error code and error details from IdentityResult
                    var errorMessage = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                    return Result.Failure($"Email confirmation failed: {errorMessage}", ErrorCode.EmailConfirmationFailed);
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return Result.Failure("An error occurred during email confirmation", ErrorCode.EmailConfirmationException, ex);
            }
        }

        public async Task<SignInResult> LoginUserAsync(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
        }

        public async Task LogUserOut()
        {
            await _signInManager.SignOutAsync();
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, string? returnUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
            return properties;
        }

        public async Task<ExternalLoginResult> ExternalLoginAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogError("No external login info found");
                return ExternalLoginResult.NoInfoFailure();
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? "";
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var createUserResult = await CreateNewUserAsync(email, info);
                if (!createUserResult.IsSuccess)
                {
                    return createUserResult;
                }
                user = createUserResult.User;
            }
            else
            {
                var userWithExternalProvider = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (userWithExternalProvider == null)
                {
                    var addLoginResult = await AddExternalLoginToUserAsync(user, info);
                    if (!addLoginResult.IsSuccess)
                    {
                        return addLoginResult;
                    }
                }
            }

            // Add picture claim if available
            await AddPictureClaimAsync(user!, info);

            var loginResult = await SignInUserWithExternalLoginAsync(info);
            return loginResult;
        }

        private async Task<ExternalLoginResult> CreateNewUserAsync(string email, ExternalLoginInfo info)
        {
            var newUser = new ApplicationUser { UserName = email, Email = email };
            var createUserResult = await _userManager.CreateAsync(newUser);

            if (!createUserResult.Succeeded)
            {
                _logger.LogError($"Failed to create user {email}");
                return ExternalLoginResult.CreateNewUserFailure();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            await _userManager.ConfirmEmailAsync(newUser, token);

            // Quick and dirty role assignment
            IdentityResult roleResult;
            if (email.Equals("votrongtin882003@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                roleResult = await _userManager.AddToRoleAsync(newUser, UserRole.Admin.ToString());
            }
            else
            {
                roleResult = await _userManager.AddToRoleAsync(newUser, UserRole.User.ToString());
            }

            if (!roleResult.Succeeded)
            {
                _logger.LogError($"Failed to assign role to user {email}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }

            var addLoginResult = await AddExternalLoginToUserAsync(newUser, info);
            return addLoginResult.IsSuccess ? ExternalLoginResult.Success(newUser) : addLoginResult;
        }

        private async Task<ExternalLoginResult> AddExternalLoginToUserAsync(ApplicationUser user, ExternalLoginInfo info)
        {
            var addExternalLoginProviderResult = await _userManager.AddLoginAsync(user, info);
            if (!addExternalLoginProviderResult.Succeeded)
            {
                _logger.LogError($"Failed to add external login for user {user.Email}");
                return ExternalLoginResult.CreateExternalProviderFailure();
            }
            return ExternalLoginResult.Success(user);
        }

        private async Task<ExternalLoginResult> SignInUserWithExternalLoginAsync(ExternalLoginInfo info)
        {
            var loginResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
            return loginResult.Succeeded ? ExternalLoginResult.Success() : ExternalLoginResult.ExternalLoginFailure();
        }

        private async Task AddPictureClaimAsync(ApplicationUser user, ExternalLoginInfo info)
        {
            var pictureClaim = info.Principal.FindFirstValue("picture") ?? "/images/default-profile-picture.png"; // Use default picture if none found

            var currentClaims = await _userManager.GetClaimsAsync(user);
            var existingPictureClaim = currentClaims.FirstOrDefault(c => c.Type == "picture");

            if (existingPictureClaim != null)
            {
                var replaceClaimResult = await _userManager.ReplaceClaimAsync(user, existingPictureClaim, new Claim("picture", pictureClaim));
                if (!replaceClaimResult.Succeeded)
                {
                    _logger.LogError($"Failed to replace picture claim for user {user.Email}");
                }
            }
            else
            {
                var addClaimResult = await _userManager.AddClaimAsync(user, new Claim("picture", pictureClaim));
                if (!addClaimResult.Succeeded)
                {
                    _logger.LogError($"Failed to add picture claim for user {user.Email}");
                }
            }
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Do not reveal that the user does not exist or is not confirmed
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Check if HttpContext is null
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("Cannot generate URL: HttpContext is not available.");
            }

            var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext
            {
                HttpContext = httpContext,
                RouteData = httpContext.GetRouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            });

            var callbackUrl = urlHelper.Action("ResetPassword", "Auth", new { token, email }, protocol: httpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(email, "Reset Password",
                $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            return true;
        }

        public async Task<bool> ResetUserPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
