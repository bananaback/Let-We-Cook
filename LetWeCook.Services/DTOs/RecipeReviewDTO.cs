namespace LetWeCook.Services.DTOs
{
    public class RecipeReviewDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string UserAvatarUrl { get; set; } = string.Empty;
        public string Review = string.Empty;
        public decimal Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPositive => Rating > 2.5m;
    }
}
