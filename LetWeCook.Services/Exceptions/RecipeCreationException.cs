namespace LetWeCook.Services.Exceptions
{
    public class RecipeCreationException : Exception
    {
        public RecipeCreationException() { }
        public RecipeCreationException(string message) : base(message) { }
        public RecipeCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
