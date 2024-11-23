using LetWeCook.Services.DTOs;

namespace LetWeCook.Web.Areas.Cooking.Models.ViewModels
{
    public class RecipeViewModel
    {
        public List<RecipeDTO> Recipes { get; set; } = new List<RecipeDTO>();

        // Pagination properties
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; } = 10; // Default to 10 items per page

        // Search/filtering properties
        public string SearchTerm { get; set; } = string.Empty;
        public string Cuisine { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int CookTime { get; set; } = 0;
        public int Servings { get; set; } = 0;
        public string SortBy { get; set; } = string.Empty;

        // Navigation properties for UI controls
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasFirstPage => CurrentPage > 1;
        public bool HasLastPage => CurrentPage < TotalPages;
    }
}
