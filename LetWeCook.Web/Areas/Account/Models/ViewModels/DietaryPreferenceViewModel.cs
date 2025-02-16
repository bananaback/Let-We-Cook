namespace LetWeCook.Web.Areas.Account.Models.ViewModels
{
    public class DietaryPreferenceViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsSelected { get; set; } // Flag indicating if the user has this preference
    }
}