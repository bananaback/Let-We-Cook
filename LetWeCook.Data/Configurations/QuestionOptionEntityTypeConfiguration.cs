using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class QuestionOptionEntityTypeConfiguration : IEntityTypeConfiguration<QuestionOption>
	{
		public void Configure(EntityTypeBuilder<QuestionOption> builder)
		{
			builder.ToTable("question_option");

			builder.HasKey(qo => qo.Id);

			builder.Property(qo => qo.Id)
				.HasColumnName("id");

			builder.Property<Guid>("QuestionId")
				.HasColumnName("question_id");

			builder.Property(qo => qo.OptionText)
				.HasColumnName("option_text");

			builder.Property(qo => qo.OptionLetter)
				.HasColumnName("option_letter");

			builder.Property(qo => qo.IsCorrect)
				.HasColumnName("is_correct");
		}
	}
}
