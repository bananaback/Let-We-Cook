using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class ShoppingListEntityTypeConfiguration : IEntityTypeConfiguration<ShoppingList>
	{
		public void Configure(EntityTypeBuilder<ShoppingList> builder)
		{
			builder.ToTable("shopping_list");

			builder.HasKey(sl => sl.Id);

			builder.Property(sl => sl.Id)
				.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("user_id");

			builder.Property(sl => sl.DateCreated)
				.HasColumnName("date_created");

			builder.Property(sl => sl.IsCompleted)
				.HasColumnName("is_completed");

			builder.HasMany(sl => sl.ShoppingListItems)
				.WithOne(sli => sli.ShoppingList)
				.HasForeignKey("ShoppingListId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
