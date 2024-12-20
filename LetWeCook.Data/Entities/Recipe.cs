using LetWeCook.Data.Enums;

namespace LetWeCook.Data.Entities
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Cuisine { get; set; } = string.Empty;
        public DifficultyEnum Difficulty { get; set; }
        public decimal CookTimeInMinutes { get; set; }
        public int Serving { get; set; }
        public ApplicationUser CreatedBy { get; set; } = null!;
        public MediaUrl? RecipeCoverImage { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal AverageRating { get; set; } = 0;

        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public List<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();
        public List<MealPlan> MealPlans { get; set; } = new List<MealPlan>();
        public List<MealPlanRecipe> MealPlanRecipes { get; set; } = new List<MealPlanRecipe>();
        public List<DishCollection> DishCollections { get; set; } = new List<DishCollection>();
        public List<CollectionRecipe> CollectionRecipes { get; set; } = new List<CollectionRecipe>();
        public List<RecipeReview> RecipeReviews { get; set; } = new List<RecipeReview>();
    }
}
