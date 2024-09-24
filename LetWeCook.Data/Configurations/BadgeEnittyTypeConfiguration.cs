using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class BadgeEnittyTypeConfiguration : IEntityTypeConfiguration<Badge>
	{
		public void Configure(EntityTypeBuilder<Badge> builder)
		{
			builder.ToTable("badge");

			builder.HasKey(b => b.Id);

			builder.Property(b => b.Id)
				.HasColumnName("id");

			builder.Property(b => b.Name)
				.HasColumnName("name");

			builder.Property(b => b.Description)
				.HasColumnName("description");

			builder.Property(b => b.DateCreated)
				.HasColumnName("date_created");
		}
	}
}
