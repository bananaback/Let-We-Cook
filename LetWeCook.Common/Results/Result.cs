using LetWeCook.Common.Enums;

namespace LetWeCook.Common.Results
{
	public class Result
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; } = string.Empty;
		public Exception? Exception { get; set; }
		public ErrorCode ErrorCode { get; set; } = ErrorCode.None;

		public static Result Success(string message = "")
		{
			return new Result { IsSuccess = true, Message = message };
		}

		public static Result Failure(string message, ErrorCode errorCode, Exception? exception = null)
		{
			return new Result { IsSuccess = false, Message = message, ErrorCode = errorCode, Exception = exception };
		}
	}
}