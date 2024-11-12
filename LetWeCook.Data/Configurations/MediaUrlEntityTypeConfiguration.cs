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


            builder.HasOne(mu => mu.Ingredient)
                .WithOne(i => i.CoverImageUrl)
                .HasForeignKey<Ingredient>("MediaUrlId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);

            builder.HasOne(mu => mu.Recipe)
                .WithOne(r => r.RecipeCoverImage)
                .HasForeignKey<Recipe>("MediaUrlId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);

            builder.HasMany(mu => mu.IngredientSections)
                .WithOne(x => x.MediaUrl)
                .HasForeignKey("MediaUrlId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }
    }
}
