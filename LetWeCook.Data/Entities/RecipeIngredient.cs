using LetWeCook.Data.Enums;

namespace LetWeCook.Data.Entities
{
	public class RecipeIngredient
	{
		public Recipe Recipe { get; set; } = null!;
		public Ingredient Ingredient { get; set; } = null!;
		public decimal Quantity { get; set; }
		public UnitEnum Unit { get; set; }
	}
}
