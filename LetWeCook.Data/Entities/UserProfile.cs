using LetWeCook.Data.Enums;

namespace LetWeCook.Data.Entities
{
	public class UserProfile
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!;
		public string PhoneNumber { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public int Age { get; set; }
		public GenderEnum Gender { get; set; }
		public string Address { get; set; } = string.Empty;

		public List<DietaryPreference> DietaryPreference { get; set; } = new List<DietaryPreference>();
		public List<UserDietaryPreference> UserDietaryPreferences { get; set; } = new List<UserDietaryPreference>();

	}
}
