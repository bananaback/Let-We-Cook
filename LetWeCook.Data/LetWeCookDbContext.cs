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
			new CollectionRecipeEntityTypeConfiguration().Configure(modelBuilder.Entity<CollectionRecipe>());
			new DietaryPreferenceEntityTypeConfiguration().Configure(modelBuilder.Entity<DietaryPreference>());
			new DishCollectionEntityTypeConfiguration().Configure(modelBuilder.Entity<DishCollection>());
			new FeedEntityTypeConfiguration().Configure(modelBuilder.Entity<Feed>());
			new IngredientEntityTypeConfiguration().Configure(modelBuilder.Entity<Ingredient>());
			new MealPlanEntityTypeConfiguration().Configure(modelBuilder.Entity<MealPlan>());
			new MealPlanRecipeEntityTypeConfiguration().Configure(modelBuilder.Entity<MealPlanRecipe>());
			new MediaUrlEntityTypeConfiguration().Configure(modelBuilder.Entity<MediaUrl>());
			new RecipeEntityTypeConfiguration().Configure(modelBuilder.Entity<Recipe>());
			new RecipeIngredientEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeIngredient>());
			new RecipeStepEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeStep>());
			new RecipeStepMediaEntityTypeConfiguration().Configure(modelBuilder.Entity<RecipeStepMedia>());
			new ShoppingListEntityTypeConfiguration().Configure(modelBuilder.Entity<ShoppingList>());
			new ShoppingListItemEntityTypeConfiguration().Configure(modelBuilder.Entity<ShoppingListItem>());
			new UserProfileEntityTypeConfiguration().Configure(modelBuilder.Entity<UserProfile>());
		}
	}
}
