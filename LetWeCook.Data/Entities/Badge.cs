namespace LetWeCook.Data.Entities
{
	public class Badge
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime DateCreated { get; set; }

		public List<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
		public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
	}
}
