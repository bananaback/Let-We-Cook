using LetWeCook.Data.Enums;

namespace LetWeCook.Data.Entities
{
	public class ShoppingListItem
	{
		public Guid Id { get; set; }
		public ShoppingList ShoppingList { get; set; } = null!;

		public Ingredient Ingredient { get; set; } = null!;
		public decimal Quantity { get; set; }
		public UnitEnum Unit { get; set; }

		public bool IsPurchased { get; set; }
	}
}
