namespace LetWeCook.Data.Entities
{
	public class DishCollection
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!;
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime DateCreated { get; set; }
		public List<Recipe> Recipes { get; set; } = new List<Recipe>();
		public List<CollectionRecipe> CollectionRecipes { get; set; } = new List<CollectionRecipe>();
	}
}
