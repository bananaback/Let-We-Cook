namespace LetWeCook.Services.DTOs
{
    public class RecipeIngredientDTO
    {
        public Guid RecipeId { get; set; }
        public Guid IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
