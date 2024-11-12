namespace LetWeCook.Data.Entities
{
    public class IngredientSection
    {
        public Guid Id { get; set; }
        public Ingredient Ingredient { get; set; } = null!;
        public MediaUrl? MediaUrl { get; set; }
        public string TextContent { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
