using LetWeCook.Data.Enums;

namespace LetWeCook.Data.Entities
{
	public class MealPlan
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!;
		public DateTime Date { get; set; }
		public MealTypeEnum MealType { get; set; }
		public List<Recipe> Recipes { get; set; } = new List<Recipe>();
		public List<MealPlanRecipe> MealPlanRecipes { get; set; } = new List<MealPlanRecipe>();
	}
}