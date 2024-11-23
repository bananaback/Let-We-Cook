using LetWeCook.Services.RecipeServices;
using Microsoft.AspNetCore.Mvc;

namespace LetWeCook.Web.ViewComponents
{
    public class RandomRecipesViewComponent : ViewComponent
    {
        private readonly IRecipeService _recipeService;

        // Constructor dependency injection
        public RandomRecipesViewComponent(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        // The method that handles the logic of getting the newest recipes
        public async Task<IViewComponentResult> InvokeAsync(int count = 5, CancellationToken cancellationToken = default)
        {
            var result = await _recipeService.GetRandomRecipesAsync(count, cancellationToken);

            // If the service fails to fetch recipes, return an error view
            if (!result.IsSuccess)
            {
                return View("Error"); // Handle failure
            }

            // Return the recipes data to the Default view
            return View(result.Data);
        }
    }
}
