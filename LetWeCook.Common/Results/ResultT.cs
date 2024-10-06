using LetWeCook.Common.Enums;

namespace LetWeCook.Common.Results
{
	public class Result<T>
	{
		public bool IsSuccess { get; private set; }
		public string Message { get; private set; } = string.Empty;
		public T? Data { get; private set; } // Non-nullable T, ensures success always has Data
		public Exception? Exception { get; private set; }
		public ErrorCode ErrorCode { get; private set; } = ErrorCode.None;

		// Success factory method with data
		public static Result<T> Success(T data, string message = "")
		{
			if (data == null) throw new ArgumentNullException(nameof(data), "Data cannot be null in a successful result.");

			return new Result<T>
			{
				IsSuccess = true,
				Data = data,
				Message = message
			};
		}

		// Failure factory method
		public static Result<T> Failure(string message, ErrorCode errorCode, Exception? exception = null)
		{
			return new Result<T>
			{
				IsSuccess = false,
				Message = message,
				ErrorCode = errorCode,
				Exception = exception
			};
		}
	}


}
