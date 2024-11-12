namespace LetWeCook.Services.DTOs
{
    public class FrameDTO
    {
        public Guid Id { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public string TextContent { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
