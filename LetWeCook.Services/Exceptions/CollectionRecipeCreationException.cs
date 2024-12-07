namespace LetWeCook.Services.Exceptions
{
    public class CollectionRecipeCreationException : Exception
    {
        public CollectionRecipeCreationException() { }
        public CollectionRecipeCreationException(string message) : base(message) { }
        public CollectionRecipeCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
