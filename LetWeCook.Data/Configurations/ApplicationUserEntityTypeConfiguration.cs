
using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.ToTable("application_user");

			builder.HasIndex(u => u.Email)
				.IsUnique();

			builder.Property(u => u.Id)
				.HasColumnName("id");

			builder.Property(u => u.IsRemoved)
				.HasColumnName("is_removed");

			builder.Property(u => u.Balance)
				.HasColumnName("balance")
				.HasColumnType("decimal(18,2)");


			builder.HasMany(u => u.ShoppingLists)
				.WithOne(sl => sl.User)
				.HasForeignKey("ApplicationUserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasOne(u => u.UserProfile)
				.WithOne(up => up.User)
				.HasForeignKey<UserProfile>("ApplicationUserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasMany(u => u.Recipes)
				.WithOne(r => r.CreatedBy)
				.HasForeignKey("ApplicationUserId")
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();

			builder.HasMany(u => u.MealPlans)
				.WithOne(ml => ml.User)
				.HasForeignKey("ApplicationUserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasMany(u => u.DishCollection)
				.WithOne(dc => dc.User)
				.HasForeignKey("ApplicationUserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasMany(u => u.Feeds)
				.WithOne(f => f.User)
				.HasForeignKey("ApplicationUserId")
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();

			builder.HasMany(u => u.Activities)
				.WithOne(a => a.User)
				.HasForeignKey("ApplicationUserId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
