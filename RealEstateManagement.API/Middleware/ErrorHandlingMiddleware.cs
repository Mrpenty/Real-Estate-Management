using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
namespace RealEstateManagement.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // đi tiếp đến middleware hoặc controller tiếp theo
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                var errorDetails = MapExceptionToErrorDetails(ex);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = errorDetails.StatusCode;

                await context.Response.WriteAsync(errorDetails.ToString());
            }
        }
        private ErrorDetails MapExceptionToErrorDetails(Exception ex)
        {
            return ex switch
            {
                KeyNotFoundException => new ErrorDetails
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                },
                ArgumentException => new ErrorDetails
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                },
                UnauthorizedAccessException => new ErrorDetails
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                },
                _ => new ErrorDetails
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "Đã xảy ra lỗi không xác định.",
                    StackTrace = ex.StackTrace
                }
            };
        }


    }
}
