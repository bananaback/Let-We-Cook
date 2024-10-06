using Microsoft.AspNetCore.Identity;

namespace LetWeCook.Services.PasswordHashers
{
	public class BCryptPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
	{
		public string HashPassword(TUser user, string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);

		}

		public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
		{
			// Verify the password against the stored hash
			if (BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword))
			{
				return PasswordVerificationResult.Success;
			}
			return PasswordVerificationResult.Failed;
		}
	}
}
