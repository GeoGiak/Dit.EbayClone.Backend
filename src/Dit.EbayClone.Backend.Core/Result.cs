namespace Dit.EbayClone.Backend.Core;

public class Result<T>
{
    public bool Success { get; private set; }
    public string? Error { get; private set; }
    public T? Data { get; private set; }

    private Result(bool success, T? data, string? error)
    {
        Success = success;
        Data = data;
        Error = error;
    }

    // Success factory
    public static Result<T> Ok(T data) => new Result<T>(true, data, null);

    // Success factory (no data)
    public static Result<T> Ok() => new Result<T>(true, default, null);

    // Failure factory
    public static Result<T> Fail(string error) => new Result<T>(false, default, error);
}