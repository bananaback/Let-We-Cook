using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class FeedEntityTypeConfiguration : IEntityTypeConfiguration<Feed>
	{
		public void Configure(EntityTypeBuilder<Feed> builder)
		{
			builder.ToTable("feed");

			builder.HasKey(f => f.Id);

			builder.Property(f => f.Id)
				.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("user_id");

			builder.Property<Guid>("ActivityId")
				.HasColumnName("activity_id");

			builder.Property(f => f.DateAdded)
				.HasColumnName("date_added");
		}
	}
}
