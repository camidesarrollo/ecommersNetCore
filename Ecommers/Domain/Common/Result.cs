namespace Ecommers.Domain.Common
{
    public class Result
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; } = string.Empty;

        public static Result Ok(string message = "") =>
            new() { Success = true, Message = message };

        public static Result Fail(string message) =>
            new() { Success = false, Message = message };
    }

    public class Result<T> : Result
    {
        public T? Data { get; private set; }

        public static Result<T> Ok(T data, string message = "") =>
            new() { Success = true, Data = data, Message = message };

        public static new Result<T> Fail(string message) =>
            new() { Success = false, Message = message };
    }
}
