namespace LetWeCook.Services.Exceptions
{
    public class IngredientCreationException : Exception
    {
        public IngredientCreationException() { }
        public IngredientCreationException(string message) : base(message) { }
        public IngredientCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
