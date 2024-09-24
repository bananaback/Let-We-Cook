using LetWeCook.Data.Enums;

namespace LetWeCook.Data.Entities
{
	public class LessonContent
	{
		public Guid Id { get; set; }
		public Lesson Lesson { get; set; } = null!;
		public ContentTypeEnum ContentType { get; set; }
		public string ContentData { get; set; } = string.Empty;
		public int OrderNumber { get; set; }
	}
}
