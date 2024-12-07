namespace LetWeCook.Services.Exceptions
{
    public class DishCollectionRetrievalException : Exception
    {
        public DishCollectionRetrievalException() { }
        public DishCollectionRetrievalException(string message) : base(message) { }
        public DishCollectionRetrievalException(string message, Exception innerException) : base(message, innerException) { }
    }
}
