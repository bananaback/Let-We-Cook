using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class QuizResultEntityTypeConfiguration : IEntityTypeConfiguration<QuizResult>
	{
		public void Configure(EntityTypeBuilder<QuizResult> builder)
		{
			builder.ToTable("quiz_result");

			builder.HasKey(qr => qr.Id);

			builder.Property(qr => qr.Id)
				.HasColumnName("id");

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("user_id");

			builder.Property<Guid>("QuizId")
				.HasColumnName("quiz_id");

			builder.Property(qr => qr.Score)
				.HasColumnName("score")
				.HasPrecision(18, 2);

			builder.Property(qr => qr.DateCompleted)
				.HasColumnName("date_completed");
		}
	}
}
