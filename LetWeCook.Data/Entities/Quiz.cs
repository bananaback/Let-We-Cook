namespace LetWeCook.Data.Entities
{
	public class Quiz
	{
		public Guid Id { get; set; }
		public Lesson Lesson { get; set; } = null!;
		public string Title { get; set; } = string.Empty;
		public DateTime DateCreated { get; set; }
		public List<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
		public List<QuizResult> QuizResults { get; set; } = new List<QuizResult>();
	}
}
