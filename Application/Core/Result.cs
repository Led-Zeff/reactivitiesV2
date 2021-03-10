namespace Application.Core
{
    public enum ResultType
    {
        SUCCESS, NOT_FOUND, FAILURE
    }

    public class Result<T>
    {
        public ResultType Type { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }

        public static Result<T> Success(T value) => new Result<T>{ Type = ResultType.SUCCESS, Value = value };
        public static Result<T> Success() => new Result<T>{ Type = ResultType.SUCCESS };
        public static Result<T> NotFound() => new Result<T>{ Type = ResultType.NOT_FOUND };
        public static Result<T> Failure(string error) => new Result<T>{ Type = ResultType.FAILURE, Error = error };
    }
}