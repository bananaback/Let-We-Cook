namespace LetWeCook.Data.Entities
{
	public class QuizQuestion
	{
		public Guid Id { get; set; }
		public Quiz Quiz { get; set; } = null!;
		public string QuestionText { get; set; } = string.Empty;
		public string CorrectAnswer { get; set; } = string.Empty;

		public List<QuestionOption> QuestionOptions { get; set; } = new List<QuestionOption>();
	}
}
