using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class UserProfileEntityTypeConfiguration : IEntityTypeConfiguration<UserProfile>
	{
		public void Configure(EntityTypeBuilder<UserProfile> builder)
		{
			builder.ToTable("user_profile");

			builder.HasKey(up => up.Id);

			builder.Property(up => up.Id)
				.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("user_id");

			builder.Property(up => up.PhoneNumber)
				.HasColumnName("phone_number");

			builder.Property(up => up.FirstName)
				.HasColumnName("first_name");

			builder.Property(up => up.LastName)
				.HasColumnName("last_name");

			builder.Property(up => up.Age)
				.HasColumnName("age");

			builder.Property(up => up.Gender)
				.HasConversion(
					v => v.ToString(),
					v => (GenderEnum)Enum.Parse(typeof(GenderEnum), v)
				)
				.HasColumnName("gender");

			builder.Property(up => up.Address)
				.HasColumnName("address");

			builder.HasMany(up => up.DietaryPreference)
				.WithMany(dp => dp.UserProfiles)
				.UsingEntity<UserDietaryPreference>(
					j => j.ToTable("user_dietary_preference")
							.HasOne(udp => udp.DietaryPreference)
							.WithMany(dp => dp.UserDietaryPreferences)
							.HasForeignKey("DietaryPreferenceId")
							.OnDelete(DeleteBehavior.Restrict)
							.IsRequired(),
					j => j.HasOne(udp => udp.UserProfile)
							.WithMany(up => up.UserDietaryPreferences)
							.HasForeignKey("UserProfileId")
							.OnDelete(DeleteBehavior.Restrict)
							.IsRequired(),
					j =>
					{
						j.Property<Guid>("DietaryPreferenceId")
							.HasColumnName("preference_id");
						j.Property<Guid>("UserProfileId")
							.HasColumnName("user_profile_id");
						j.HasKey("DietaryPreferenceId", "UserProfileId");
					}

				);
		}
	}
}
