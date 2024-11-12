namespace LetWeCook.Web.Models.Configs
{
	public class AppSettings
	{
		public SmtpSettings SmtpSettings { get; set; } = new SmtpSettings();
		public string CloudinaryUrl { get; set; } = string.Empty;
		public string ConnectionString { get; set; } = string.Empty;
	}
}
