﻿namespace LetWeCook.Data.Entities
{
	public class DietaryPreference
	{
		public Guid Id { get; set; }
		public string Value { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Color { get; set; } = string.Empty;
		public string Icon { get; set; } = string.Empty;

		public List<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
		public List<UserDietaryPreference> UserDietaryPreferences { get; set; } = new List<UserDietaryPreference>();
	}
}
