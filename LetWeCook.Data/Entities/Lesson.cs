namespace LetWeCook.Data.Entities
{
	public class Lesson
	{
		public Guid Id { get; set; }
		public Course Course { get; set; } = null!;
		public string Title { get; set; } = string.Empty;
		public bool IsTrial { get; set; }
		public DateTime DateCreated { get; set; }

		public List<LessonContent> LessonContents { get; set; } = new List<LessonContent>();
		public List<Quiz> Quizzes { get; set; } = new List<Quiz>();
	}
}
