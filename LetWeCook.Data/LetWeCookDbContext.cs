using LetWeCook.Data.Configurations;
using LetWeCook.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data
{
    public class LetWeCookDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<MediaUrl> MediaUrls { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientSection> IngredientSections { get; set; }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<DishCollection> DishCollections { get; set; }
        public DbSet<CollectionRecipe> CollectionRecipes { get; set; }
        public DbSet<RecipeReview> RecipeReviews { get; set; }
        public DbSet<DietaryPreference> DietaryPreferences { get; set; }
        public DbSet<UserDietaryPreference> UserDietaryPreferences { get; set; }
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
            new IngredientSectionEntityTypeConfiguration().Configure(modelBuilder.Entity<IngredientSection>());
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
