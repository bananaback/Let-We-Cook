namespace LetWeCook.Data.Entities
{
	public class RecipeStep
	{
		public Guid Id { get; set; }
		public Recipe Recipe { get; set; } = null!;
		public int StepNumber { get; set; }
		public string Instruction { get; set; } = string.Empty;

		public List<MediaUrl> MediaUrls { get; set; } = new List<MediaUrl>();
		public List<RecipeStepMedia> RecipeStepMedias { get; set; } = new List<RecipeStepMedia>();
	}
}
