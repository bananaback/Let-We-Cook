using LetWeCook.Web.Areas.Account.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace LetWeCook.Web.Areas.Account.Models.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required]
		[CustomPasswordValidation]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; } = string.Empty;

		[Required]
		[CustomPasswordValidation]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmNewPassword { get; set; } = string.Empty;

		[Required]
		public string Token { get; set; } = string.Empty;
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
	}

}
