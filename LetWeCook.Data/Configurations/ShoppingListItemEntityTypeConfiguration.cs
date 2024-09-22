using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class ShoppingListItemEntityTypeConfiguration : IEntityTypeConfiguration<ShoppingListItem>
	{
		public void Configure(EntityTypeBuilder<ShoppingListItem> builder)
		{
			builder.ToTable("shopping_list_item");

			builder.HasKey(sli => sli.Id);

			builder.Property(sli => sli.Id)
				.HasColumnName("id");

			builder.Property(sli => sli.Quantity)
				.HasColumnName("quantity")
				.HasColumnType("decimal(18,2)");

			builder.Property<Guid>("ShoppingListId")
				.HasColumnName("list_id");

			builder.Property<Guid>("IngredientId")
				.HasColumnName("ingredient_id");

			builder.Property(sli => sli.Unit)
				.HasColumnName("unit");

			builder.Property(sli => sli.Unit)
					.HasConversion(
						v => v.ToString(),
						v => (UnitEnum)Enum.Parse(typeof(UnitEnum), v)
					);

			builder.Property(sli => sli.IsPurchased)
				.HasColumnName("is_purchased");
		}
	}
}
