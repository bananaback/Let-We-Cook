using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class RecipeIngredientEntityTypeConfiguration : IEntityTypeConfiguration<RecipeIngredient>
	{
		public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
		{
			builder.ToTable("recipe_ingredient");
		}
	}
}
