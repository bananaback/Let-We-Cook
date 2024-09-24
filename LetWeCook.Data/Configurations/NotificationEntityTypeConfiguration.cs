using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
	{
		public void Configure(EntityTypeBuilder<Notification> builder)
		{
			builder.ToTable("notification");

			builder.HasKey(n => n.Id);

			builder.Property(n => n.Id)
				.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("user_id");

			builder.Property(n => n.NotificationType)
				.HasColumnName("notification_type")
				.HasConversion(
					v => v.ToString(),
					v => (NotificationTypeEnum)Enum.Parse(typeof(NotificationTypeEnum), v)
				);

			builder.Property(n => n.ReferenceId)
				.HasColumnName("reference_id");

			builder.Property(n => n.Message)
				.HasColumnName("message");

			builder.Property(n => n.IsRead)
				.HasColumnName("is_read");

			builder.Property(n => n.DateCreated)
				.HasColumnName("date_created");
		}
	}
}
