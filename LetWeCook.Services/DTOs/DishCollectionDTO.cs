namespace LetWeCook.Services.DTOs
{
    public class DishCollectionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public List<RecipeDTO> Recipes { get; set; } = new List<RecipeDTO>();
    }
}
