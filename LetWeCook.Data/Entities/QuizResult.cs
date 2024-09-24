namespace LetWeCook.Data.Entities
{
	public class QuizResult
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!;
		public Quiz Quiz { get; set; } = null!;
		public decimal Score { get; set; }
		public DateTime DateCompleted { get; set; }
	}
}
