using LetWeCook.Web.Areas.Account.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace LetWeCook.Web.Areas.Account.Models.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Required]
		[CustomPasswordValidation]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;
		[Required]
		public string Username { get; set; } = string.Empty;
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}

}
