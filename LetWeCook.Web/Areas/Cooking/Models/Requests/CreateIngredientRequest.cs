using LetWeCook.Web.Areas.Cooking.Models.DTOs;

namespace LetWeCook.Web.Areas.Cooking.Models.Requests
{
    public class CreateIngredientRequest
    {
        public string IngredientName { get; set; } = string.Empty;
        public string IngredientDescription { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public List<RawFrameDTO> RawFrameDTOs { get; set; } = new List<RawFrameDTO>();
    }
}
