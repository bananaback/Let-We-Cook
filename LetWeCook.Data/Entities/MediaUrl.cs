namespace LetWeCook.Data.Entities
{
	public class MediaUrl
	{
		public Guid Id { get; set; }
		public string Url { get; set; } = string.Empty;
		public string Alt { get; set; } = string.Empty;

		public List<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();
		public List<RecipeStepMedia> RecipeStepsMedia { get; set; } = new List<RecipeStepMedia>();
	}
}
