using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class CollectionRecipeEntityTypeConfiguration : IEntityTypeConfiguration<CollectionRecipe>
	{
		public void Configure(EntityTypeBuilder<CollectionRecipe> builder)
		{
			builder.ToTable("collection_recipe");
		}
	}
}
