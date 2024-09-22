namespace LetWeCook.Data.Entities
{
	public class MealPlanRecipe
	{
		public MealPlan MealPlan { get; set; } = null!;
		public Recipe Recipe { get; set; } = null!;
		public int Servings { get; set; }
	}
}
