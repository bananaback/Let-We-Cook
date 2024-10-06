namespace LetWeCook.Web.Models.Configs
{
	public class AuthenticationConfiguration
	{
		public GoogleAuthenticationConfiguration Google { get; set; } = new GoogleAuthenticationConfiguration();
		public FacebookAuthenticationConfiguration Facebook { get; set; } = new FacebookAuthenticationConfiguration();
	}
}
