namespace RUYA_API.Responses;

public static class ResponseFactory
{
    public static ApiResponse<T> Success<T>(T data, string message)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<object> Success(string message)
    {
        return new ApiResponse<object>
        {
            Success = true,
            Message = message
        };
    }

    public static ApiResponse<object> Failure(string message, List<string>? errors = null)
    {
        return new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}