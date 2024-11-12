using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
    public class IngredientEntityTypeConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("ingredient");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnName("id");

            builder.Property(i => i.Name)
                .HasColumnName("name");

            builder.Property(i => i.Description)
                .HasColumnName("description");

            builder.HasMany(i => i.ShoppingListItems)
                .WithOne(sli => sli.Ingredient)
                .HasForeignKey("IngredientId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property<Guid?>("MediaUrlId")
                .HasColumnName("media_url_id");

            builder.HasMany(i => i.IngredientSections)
                .WithOne(x => x.Ingredient)
                .HasForeignKey("IngredientId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
