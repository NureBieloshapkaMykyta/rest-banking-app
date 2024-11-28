namespace Bank.Server.Shared.Helpers;

public class Result<T>
{
    public T? Data { get; set; }

    public bool IsSuccessful { get; set; }

    public string? Message { get; set; }

    public Result(bool isSuccessful, T data)
    {
        IsSuccessful = isSuccessful;
        Data = data;
    }

    public Result(bool isSuccessful, string message)
    {
        IsSuccessful = isSuccessful;
        Message = message;
    }

    public Result(bool isSuccessful)
    {
        IsSuccessful = isSuccessful;
    }
}
