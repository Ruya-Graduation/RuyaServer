namespace RUYA_API.ExceptionHandling.Common
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;

        public IEnumerable<string>? Errors { get; set; }
    }
}
