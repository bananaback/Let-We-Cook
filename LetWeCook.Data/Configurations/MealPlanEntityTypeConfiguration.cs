using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class MealPlanEntityTypeConfiguration : IEntityTypeConfiguration<MealPlan>
	{
		public void Configure(EntityTypeBuilder<MealPlan> builder)
		{
			builder.ToTable("meal_plan");

			builder.HasKey(mp => mp.Id);

			builder.Property(mp => mp.Id)
				.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("user_id");

			builder.Property(mp => mp.Date)
				.HasColumnName("date");

			builder.Property(mp => mp.MealType)
				.HasColumnName("meal_type")
				.HasConversion(
					v => v.ToString(),
					v => (MealTypeEnum)Enum.Parse(typeof(MealTypeEnum), v)
				);

			builder.HasMany(ml => ml.Recipes)
				.WithMany(r => r.MealPlans)
				.UsingEntity<MealPlanRecipe>(
					j => j.ToTable("meal_plan_recipe")
						.HasOne(mpr => mpr.Recipe)
						.WithMany(r => r.MealPlanRecipes)
						.HasForeignKey("RecipeId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired(),
					j => j.HasOne(mpr => mpr.MealPlan)
						.WithMany(mp => mp.MealPlanRecipes)
						.HasForeignKey("MealPlanId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired(),
					j =>
					{
						j.Property<Guid>("MealPlanId")
							.HasColumnName("meal_id");

						j.Property<Guid>("RecipeId")
							.HasColumnName("recipe_id");

						j.HasKey("MealPlanId", "RecipeId");

						j.Property(j => j.Servings)
							.HasColumnName("servings");
					}
				);
		}
	}
}
