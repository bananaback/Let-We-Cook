using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class RecipeReviewEntityTypeConfiguration : IEntityTypeConfiguration<RecipeReview>
	{
		public void Configure(EntityTypeBuilder<RecipeReview> builder)
		{
			builder.ToTable("recipe_review");

			builder.HasKey(rr => rr.Id);

			builder.Property(rr => rr.Id)
				.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("user_id");

			builder.Property<Guid>("RecipeId")
				.HasColumnName("recipe_id");

			builder.Property(rr => rr.Review)
				.HasColumnName("review");

			builder.Property(rr => rr.Rating)
				.HasColumnName("rating")
				.HasPrecision(18, 2);

			builder.Property(rr => rr.CreatedDate)
				.HasColumnName("created_date");

			builder.Property(rr => rr.IsPossitive)
				.HasColumnName("is_possitive");
		}
	}
}
