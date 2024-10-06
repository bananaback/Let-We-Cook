using LetWeCook.Web.Areas.Account.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace LetWeCook.Web.Areas.Account.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		[CustomPasswordValidation]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
	}

}
