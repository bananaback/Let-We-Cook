namespace LetWeCook.Services.DTOs
{
    public class StepDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Order { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string ImageId { get; set; } = string.Empty;
        public string VideoId { get; set; } = string.Empty;
    }
}
