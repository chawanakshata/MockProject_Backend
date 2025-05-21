using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Mock_Project.Exceptions;

namespace Mock_Project.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception occurred.");

            int statusCode = 500;
            string message = "An unexpected error occurred.";
            string code = "InternalServerError";

            if (context.Exception is NotFoundException)
            {
                statusCode = 404;
                message = context.Exception.Message;
                code = "NotFound";
            }
            else if (context.Exception is BadRequestException)
            {
                statusCode = 400;
                message = context.Exception.Message;
                code = "BadRequest";
            }

            var errorResponse = new
            {
                Code = code,
                Message = message,
                Details = context.Exception.Message // Remove or hide in production
            };

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }

    }
}
