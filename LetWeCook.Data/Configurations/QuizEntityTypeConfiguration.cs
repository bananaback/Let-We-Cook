using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class QuizEntityTypeConfiguration : IEntityTypeConfiguration<Quiz>
	{
		public void Configure(EntityTypeBuilder<Quiz> builder)
		{
			builder.ToTable("quiz");

			builder.HasKey(q => q.Id);

			builder.Property(q => q.Id)
				.HasColumnName("id");

			builder.Property<Guid>("LessonId")
				.HasColumnName("lesson_id");

			builder.Property(q => q.Title)
				.HasColumnName("title");

			builder.Property(q => q.DateCreated)
				.HasColumnName("date_created");

			builder.HasMany(q => q.QuizQuestions)
				.WithOne(qq => qq.Quiz)
				.HasForeignKey("QuizId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasMany(q => q.QuizResults)
				.WithOne(qr => qr.Quiz)
				.HasForeignKey("QuizId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
