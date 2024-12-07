using LetWeCook.Data.Entities;

namespace LetWeCook.Services.DTOs
{
    public class CollectionRecipeDTO
    {
        public DishCollection Collection { get; set; } = null!;

        public Recipe Recipe { get; set; } = null!;
        public DateTime DateAdded { get; set; }
    }
}
