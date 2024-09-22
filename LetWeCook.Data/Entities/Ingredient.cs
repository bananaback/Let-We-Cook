namespace LetWeCook.Data.Entities
{
	public class Ingredient
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public List<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();
		public List<Recipe> Recipes { get; set; } = new List<Recipe>();
		public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
	}
}
