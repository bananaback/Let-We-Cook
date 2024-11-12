using System.ComponentModel.DataAnnotations;

namespace LetWeCook.Web.Areas.Account.Models.ViewModels
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
	}

}
