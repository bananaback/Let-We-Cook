using LetWeCook.Services.RecipeServices;
using Microsoft.AspNetCore.Mvc;

namespace LetWeCook.Web.ViewComponents
{
    public class VietnameseRecipesViewComponent : ViewComponent
    {
        private readonly IRecipeService _recipeService;

        public VietnameseRecipesViewComponent(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count, CancellationToken cancellationToken)
        {
            // Fetch the Vietnamese recipes using the service
            var recipes = await _recipeService.GetRecipesByCuisineAsync("Vietnamese", count, cancellationToken);

            // In case of failure, return an empty list or handle error gracefully
            return View(recipes);
        }
    }
}
