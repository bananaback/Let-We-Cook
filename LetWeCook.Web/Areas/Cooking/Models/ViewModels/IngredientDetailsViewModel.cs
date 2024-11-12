using LetWeCook.Services.DTOs;

namespace LetWeCook.Web.Areas.Cooking.Models.ViewModels
{
    public class IngredientDetailsViewModel
    {
        public Guid Id { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string IngredientDescription { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public List<FrameDTO> Frames { get; set; } = new List<FrameDTO>();
    }

}
