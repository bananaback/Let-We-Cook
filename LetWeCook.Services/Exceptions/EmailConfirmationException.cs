namespace LetWeCook.Services.Exceptions
{
    public class EmailConfirmationException : Exception
    {
        public EmailConfirmationException() { }
        public EmailConfirmationException(string message) : base(message) { }
        public EmailConfirmationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
