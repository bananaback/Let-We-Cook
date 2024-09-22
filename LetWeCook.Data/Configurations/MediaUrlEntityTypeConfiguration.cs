using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class MediaUrlEntityTypeConfiguration : IEntityTypeConfiguration<MediaUrl>
	{
		public void Configure(EntityTypeBuilder<MediaUrl> builder)
		{
			builder.ToTable("media_url");

			builder.HasKey(mu => mu.Id);

			builder.Property(mu => mu.Id)
				.HasColumnName("id");

			builder.Property(mu => mu.Url)
				.HasColumnName("url");

			builder.Property(mu => mu.Alt)
				.HasColumnName("alt");
		}
	}
}
