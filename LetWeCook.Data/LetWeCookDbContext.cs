using LetWeCook.Common.DbContexts;
using LetWeCook.Data.Configurations;
using LetWeCook.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data
{
	public class LetWeCookDbContext : DbContext, IApplicationDbContext
	{
		public LetWeCookDbContext(DbContextOptions<LetWeCookDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
			{
				entity.HasKey(r => new { r.UserId, r.RoleId });
			});

			new ActivityEntityTypeConfiguration().Configure(modelBuilder.Entity<Activity>());
			new ApplicationUserEntityTypeConfiguration().Configure(modelBuilder.Entity<ApplicationUser>());
			new BadgeEnittyTypeConfiguration().Configure(modelBuilder.Entity<Badge>());
			new CollectionRecipeEntityTypeConfiguration().Configure(modelBuilder.Entity<CollectionRecipe>());
			new CourseEntityTypeConfiguration().Configure(modelBuilder.Entity<Course>());
			new DietaryPreferenceEntityTypeConfiguration().Configure(modelBuilder.Entity<DietaryPreference>());
			new DishCollectionEntityTypeConfiguration().Configure(modelBuilder.Entity<DishCollection>());
			new EnrollmentEntityTypeConfiguration().Configure(modelBuilder.Entity<Enrollment>());
			new FeedEntityTypeConfiguration().Configure(modelBuilder.Entity<Feed>());
			new FollowEntityTypeConfiguration().Configure(modelBuilder.Entity<Follow>());
			new IngredientEntityTypeConfiguration().Configure(modelBuilder.Entity<Ingredient>());
			new LessonContentEntityTypeConfiguration().Configure(modelBuilder.Entity<LessonContent>());
			new LessonEntityTypeConfiguration().Configure(modelBuilder.Entity<Lesson>());
			new MealPlanEntityTypeConfiguration().Configure(modelBuilder.Entity<MealPlan>());
			new MealPlanRecipeEntityTypeConfiguration().Configure(modelBuilder.Entity<MealPlanRecipe>());
			new MediaUrlEntityTypeConfiguration().Configure(modelBuilder.Entity<MediaUrl>());
			new NotificationEntityTypeConfiguration().Configure(modelBuilder.Entity<Notification>());
			new QuestionOptionEntityTypeConfiguration().Configure(modelBuilder.Entity<QuestionOption>());
			new QuizEntityTypeConfiguration().Configure(modelBuilder.Entity<Quiz>());
			new QuizQuestionEntityTypeConfiguration().Configure(modelBuilder.Entity<QuizQuestion>());
			new QuizResultEntityTypeConfiguration().Configure(modelBuilder.Entity<QuizResult>());
			new RecipeEntityTypeConfiguration().Configure(modelBuilder.Entity<Recipe>());
			new RecipeIngredientEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeIngredient>());
			new RecipeReviewEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeReview>());
			new RecipeStepEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeStep>());
			new RecipeStepMediaEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeStepMedia>());
			new ShoppingListEntityTypeConfiguration().Configure(modelBuilder.Entity<ShoppingList>());
			new ShoppingListItemEntityTypeConfiguration().Configure(modelBuilder.Entity<ShoppingListItem>());
			new UserBadgeEntityTypeConfiguration().Configure(modelBuilder.Entity<UserBadge>());
			new UserProfileEntityTypeConfiguration().Configure(modelBuilder.Entity<UserProfile>());
		}
	}
}
