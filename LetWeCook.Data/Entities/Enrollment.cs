namespace LetWeCook.Data.Entities
{
	public class Enrollment
	{
		public ApplicationUser User { get; set; } = null!;
		public Course Course { get; set; } = null!;
		public DateTime DateEnrolled { get; set; }
		public decimal Progress { get; set; }
	}
}
