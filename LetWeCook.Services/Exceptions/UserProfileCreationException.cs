namespace LetWeCook.Services.Exceptions
{
    public class UserProfileCreationException : Exception
    {
        public UserProfileCreationException() { }
        public UserProfileCreationException(string message) : base(message) { }
        public UserProfileCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
