using LetWeCook.Services.RecipeServices;
using Microsoft.AspNetCore.Mvc;

namespace LetWeCook.Web.ViewComponents
{
    public class NewestRecipesViewComponent : ViewComponent
    {
        private readonly IRecipeService _recipeService;

        // Constructor dependency injection
        public NewestRecipesViewComponent(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        // The method that handles the logic of getting the newest recipes
        public async Task<IViewComponentResult> InvokeAsync(int count = 5, CancellationToken cancellationToken = default)
        {
            var recipes = await _recipeService.GetNewestRecipesAsync(count, cancellationToken);
            return View(recipes);
        }
    }
}
