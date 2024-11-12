using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
    public class RecipeEntityTypeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("recipe");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("id");

            builder.Property(r => r.Title)
                .HasColumnName("title");

            builder.Property(r => r.Description)
                .HasColumnName("description");

            builder.Property(r => r.Cuisine)
                .HasColumnName("cuisine");

            builder.Property(r => r.Difficulty)
                .HasColumnName("difficulty")
                .HasConversion(
                    v => v.ToString(),
                    v => (DifficultyEnum)Enum.Parse(typeof(DifficultyEnum), v)
                );

            builder.Property(r => r.CookTimeInMinutes)
                .HasColumnName("cook_time")
                .HasPrecision(18, 2);

            builder.Property(r => r.Serving)
                .HasColumnName("serving");

            builder.Property<Guid?>("MediaUrlId")
                .HasColumnName("recipe_cover_image");

            builder.Property<Guid>("ApplicationUserId")
                .HasColumnName("created_by");

            builder.Property(r => r.DateCreated)
                .HasColumnName("date_created");

            builder.HasMany(r => r.Ingredients)
                .WithMany(i => i.Recipes)
                .UsingEntity<RecipeIngredient>(
                    j => j.ToTable("recipe_ingredient")
                            .HasOne(j => j.Ingredient)
                            .WithMany(i => i.RecipeIngredients)
                            .HasForeignKey("IngredientId")
                            .OnDelete(DeleteBehavior.Cascade)
                            .IsRequired(),
                    j => j.HasOne(j => j.Recipe)
                            .WithMany(r => r.RecipeIngredients)
                            .HasForeignKey("RecipeId")
                            .OnDelete(DeleteBehavior.Cascade)
                            .IsRequired(),
                    j =>
                    {
                        j.Property<Guid>("IngredientId")
                            .HasColumnName("ingredient_id");

                        j.Property<Guid>("RecipeId")
                            .HasColumnName("recipe_id");

                        j.HasKey("IngredientId", "RecipeId");

                        j.Property(j => j.Quantity)
                            .HasColumnName("quantity")
                            .HasPrecision(18, 2);

                        j.Property(j => j.Unit)
                            .HasColumnName("unit")
                            .HasConversion(
                                v => v.ToString(),
                                v => (UnitEnum)Enum.Parse(typeof(UnitEnum), v)
                            );
                    }
                );

            builder.HasMany(r => r.RecipeSteps)
                .WithOne(rs => rs.Recipe)
                .HasForeignKey("RecipeId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(r => r.RecipeReviews)
                .WithOne(rr => rr.Recipe)
                .HasForeignKey("RecipeId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
