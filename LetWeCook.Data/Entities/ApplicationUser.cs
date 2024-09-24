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

		public List<RecipeReview> RecipeReviews { get; set; } = new List<RecipeReview>();
		public List<Follow> Followers { get; set; } = new List<Follow>();
		public List<Follow> Followings { get; set; } = new List<Follow>();

		public List<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
		public List<Badge> Badges { get; set; } = new List<Badge>();

		public List<Notification> Notifications { get; set; } = new List<Notification>();
		public List<Course> TaughtCourses { get; set; } = new List<Course>();
		public List<Course> EnrolledCourses { get; set; } = new List<Course>();
		public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
		public List<QuizResult> QuizResults { get; set; } = new List<QuizResult>();
	}
}
