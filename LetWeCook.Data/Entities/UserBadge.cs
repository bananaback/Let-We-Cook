namespace LetWeCook.Data.Entities
{
	public class UserBadge
	{
		public ApplicationUser User { get; set; } = null!;
		public Badge Badge { get; set; } = null!;
		public DateTime DateEarned { get; set; }
	}
}
