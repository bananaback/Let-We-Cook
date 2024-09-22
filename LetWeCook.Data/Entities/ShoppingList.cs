namespace LetWeCook.Data.Entities
{
	public class ShoppingList
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!; // Required reference navigation to principal
														   // There I use shadow foreign key to reduce mem
		public DateTime DateCreated { get; set; }
		public bool IsCompleted { get; set; }

		public List<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();
	}
}
