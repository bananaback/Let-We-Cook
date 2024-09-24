using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class FollowEntityTypeConfiguration : IEntityTypeConfiguration<Follow>
	{
		public void Configure(EntityTypeBuilder<Follow> builder)
		{
			builder.ToTable("follow");

			builder.HasKey(f => f.Id);

			builder.Property(f => f.Id)
				.HasColumnName("id");

			builder.Property<Guid>("FollowerId")
				.HasColumnName("follower_id");

			builder.Property<Guid>("FollowedId")
				.HasColumnName("followed_id");

			builder.Property(f => f.DateFollowed)
				.HasColumnName("date_followed");
		}
	}
}
