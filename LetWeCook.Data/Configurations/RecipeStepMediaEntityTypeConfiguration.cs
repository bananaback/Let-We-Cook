using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class RecipeStepMediaEntityTypeConfiguration : IEntityTypeConfiguration<RecipeStepMedia>
	{
		public void Configure(EntityTypeBuilder<RecipeStepMedia> builder)
		{
			builder.ToTable("recipe_step_media");
		}
	}
}
