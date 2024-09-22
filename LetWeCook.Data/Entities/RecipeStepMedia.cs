namespace LetWeCook.Data.Entities
{
	public class RecipeStepMedia
	{
		public RecipeStep RecipeStep { get; set; } = null!;
		public MediaUrl MediaUrl { get; set; } = null!;
	}
}
