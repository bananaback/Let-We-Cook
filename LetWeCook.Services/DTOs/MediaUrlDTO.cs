namespace LetWeCook.Services.DTOs
{
    public class MediaUrlDTO
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
    }
}
