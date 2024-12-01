namespace LetWeCook.Services.Exceptions
{
    public class MediaUrlCreationException : Exception
    {
        public MediaUrlCreationException() { }
        public MediaUrlCreationException(string message) : base(message) { }
        public MediaUrlCreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
