using LetWeCook.Data.Enums;

namespace LetWeCook.Data.Entities
{
	public class Notification
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!;
		public NotificationTypeEnum NotificationType { get; set; }
		public Guid ReferenceId { get; set; }
		public string Message { get; set; } = string.Empty;
		public bool IsRead { get; set; }
		public DateTime DateCreated { get; set; }

	}
}
