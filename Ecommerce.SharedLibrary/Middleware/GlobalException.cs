

using Ecommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Ecommerce.SharedLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            {
                string message = "Internal server error.Please Try again";
                int statusCode = (int)HttpStatusCode.InternalServerError;
                string title = "Error";


                try
                {
                    await next(context);
                    if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                    {
                        title = "Warning";
                        statusCode = (int)HttpStatusCode.TooManyRequests;
                        message = "Too many requests";
                        await ModifyHeader(context, title, statusCode, message);
                    }
                    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                    {
                        title = "Forbidden";
                        statusCode = (int)HttpStatusCode.Forbidden;
                        message = "You do not have permission to access this resource.";
                        await ModifyHeader(context, title, statusCode, message);
                    }
                    else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                    {
                        title = "Unauthorized";
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        message = "Authentication is required to access this resource.";
                        await ModifyHeader(context, title, statusCode, message);
                    }

                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                    if (ex is TaskCanceledException || ex is TimeoutException)
                    {
                        title = "Request Timeout";
                        statusCode = (int)HttpStatusCode.RequestTimeout;
                        message = "The request was canceled or timed out. Please try again later.";
                        await ModifyHeader(context, title, statusCode, message);
                    }

                    await ModifyHeader(context, title, statusCode, message);
                }
            }

        }

        private async Task ModifyHeader(HttpContext context, string title, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = title
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
            return;
        }
    }
}
