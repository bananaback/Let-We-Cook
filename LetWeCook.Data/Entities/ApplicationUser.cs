using Microsoft.AspNetCore.Identity;

namespace LetWeCook.Data.Entities
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public bool IsRemoved { get; set; }
		public decimal Balance { get; set; }

		public UserProfile? UserProfile { get; set; }
		public List<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
		public List<Recipe> Recipes { get; set; } = new List<Recipe>();

		public List<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
		public List<DishCollection> DishCollection { get; set; } = new List<DishCollection>();
		public List<Feed> Feeds { get; set; } = new List<Feed>();
		public List<Activity> Activities { get; set; } = new List<Activity>();
	}
}
