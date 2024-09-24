namespace LetWeCook.Data.Entities
{
	public class QuestionOption
	{
		public Guid Id { get; set; }
		public QuizQuestion Question { get; set; } = null!;
		public string OptionText { get; set; } = string.Empty;
		public string OptionLetter { get; set; } = string.Empty;
		public bool IsCorrect { get; set; }
	}
}
