namespace LetWeCook.Data.Entities
{
	public class UserDietaryPreference
	{
		public UserProfile UserProfile { get; set; } = null!;
		public DietaryPreference DietaryPreference { get; set; } = null!;
	}
}
