namespace LetWeCook.Services.Exceptions
{
    public class RecipeUpdateException : Exception
    {
        public RecipeUpdateException() { }
        public RecipeUpdateException(string message) : base(message) { }
        public RecipeUpdateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
