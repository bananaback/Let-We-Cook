using LetWeCook.Services.DTOs;

namespace LetWeCook.Web.Areas.Account.Models.ViewModels
{
    public class CollectionDetailsViewModel
    {
        public Guid CollectionId { get; set; }
        public string CollectionName { get; set; } = string.Empty;
        public string CollectionDescription { get; set; } = string.Empty;
        public List<RecipeDTO> Recipes { get; set; } = new List<RecipeDTO>();
    }
}
