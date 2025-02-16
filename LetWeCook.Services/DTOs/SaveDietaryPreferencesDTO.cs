namespace LetWeCook.Services.DTOs
{
    public class SaveDietaryPreferencesDTO
    {
        public List<DietaryPreferenceDTO> Preferences { get; set; } = new();
    }
}