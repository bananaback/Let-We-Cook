﻿namespace LetWeCook.Web.Areas.Account.Models.Requests
{
	public class LoginRequest
	{
		public string UsernameOrEmail { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;

	}
}