using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LetWeCook.Data.Configurations
{
	public class LessonContentEntityTypeConfiguration : IEntityTypeConfiguration<LessonContent>
	{
		public void Configure(EntityTypeBuilder<LessonContent> builder)
		{
			builder.ToTable("lesson_content");

			builder.HasKey(lc => lc.Id);

			builder.Property(lc => lc.Id)
				.HasColumnName("id");

			builder.Property<Guid>("LessionId")
				.HasColumnName("lesson_id");

			builder.Property(lc => lc.ContentType)
				.HasColumnName("content_type")
				.HasConversion(
					v => v.ToString(),
					v => (ContentTypeEnum)Enum.Parse(typeof(ContentTypeEnum), v)
				);
			builder.Property(lc => lc.ContentData)
				.HasColumnName("content_data");

			builder.Property(lc => lc.OrderNumber)
				.HasColumnName("order_number");
		}
	}
}
