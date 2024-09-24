using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class QuizQuestionEntityTypeConfiguration : IEntityTypeConfiguration<QuizQuestion>
	{
		public void Configure(EntityTypeBuilder<QuizQuestion> builder)
		{
			builder.ToTable("quiz_question");

			builder.HasKey(qq => qq.Id);

			builder.Property(qq => qq.Id)
				.HasColumnName("id");

			builder.Property<Guid>("QuizId")
				.HasColumnName("quiz_id");

			builder.Property(qq => qq.QuestionText)
				.HasColumnName("question_text");

			builder.Property(qq => qq.CorrectAnswer)
				.HasColumnName("correct_answer");

			builder.HasMany(qq => qq.QuestionOptions)
				.WithOne(qo => qo.Question)
				.HasForeignKey("QuestionId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
