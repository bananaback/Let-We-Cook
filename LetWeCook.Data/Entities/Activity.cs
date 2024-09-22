using LetWeCook.Data.Enums;

namespace LetWeCook.Data.Entities
{
	public class Activity
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!;
		public ActivityTypeEnum ActivityType { get; set; }
		public Guid ReferenceId { get; set; }
		public DateTime DateCreated { get; set; }

		public List<Feed> Feeds { get; set; } = new List<Feed>();
	}
}
