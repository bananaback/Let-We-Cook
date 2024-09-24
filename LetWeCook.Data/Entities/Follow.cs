namespace LetWeCook.Data.Entities
{
	public class Follow
	{
		public Guid Id { get; set; }
		public ApplicationUser Follower { get; set; } = null!;
		public ApplicationUser Followed { get; set; } = null!;
		public DateTime DateFollowed { get; set; }
	}
}
