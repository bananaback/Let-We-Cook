namespace LetWeCook.Data.Entities
{
	public class CollectionRecipe
	{
		public DishCollection Collection { get; set; } = null!;
		public Recipe Recipe { get; set; } = null!;
		public DateTime DateAdded { get; set; }
	}
}
