namespace LetWeCook.Services.DTOs
{
    public class DietaryPreferenceDTO
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsSelected { get; set; } = false;
    }
}