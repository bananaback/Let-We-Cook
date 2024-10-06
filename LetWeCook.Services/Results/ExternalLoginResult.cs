using LetWeCook.Data.Entities;

namespace LetWeCook.Services.Models.Results
{
	public class ExternalLoginResult
	{
		public bool IsSuccess { get; private set; }
		public ApplicationUser? User { get; private set; }
		public string? ErrorMessage { get; private set; }

		private ExternalLoginResult(bool isSuccess, ApplicationUser? user = null, string? errorMessage = null)
		{
			IsSuccess = isSuccess;
			User = user;
			ErrorMessage = errorMessage;
		}

		public static ExternalLoginResult Success(ApplicationUser? user = null)
		{
			return new ExternalLoginResult(true, user);
		}

		public static ExternalLoginResult NoInfoFailure()
		{
			return new ExternalLoginResult(false, errorMessage: "No external login info found");
		}

		public static ExternalLoginResult CreateNewUserFailure()
		{
			return new ExternalLoginResult(false, errorMessage: "Failed to create new user");
		}

		public static ExternalLoginResult CreateExternalProviderFailure()
		{
			return new ExternalLoginResult(false, errorMessage: "Failed to add external provider");
		}

		public static ExternalLoginResult ExternalLoginFailure()
		{
			return new ExternalLoginResult(false, errorMessage: "External login failed");
		}
	}

}
