using LetWeCook.Services.DTOs;

namespace LetWeCook.Web.Areas.Cooking.Models.Requests
{
    public class CreateRecipeRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CoverImageId { get; set; } = string.Empty;
        public string Cuisine { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public decimal CookingTimeInMinutes { get; set; }
        public int Serving { get; set; }
        public List<RecipeIngredientDTO> RecipeIngredientDTOs { get; set; } = new List<RecipeIngredientDTO>();
        public List<StepDTO> StepDTOs { get; set; } = new List<StepDTO>();

    }
}
