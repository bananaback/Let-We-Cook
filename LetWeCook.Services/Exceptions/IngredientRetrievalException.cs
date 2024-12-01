namespace LetWeCook.Services.Exceptions
{
    public class IngredientRetrievalException : Exception
    {
        public IngredientRetrievalException() { }
        public IngredientRetrievalException(string message) : base(message) { }
        public IngredientRetrievalException(string message, Exception innerException) : base(message, innerException) { }
    }
}
