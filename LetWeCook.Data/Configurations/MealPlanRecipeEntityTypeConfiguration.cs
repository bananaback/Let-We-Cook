using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class MealPlanRecipeEntityTypeConfiguration : IEntityTypeConfiguration<MealPlanRecipe>
	{
		public void Configure(EntityTypeBuilder<MealPlanRecipe> builder)
		{
			builder.ToTable("meal_plan_recipe");
		}
	}
}
