using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
    public class IngredientSectionEntityTypeConfiguration : IEntityTypeConfiguration<IngredientSection>
    {
        public void Configure(EntityTypeBuilder<IngredientSection> builder)
        {
            builder.ToTable("ingredient_section");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property<Guid>("IngredientId")
                .HasColumnName("ingredient_id");

            builder.Property<Guid?>("MediaUrlId")
                .HasColumnName("media_url_id");

            builder.Property(x => x.TextContent)
                .HasColumnName("text_content");

            builder.Property(x => x.Order)
                .HasColumnName("section_order");

        }
    }
}
