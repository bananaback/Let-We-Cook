namespace LetWeCook.Services.Exceptions
{
    public class UserProfileRetrievalException : Exception
    {
        public UserProfileRetrievalException() { }
        public UserProfileRetrievalException(string message) : base(message) { }
        public UserProfileRetrievalException(string message, Exception innerException) : base(message, innerException) { }
    }
}
