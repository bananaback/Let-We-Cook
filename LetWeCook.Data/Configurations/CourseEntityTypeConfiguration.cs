using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
	{
		public void Configure(EntityTypeBuilder<Course> builder)
		{
			builder.ToTable("course");

			builder.HasKey(c => c.Id);

			builder.Property(c => c.Id)
				.HasColumnName("id");

			builder.Property(c => c.Title)
				.HasColumnName("title");

			builder.Property(c => c.Description)
				.HasColumnName("description");

			builder.Property(c => c.Price)
				.HasColumnName("price")
				.HasPrecision(18, 2);

			builder.Property<Guid>("ApplicationUserId")
				.HasColumnName("instructor_id");

			builder.Property(c => c.IsPublished)
				.HasColumnName("is_published");

			builder.Property(c => c.Certificate)
				.HasColumnName("certificate");

			builder.Property(c => c.DateCreated)
				.HasColumnName("date_created");

			builder.HasMany(c => c.Lessons)
				.WithOne(l => l.Course)
				.HasForeignKey("CourseId")
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();
		}
	}
}
