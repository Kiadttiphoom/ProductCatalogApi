namespace ProductCatalogApi.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public T? Data { get; set; }
    public long ElapsedMilliseconds { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, long elapsedMs, string message = "Success")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            ElapsedMilliseconds = elapsedMs
        };
    }

    public static ApiResponse<T> ErrorResponse(string message, long elapsedMs = 0)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            ElapsedMilliseconds = elapsedMs
        };
    }
}

public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = [];
}
