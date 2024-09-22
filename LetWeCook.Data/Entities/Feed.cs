namespace LetWeCook.Data.Entities
{
	public class Feed
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!;
		public Activity Activity { get; set; } = null!;

		public DateTime DateAdded { get; set; }
	}
}
