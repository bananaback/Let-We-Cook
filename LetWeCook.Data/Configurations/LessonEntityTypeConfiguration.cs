using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class LessonEntityTypeConfiguration : IEntityTypeConfiguration<Lesson>
	{
		public void Configure(EntityTypeBuilder<Lesson> builder)
		{
			builder.ToTable("lesson");

			builder.HasKey(l => l.Id);

			builder.Property(l => l.Id)
				.HasColumnName("id");

			builder.Property<Guid>("CourseId")
				.HasColumnName("course_id");

			builder.Property(l => l.Title)
				.HasColumnName("title");

			builder.Property(l => l.IsTrial)
				.HasColumnName("is_trial");

			builder.Property(l => l.DateCreated)
				.HasColumnName("date_created");

			builder.HasMany(l => l.LessonContents)
				.WithOne(lc => lc.Lesson)
				.HasForeignKey("LessonId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasMany(l => l.Quizzes)
				.WithOne(q => q.Lesson)
				.HasForeignKey("LessonId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
