namespace LetWeCook.Services.Exceptions
{
    public class DishCollectionCreationException : Exception
    {
        public DishCollectionCreationException() { }
        public DishCollectionCreationException(string message) : base(message) { }
        public DishCollectionCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
