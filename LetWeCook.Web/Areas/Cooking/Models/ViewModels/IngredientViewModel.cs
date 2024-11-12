using LetWeCook.Services.DTOs;

namespace LetWeCook.Web.Areas.Cooking.Models.ViewModels
{
    public class IngredientViewModel
    {
        public List<IngredientDTO> Ingredients { get; set; } = new List<IngredientDTO>();

        // Pagination properties
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; } = 10; // Default to 10 items per page

        // Search/filtering
        public string SearchTerm { get; set; } = string.Empty;

        // Navigation properties for UI controls
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasFirstPage => CurrentPage > 1;
        public bool HasLastPage => CurrentPage < TotalPages;
    }

}
