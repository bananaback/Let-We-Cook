using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data.Configurations
{
	public class ActivityEntityTypeConfiguration : IEntityTypeConfiguration<Activity>
	{
		public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Activity> builder)
		{
			builder.ToTable("activity");

			builder.HasKey(a => a.Id);

			builder.Property(a => a.Id)
				.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("user_id");

			builder.Property(a => a.ActivityType)
				.HasColumnName("activity_type")
				.HasConversion(
					v => v.ToString(),
					v => (ActivityTypeEnum)Enum.Parse(typeof(ActivityTypeEnum), v)
				);

			builder.Property(a => a.ReferenceId)
				.HasColumnName("reference_id");

			builder.Property(a => a.DateCreated)
				.HasColumnName("date_created");

			builder.HasMany(a => a.Feeds)
				.WithOne(f => f.Activity)
				.HasForeignKey("ActivityId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
