using RUYA_API.ExceptionHandling.CustomException;
using RUYA_API.Responses;

namespace RUYA_API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is AppException appException)
            {
                context.Response.StatusCode = appException.StatusCode;

                await context.Response.WriteAsJsonAsync(
                    ResponseFactory.Failure(appException.Message));

                return;
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(
                ResponseFactory.Failure("An unexpected error occurred."));
        }
    }
}
