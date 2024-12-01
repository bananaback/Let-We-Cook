namespace LetWeCook.Services.Exceptions
{
    public class ImageUploadException : Exception
    {
        public ImageUploadException() { }
        public ImageUploadException(string message) : base(message) { }
        public ImageUploadException(string message, Exception innerException) : base(message, innerException) { }
    }
}
