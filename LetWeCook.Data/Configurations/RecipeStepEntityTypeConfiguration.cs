using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class RecipeStepEntityTypeConfiguration : IEntityTypeConfiguration<RecipeStep>
	{
		public void Configure(EntityTypeBuilder<RecipeStep> builder)
		{
			builder.ToTable("recipe_step");

			builder.HasKey(rs => rs.Id);

			builder.Property(rs => rs.Id)
					.HasColumnName("id");

			builder.Property<Guid>("RecipeId")
					.HasColumnName("recipe_id");

			builder.Property(rs => rs.StepNumber)
					.HasColumnName("step_number");

			builder.Property(rs => rs.Instruction)
					.HasColumnName("instruction");

			builder.HasMany(rs => rs.MediaUrls)
				.WithMany(mu => mu.RecipeSteps)
				.UsingEntity<RecipeStepMedia>(
					j => j.ToTable("recipe_step_media")
						.HasOne(j => j.MediaUrl)
						.WithMany(mu => mu.RecipeStepsMedia)
						.HasForeignKey("MediaUrlId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired(),
					j => j.HasOne(j => j.RecipeStep)
						.WithMany(rs => rs.RecipeStepMedias)
						.HasForeignKey("RecipeStepId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired(),
					j =>
					{
						j.Property<Guid>("RecipeStepId")
							.HasColumnName("recipe_step_id");

						j.Property<Guid>("MediaUrlId")
							.HasColumnName("media_url_id");

						j.HasKey("RecipeStepId", "MediaUrlId");
					}
				);

		}
	}
}
