namespace LetWeCook.Web.Areas.Cooking.Models.DTOs
{
    public class RawFrameDTO
    {
        public string ContentType { get; set; } = string.Empty;
        public string ImageContent { get; set; } = string.Empty;
        public string TextContent { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
