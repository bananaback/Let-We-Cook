using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class UserBadgeEntityTypeConfiguration : IEntityTypeConfiguration<UserBadge>
	{
		public void Configure(EntityTypeBuilder<UserBadge> builder)
		{
			builder.ToTable("user_badge");
		}
	}
}
