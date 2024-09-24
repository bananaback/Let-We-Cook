namespace LetWeCook.Data.Entities
{
	public class RecipeReview
	{
		public Guid Id { get; set; }
		public ApplicationUser User { get; set; } = null!;
		public Recipe Recipe { get; set; } = null!;
		public string Review = string.Empty;
		public decimal Rating { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsPossitive { get; set; }
	}
}
