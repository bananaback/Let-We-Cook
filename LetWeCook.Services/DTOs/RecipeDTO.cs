namespace LetWeCook.Services.DTOs
{
    public class RecipeDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Cuisine { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public decimal CookTimeInMinutes { get; set; }
        public int Serving { get; set; }
        public Guid CreatedBy { get; set; }
        public MediaUrlDTO RecipeCoverImage { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public List<IngredientDTO> IngredientDTOs { get; set; } = new List<IngredientDTO>();
        public List<StepDTO> StepDTOs { get; set; } = new List<StepDTO>();

    }
}
