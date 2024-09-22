using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class DishCollectionEntityTypeConfiguration : IEntityTypeConfiguration<DishCollection>
	{
		public void Configure(EntityTypeBuilder<DishCollection> builder)
		{
			builder.ToTable("dish_collection");

			builder.HasKey(dc => dc.Id);

			builder.Property(dc => dc.Id)
				.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("user_id");

			builder.Property(dc => dc.Name)
				.HasColumnName("name");

			builder.Property(dc => dc.Description)
				.HasColumnName("description");

			builder.HasMany(dc => dc.Recipes)
				.WithMany(r => r.DishCollections)
				.UsingEntity<CollectionRecipe>(
					j => j.ToTable("collection_recipe")
						.HasOne(cr => cr.Recipe)
						.WithMany(r => r.CollectionRecipes)
						.HasForeignKey("RecipeId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired(),
					j => j.HasOne(cr => cr.Collection)
						.WithMany(dc => dc.CollectionRecipes)
						.HasForeignKey("DishCollectionId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired(),
					j =>
					{
						j.Property<Guid>("RecipeId")
							.HasColumnName("recipe_id");

						j.Property<Guid>("DishCollectionId")
							.HasColumnName("collection_id");

						j.Property(j => j.DateAdded)
							.HasColumnName("date_added");
					}
				);
		}
	}
}
