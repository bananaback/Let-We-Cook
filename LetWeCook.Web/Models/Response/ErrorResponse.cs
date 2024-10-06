namespace LetWeCook.Web.Models.Response
{
	public class ErrorResponse
	{
		public string ErrorMessage { get; set; } = string.Empty;
		public ErrorResponse(string errorMessage)
		{
			ErrorMessage = errorMessage;
		}
	}
}
